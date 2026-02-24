using System;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance;

    [Header("References")]
    /**
     * Array of tower prefabs that can be built by the player.
     */
    [SerializeField] private Tower[] towers;

    [Header("Attributes")]
    /**
     * The amount of money the player starts with.
     */
    [SerializeField] private int startingMoney = 100;

    /**
     * The current amount of money the player has.
     */
    private int currentMoney;

    /**
     * The index of the currently selected tower in the towerPrefabs array.
     */
    private int? selectedTowerIndex;

    void Awake()
    {
        // set the singleton instance
        Instance = this;
    }

    void Start()
    {
        SetMoney(startingMoney);
    }

    public int GetCurrentMoney()
    {
        return currentMoney;
    }

    private void SetMoney(int amount)
    {
        if (amount >= 0)
        {
            currentMoney = amount;
        }
        else
        {
            throw new ArgumentException("Money cannot be negative.");
        }
    }

    public void AddMoney(int amount)
    {
        SetMoney(currentMoney + amount);
    }

    public void SpendMoney(int amount)
    {
        try
        {
            SetMoney(currentMoney - amount);
        }
        catch (ArgumentException)
        {
            Debug.LogWarning("You don't have enough money.");
        }
    }

    public Tower GetSelectedTower()
    {
        if (selectedTowerIndex == null)
            return null;
        return towers[(int)selectedTowerIndex];
    }

    public void SetSelectedTower(int selectedTower)
    {
        selectedTowerIndex = selectedTower;
    }
}
