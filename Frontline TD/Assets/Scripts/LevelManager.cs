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

    private void Awake()
    {
        // set the singleton instance
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
