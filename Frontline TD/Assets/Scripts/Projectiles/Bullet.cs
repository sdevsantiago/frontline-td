using UnityEngine;

public class Bullet : Projectile
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        // deal damage to the enemy
        GameObject enemy = collision.gameObject;

        if (enemy.GetComponent<EnemyMovement>().enemyType != EnemyType.Armored)
        {
            // deal damage if enemy is not armored
            enemy.GetComponent<Health>().TakeDamage(damage);
        }

        // destroy the bullet
        Destroy(gameObject);

    }   
}
