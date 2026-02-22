using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Attributes")]
    /**
     * The maximum health of the object.
     */
    [SerializeField] private int maxHealth = 3;
    /**
     * The amount of money rewarded to the player when this object is destroyed.
     */
    [SerializeField] private int moneyReward = 50;

    /**
     * The current health of the object.
     */
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
        // reward the player with money for destroying this object
        BuildManager.Instance.AddMoney(moneyReward);
    }
}
