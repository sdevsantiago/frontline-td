using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;

    [Header("References")]
    /**
     * Los prefabs de enemigos en orden:
     * 0=Basico  1=Rapido  2=Tanque  3=Blindado  4=Camuflaje
     */
    [SerializeField] private GameObject[] enemyPrefabs;

    [Header("Intro Waves")]
    /**
     * Oleadas de introduccion definidas manualmente.
     * Cuando se agoten, el sistema pasa al modo infinito procedural.
     */
    [SerializeField] private WaveDefinition[] introWaves;

    [Header("Infinite Wave Attributes")]
    /**
     * Numero base de enemigos en el modo infinito.
     */
    [SerializeField] private int baseEnemyCount = 10;

    /**
     * Exponente de escalado de cantidad de enemigos por oleada.
     * Cuanto mayor, mas rapido crece el numero de enemigos.
     */
    [SerializeField] private float enemyCountExponent = 1f;

    /**
     * Intervalo por defecto entre spawns en el modo infinito.
     * Se reduce progresivamente con cada oleada hasta el minimo.
     */
    [SerializeField] private float spawnIntervalSeconds = 0.8f;

    /**
     * Intervalo minimo de spawn. No bajara de este valor aunque escale mucho.
     */
    [SerializeField] private float minSpawnIntervalSeconds = 0.08f;

    /**
     * Cuanto se reduce el intervalo de spawn por oleada infinita (en segundos).
     */
    [SerializeField] private float spawnIntervalReductionPerWave = 0.25f;

    /**
     * Multiplicador de vida maxima aplicado sobre el valor del prefab.
     * Escala con cada oleada infinita segun healthScaleExponent.
     */
    [SerializeField] private float baseHealthMultiplier = 1f;

    /**
     * Exponente de escalado de vida. Valores entre 0.1 y 0.5 dan un crecimiento suave.
     */
    [SerializeField] private float healthScaleExponent = 0.3f;

    /**
     * Multiplicador de velocidad aplicado sobre el valor del prefab.
     * Escala con cada oleada infinita segun speedScaleExponent.
     */
    [SerializeField] private float baseSpeedMultiplier = 1f;

    /**
     * Exponente de escalado de velocidad. Valores entre 0.05 y 0.2 dan un crecimiento suave.
     */
    [SerializeField] private float speedScaleExponent = 0.2f;

    /**
     * Tiempo en segundos entre oleadas.
     */
    [SerializeField] private float timeBetweenWavesSeconds = 5f;

    /**
     * Numero de oleada actual (empieza en 1).
     */
    private int currentWave = 0;

    /**
     * Indica si el spawner esta actualmente spawneando enemigos.
     */
    private bool isSpawning = false;

    // ─────────────────────────────────────────────
    // Tabla de composicion para el modo infinito.
    // Cada fila: { oleadaMinima, Basic, Rapido, Tanque, Blindado, Camuflaje }
    // Se aplica la fila con oleadaMinima mas alta que no supere la oleada actual.
    // Los pesos se normalizan automaticamente al calcular la distribucion.
    // ─────────────────────────────────────────────
    private static readonly int[][] compositionTable =
    {
        new[] {  1,  100,   0,   0,   0,   0 },
        new[] {  2,   60,  40,   0,   0,   0 },
        new[] {  3,   40,  30,  30,   0,   0 },
        new[] {  4,   25,  25,  25,  25,   0 },
        new[] {  5,   15,  20,  25,  20,  20 },
        new[] { 10,   10,  15,  20,  25,  30 },
        new[] { 20,    5,  10,  15,  30,  40 },
    };

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        StartCoroutine(WaveLoop());
    }

    // ─────────────────────────────────────────────
    // Bucle principal de oleadas
    // ─────────────────────────────────────────────

    private IEnumerator WaveLoop()
    {
        while (true)
        {
            currentWave++;
            isSpawning = true;

            Debug.Log("Wave " + currentWave + " started");

            int introIndex = currentWave - 1;
            if (introIndex < introWaves.Length && introWaves[introIndex] != null)
            {
                yield return StartCoroutine(SpawnIntroWave(introWaves[introIndex]));
            }
            else
            {
                // Oleada infinita: calculamos cuantas oleadas llevamos en modo infinito
                int infiniteWave = currentWave - introWaves.Length;
                yield return StartCoroutine(SpawnInfiniteWave(infiniteWave));
            }

            isSpawning = false;
            Debug.Log("Wave " + currentWave + " ended");

            yield return new WaitForSeconds(timeBetweenWavesSeconds);
        }
    }

    // ─────────────────────────────────────────────
    // Spawn oleada de introduccion (manual)
    // ─────────────────────────────────────────────

    private IEnumerator SpawnIntroWave(WaveDefinition wave)
    {
        foreach (WaveDefinition.EnemyEntry entry in wave.enemies)
        {
            int typeIndex = Mathf.Clamp(entry.enemyTypeIndex, 0, enemyPrefabs.Length - 1);
            float interval = entry.spawnInterval > 0f ? entry.spawnInterval : spawnIntervalSeconds;

            for (int i = 0; i < entry.count; i++)
            {
                SpawnEnemy(typeIndex);
                yield return new WaitForSeconds(interval);
            }
        }
    }

    // ─────────────────────────────────────────────
    // Spawn oleada infinita (procedural)
    // ─────────────────────────────────────────────

    private IEnumerator SpawnInfiniteWave(int infiniteWave)
    {
        int total = EnemiesPerWave(infiniteWave);
        int[] counts = DistributeEnemies(infiniteWave, total);

        float healthMult = GetHealthMultiplier(infiniteWave);
        float speedMult = GetSpeedMultiplier(infiniteWave);
        float baseInterval = GetCurrentSpawnInterval(infiniteWave);

        Debug.Log(string.Format(
            "Wave {0} (infinite {1}) | Total={2} | Basic={3} Rapido={4} Tanque={5} Blindado={6} Camuflaje={7} | HealthMult={8:F2} SpeedMult={9:F2} Interval={10:F2}",
            currentWave, infiniteWave, total,
            counts[0], counts[1], counts[2], counts[3], counts[4],
            healthMult, speedMult, baseInterval));

        List<int> spawnList = BuildSpawnList(infiniteWave, counts);

        foreach (int typeIndex in spawnList)
        {
            SpawnEnemy(typeIndex, healthMult, speedMult);
            float interval = GetSpawnInterval(typeIndex, baseInterval);
            yield return new WaitForSeconds(interval);
        }
    }

    // ─────────────────────────────────────────────
    // Helpers
    // ─────────────────────────────────────────────

    /**
     * Spawnea un enemigo y aplica los multiplicadores de dificultad
     * directamente sobre sus componentes Health y EnemyMovement.
     */
    private void SpawnEnemy(int typeIndex, float healthMult = 1f, float speedMult = 1f)
    {
        if (typeIndex < 0 || typeIndex >= enemyPrefabs.Length || enemyPrefabs[typeIndex] == null)
        {
            Debug.LogWarning("EnemySpawner: no hay prefab asignado para el tipo " + typeIndex);
            return;
        }

        GameObject enemy = Instantiate(
            enemyPrefabs[typeIndex],
            LevelManager.Instance.enemySpawnPoint.position,
            Quaternion.identity);

        Health health = enemy.GetComponent<Health>();
        if (health != null)
            health.SetMaxHealth(Mathf.RoundToInt(health.GetMaxHealth() * healthMult));

        EnemyMovement movement = enemy.GetComponent<EnemyMovement>();
        if (movement != null)
            movement.movementSpeed *= speedMult;
    }

    private int EnemiesPerWave(int infiniteWave)
    {
        return Mathf.RoundToInt(baseEnemyCount * Mathf.Pow(infiniteWave, enemyCountExponent));
    }

    /**
     * Multiplicador de vida para la oleada infinita dada.
     * Crece suavemente con un exponente configurable.
     */
    private float GetHealthMultiplier(int infiniteWave)
    {
        return baseHealthMultiplier * Mathf.Pow(infiniteWave, healthScaleExponent);
    }

    /**
     * Multiplicador de velocidad para la oleada infinita dada.
     */
    private float GetSpeedMultiplier(int infiniteWave)
    {
        return baseSpeedMultiplier * Mathf.Pow(infiniteWave, speedScaleExponent);
    }

    /**
     * Intervalo base de spawn para la oleada infinita dada.
     * Se reduce linealmente hasta el minimo configurado.
     */
    private float GetCurrentSpawnInterval(int infiniteWave)
    {
        float reduced = spawnIntervalSeconds - (spawnIntervalReductionPerWave * (infiniteWave - 1));
        return Mathf.Max(reduced, minSpawnIntervalSeconds);
    }

    private int[] DistributeEnemies(int infiniteWave, int total)
    {
        int[] weights = GetWeightsForWave(infiniteWave);
        int weightSum = 0;
        foreach (int w in weights) weightSum += w;

        int[] counts = new int[weights.Length];
        int assigned = 0;

        for (int i = 0; i < weights.Length; i++)
        {
            counts[i] = Mathf.RoundToInt((float)weights[i] / weightSum * total);
            assigned += counts[i];
        }

        // Corregir desvio de redondeo en el tipo dominante
        int drift = total - assigned;
        counts[GetDominantType(weights)] += drift;

        return counts;
    }

    /**
     * Construye la lista de spawn en orden:
     * primero una rafaga del tipo nuevo de esta oleada (para que sea visible),
     * luego el resto intercalado en round-robin.
     */
    private List<int> BuildSpawnList(int infiniteWave, int[] counts)
    {
        // Copia para no modificar el array original
        int[] remaining = (int[])counts.Clone();

        List<int> list = new List<int>();

        int newType = GetNewestTypeForWave(infiniteWave);
        if (newType >= 0 && remaining[newType] > 0)
        {
            int burst = Mathf.Min(remaining[newType], 3);
            for (int i = 0; i < burst; i++)
                list.Add(newType);
            remaining[newType] -= burst;
        }

        // Round-robin del resto
        bool anyLeft = true;
        while (anyLeft)
        {
            anyLeft = false;
            for (int t = 0; t < remaining.Length; t++)
            {
                if (remaining[t] > 0)
                {
                    list.Add(t);
                    remaining[t]--;
                    anyLeft = true;
                }
            }
        }

        return list;
    }

    private int[] GetWeightsForWave(int infiniteWave)
    {
        int[] result = compositionTable[0];
        foreach (int[] row in compositionTable)
        {
            if (infiniteWave >= row[0])
                result = row;
        }
        return new[] { result[1], result[2], result[3], result[4], result[5] };
    }

    /**
     * Devuelve el indice del tipo que aparece por primera vez en esta oleada infinita.
     * Devuelve -1 si no hay tipo nuevo.
     */
    private int GetNewestTypeForWave(int infiniteWave)
    {
        int[] prev = infiniteWave > 1 ? GetWeightsForWave(infiniteWave - 1) : new[] { 0, 0, 0, 0, 0 };
        int[] curr = GetWeightsForWave(infiniteWave);
        for (int i = 0; i < curr.Length; i++)
            if (curr[i] > 0 && prev[i] == 0)
                return i;
        return -1;
    }

    private int GetDominantType(int[] weights)
    {
        int max = -1, idx = 0;
        for (int i = 0; i < weights.Length; i++)
            if (weights[i] > max) { max = weights[i]; idx = i; }
        return idx;
    }

    /**
     * Intervalo de spawn personalizado por tipo, calculado sobre el intervalo
     * base de la oleada actual (ya reducido por dificultad).
     * Rapido y Camuflaje son mas agresivos; Tanque y Blindado mas pausados.
     */
    private float GetSpawnInterval(int typeIndex, float baseInterval)
    {
        switch (typeIndex)
        {
            case 0: return baseInterval;          // Basico
            case 1: return baseInterval * 0.5f;   // Rapido
            case 2: return baseInterval * 1.8f;   // Tanque
            case 3: return baseInterval * 1.5f;   // Blindado
            case 4: return baseInterval * 0.7f;   // Camuflaje
            default: return baseInterval;
        }
    }

    // ─────────────────────────────────────────────
    // API publica
    // ─────────────────────────────────────────────

    public int GetCurrentWave()
    {
        return currentWave;
    }

    public bool IsSpawning()
    {
        return isSpawning;
    }
}