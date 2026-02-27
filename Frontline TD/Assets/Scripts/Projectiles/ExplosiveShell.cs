using UnityEngine;

public class ExplosiveShell : Projectile
{
    /**
     * The radius of the explosion when the shell hits an enemy.
     * Enemies within this radius will take damage from the explosion.
     */
    private float explosionRadius;

    public GameObject explosionPrefab;

    override public void Initialize(ProjectileAttributes attributes)
    {
        base.Initialize(attributes);
        explosionRadius = attributes.ExplosionRadius;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        // get enemies in the explosion radius
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, explosionRadius, LayerMask.GetMask("Enemy"));
        // apply damage to each enemy in the explosion radius
        foreach (Collider2D enemy in enemies)
        {
            enemy.GetComponent<Health>().TakeDamage(damage);
        }

        // destroy the bullet
        Destroy(gameObject);
    }
}
