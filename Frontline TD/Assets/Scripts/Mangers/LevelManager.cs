using UnityEngine;

public class LevelManager : MonoBehaviour
{
    /**
     * Singleton instance of the LevelManager.
     */
    public static LevelManager Instance;

    /**
     * The point where the enemies spawns.
     */
    public Transform enemySpawnPoint;

    /**
     * Array of points that the enemies follow.
     */
    public Transform[] pathPoints;

    [SerializeField] public int startingLives = 100;
    [SerializeField] public int startingMoney = 200;

    private void Awake()
    {
        // set the singleton instance
        Instance = this;
    }

    
}
