using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Projectile : MonoBehaviour
{
    /**
     * The Rigidbody2D component of the projectile.
     */
    private Rigidbody2D rigidBody;

    /**
     * The speed at which the projectile moves.
     */
    protected float speed;

    /**
     * The damage the projectile deals to the enemy it hits.
     */
    protected int damage;

    /**
     * The position the projectile is currently targeting.
     */
    private Transform target;

    /**
     * The lifetime of the projectile in seconds.
     * The projectile will be destroyed after this time has passed to prevent
     * it from existing indefinitely if it misses all enemies.
     */
    private float lifetime = 5f;

    private float baseSpeed = 5f;

    [SerializeField] private AudioClip shootSound;
    protected AudioSource audioSource;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }
    
    void FixedUpdate()
    {
        if (!target) return ;

        Vector2 direction = (target.transform.position - transform.position).normalized;

        rigidBody.linearVelocity = (speed * baseSpeed) * direction;
        RotateTowardsTarget();
    }

    void Update()
    {
        UpdateLifetime();
    }

    private void UpdateLifetime()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0f)
        {
            // destroy the projectile after its lifetime has expired
            Destroy(gameObject);
        }
    }

    virtual public void Initialize(ProjectileAttributes attributes)
    {
        speed = attributes.Speed;
        damage = attributes.Damage;
    }

    public void SetTarget(Transform position)
    {
        target = position;
    }

    void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(target.transform.position.y - rigidBody.position.y, target.transform.position.x - rigidBody.position.x) * Mathf.Rad2Deg - 90f;
        Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
        rigidBody.MoveRotation(rotation);
    }
}
