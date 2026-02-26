using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    private int currentLives;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        currentLives = LevelManager.Instance.startingLives;
    }

    public void TakeDamage(int damage)
    {
        currentLives -= damage;
        if (currentLives <= 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        Time.timeScale = 0f; // pause the game
    }
}
