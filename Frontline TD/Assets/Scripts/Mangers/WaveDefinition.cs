using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "WaveDefinition", menuName = "FrontlineTD/WaveDefinition")]
public class WaveDefinition : ScriptableObject
{
    [System.Serializable]
    public class EnemyEntry
    {
        [Tooltip("0=Basico  1=Rapido  2=Tanque  3=Blindado  4=Camuflaje")]
        public int enemyTypeIndex;

        [Tooltip("Cantidad de este tipo en la oleada")]
        public int count;

        [Tooltip("Segundos entre spawns de este tipo")]
        public float spawnInterval = 1f;
    }

    [Tooltip("Enemigos que se spawnean en esta oleada, en el orden definido")]
    public List<EnemyEntry> enemies = new List<EnemyEntry>();
}
