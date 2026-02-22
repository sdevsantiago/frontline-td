using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Attributes")]
    /**
     * The maximum health of the object.
     */
    [SerializeField] private int maxHealth = 3;

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

    private void Die()
    {
        Destroy(gameObject);
    }
}
