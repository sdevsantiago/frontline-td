using System;
using UnityEngine;
using TMPro;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance;

    [Header("References")]
    /**
     * Array of tower prefabs that can be built by the player.
     */
    [SerializeField] private Tower[] towers;

    /**
     * The current amount of money the player has.
     */
    private int currentMoney;

    /**
     * The index of the currently selected tower in the towerPrefabs array.
     */
    private int selectedTowerIndex = -1;

    [SerializeField] private Transform plots;

    void Awake()
    {
        // set the singleton instance
        Instance = this;
    }

    void Start()
    {
        SetMoney(LevelManager.Instance.startingMoney);
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

    public void SetSelectedTower(int index)
    {
        if (index < 0 || index >= towers.Length || index == selectedTowerIndex)
        {
            selectedTowerIndex = -1;
            foreach (Transform child in plots)
            {
                if (child.childCount > 0)
                {
                    child.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = false;
                }
            }
            return;
        }
        foreach (Transform child in plots)
        {
            if (child.childCount > 0)
            {
                child.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = true;
            }
        }
        selectedTowerIndex = index;
    }

    public Tower GetSelectedTower()
    {
        if (selectedTowerIndex < 0)
            return null;

        return towers[selectedTowerIndex];
    }
}
