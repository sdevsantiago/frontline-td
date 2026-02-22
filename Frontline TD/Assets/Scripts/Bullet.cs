using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("References")]
    /**
     * The Rigidbody2D component of the bullet.
     */
    [SerializeField] private Rigidbody2D rigidBody;
    
    [Header("Attributes")]
    /**
     * The speed at which the bullet moves.
     */
    [SerializeField] private float bulletSpeed = 10f;
    /**
     * The damage the bullet deals to the enemy it hits.
     */
    [SerializeField] private int bulletDamage = 1;

    /**
     * The enemy the bullet is currently targeting.
     */
    private GameObject targetEnemy;

    public void SetTarget(GameObject enemy)
    {
        targetEnemy = enemy;
    }

    void FixedUpdate()
    {
        if (!targetEnemy) return ;

        Vector2 direction = (targetEnemy.transform.position - transform.position).normalized;
        
        rigidBody.linearVelocity = bulletSpeed * direction;
        RotateTowardsTarget();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // deal damage to the enemy
        GameObject enemy = collision.gameObject;
        enemy.GetComponent<Health>().TakeDamage(bulletDamage);

        // destroy the bullet
        Destroy(gameObject);
    }

    void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(targetEnemy.transform.position.y - rigidBody.position.y, targetEnemy.transform.position.x - rigidBody.position.x) * Mathf.Rad2Deg - 90f;
        Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
        rigidBody.MoveRotation(rotation);
    }
}
