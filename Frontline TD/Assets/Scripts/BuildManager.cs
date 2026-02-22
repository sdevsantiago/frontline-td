using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance;

    [Header("References")]
    /**
     * Array of tower prefabs that can be built by the player.
     */
    [SerializeField] private GameObject[] towerPrefabs;

    private int selectedTowerIndex = 0;

    private void Awake()
    {
        // set the singleton instance
        Instance = this;
    }

    public GameObject GetSelectedTower()
    {
        return towerPrefabs[selectedTowerIndex];
    }
}
