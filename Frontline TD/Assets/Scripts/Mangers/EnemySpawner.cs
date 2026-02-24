using System;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    /**
     * The enemy prefabs to spawn.
     */
    [SerializeField] private GameObject[] enemyPrefabs;

    [Header("Attributes")]
    /**
     * The base number of enemies to spawn.
     * The actual number of enemies will depend on the current wave and difficulty.
     */
    [SerializeField] private int baseEnemyCount = 5;
    /**
     * The interval in seconds between enemy spawns.
     */
    [SerializeField] private float spawnIntervalSeconds = 2f;
    /**
     * The time in seconds between waves.
     */
    [SerializeField] private float timeBetweenWavesSeconds = 5f;
    /**
     * The multiplier for the number of enemies spawned each wave.
     * The higher the multiplier, the more enemies will spawn each wave.
     */
    [SerializeField] private float difficultyMultiplier = 1f;

    /**
     * The current wave number.
     * The higher the wave, the more enemies will spawn.
     */
    private int currentWave = 1;

    /**
     * The time since the last enemy was spawned.
     */
    private float timeSinceLastSpawn;

    /**
     * The number of enemies left to spawn in the current wave.
     */
    private int enemiesLeftToSpawn;

    /**
     * Whether the spawner is currently spawning enemies.
     */
    private bool isSpawning = false;


    void Start()
    {
        StartWave();
    }

    void Update()
    {
        // ignore update loop if not currently spawning
        if (!isSpawning) return;

        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= spawnIntervalSeconds && enemiesLeftToSpawn > 0)
        {
            SpawnEnemy();
            timeSinceLastSpawn = 0;
            enemiesLeftToSpawn--;
        }

        if (enemiesLeftToSpawn == 0)
        {
            isSpawning = false;
            Debug.Log("Wave " + currentWave + " ended");
        }
    }

    void SpawnEnemy()
    {
        // define the enemy to spawn
        GameObject enemy = enemyPrefabs[0];
        // spawn the enemy at the spawn point
        Instantiate(enemy, LevelManager.Instance.enemySpawnPoint.position, Quaternion.identity);
    }

    void StartWave()
    {
        enemiesLeftToSpawn = EnemiesPerWave();
        isSpawning = true;
    }

    int EnemiesPerWave()
    {
        return Mathf.RoundToInt(baseEnemyCount * Mathf.Pow(currentWave, difficultyMultiplier));
    }
}
