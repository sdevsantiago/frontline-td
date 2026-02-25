using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    [Header("References")]
    /**
     * The Rigidbody2D component of the projectile.
     */
    [SerializeField] private Rigidbody2D rigidBody;
    /**
     *
     */
    [SerializeField] public GameObject prefab;

    [Header("Attributes")]
    /**
     * The speed at which the projectile moves.
     */
    [SerializeField] protected float speed;
    /**
     * The damage the projectile deals to the enemy it hits.
     */
    [SerializeField] protected int damage;

    /**
     * The position the bullet is currently targeting.
     */
    private Transform target;

    /**
     * The lifetime of the bullet in seconds.
     * The bullet will be destroyed after this time has passed to prevent
     * it from existing indefinitely if it misses all enemies.
     */
    private float lifetime = 5f;

    public void SetTarget(Transform position)
    {
        target = position;
    }

    void FixedUpdate()
    {
        if (!target) return ;

        Vector2 direction = (target.transform.position - transform.position).normalized;

        rigidBody.linearVelocity = speed * direction;
        RotateTowardsTarget();
    }

    void Update()
    {
        // update the bullet's lifetime
        lifetime -= Time.deltaTime;
        if (lifetime <= 0f)
        {
            // destroy the bullet after its lifetime has expired
            Destroy(gameObject);
        }
    }

    void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(target.transform.position.y - rigidBody.position.y, target.transform.position.x - rigidBody.position.x) * Mathf.Rad2Deg - 90f;
        Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
        rigidBody.MoveRotation(rotation);
    }
}
