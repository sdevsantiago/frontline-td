using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    [Header("References")]
    /**
     * The layer mask for detecting enemies.
     * Only objects on this layer will be considered as targets for the turret.
     */
    [SerializeField] private LayerMask enemyLayerMask;
    /**
     * The Rigidbody2D component of the turret.
     */
    [SerializeField] protected Rigidbody2D rigidBody;
    /**
     * The prefab for the projectile that the turret shoots.
     */
    [SerializeField] protected GameObject projectilePrefab;

    [Header("Attributes")]
    /**
     * The distance at which the turret can detect and shoot enemies.
     */
    [SerializeField] protected float targetingRange;
    /**
     * The rate at which the turret shoots per second.
     */
    [SerializeField] protected float fireRate;
    public int cost;
    [SerializeField] protected float projectileSpeed;
    [SerializeField] protected int projectileDamage;
    protected ProjectileAttributes projectileAttributes;

    void Awake()
    {
        projectileAttributes = new ProjectileAttributes(projectileSpeed, projectileDamage);
    }
    
    /**
     * The position the turret is currently targeting.
     */
    protected Transform target;
    private float timeSinceLastShot;

    void Update()
    {
        if (target == null)
        {
            FindTarget();
            return ;
        }

        if (!TargetIsInRange())
        {
            target = null;
        }
        else
        {
            RotateTowardsTarget();

            timeSinceLastShot += Time.deltaTime;
            if (timeSinceLastShot >= 1f / fireRate)
            {
                Shoot();
                timeSinceLastShot = 0f;
            }
        }
    }

    void FindTarget()
    {
        // use raycast in a circle to find all enemies in range
        RaycastHit2D[] hits = Physics2D.CircleCastAll(rigidBody.position, targetingRange, rigidBody.position, 0f, enemyLayerMask);

        if (hits.Length > 0)
        {
            // get the closest enemy among the hits
            target = hits[0].transform;
        }
    }

    bool TargetIsInRange()
    {
        return Vector2.Distance(rigidBody.position, target.transform.position) <= targetingRange;
    }

    protected void Shoot()
    {
        // instantiate a projectile and set its target to the current enemy target
        GameObject projectile = Instantiate(projectilePrefab, rigidBody.position, Quaternion.identity);
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        projectileScript.Initialize(projectileAttributes);
        projectileScript.SetTarget(target);
    }

    void OnDrawGizmosSelected()
    {
        // draw a red wire disc to visualize the turret's range when selected in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, targetingRange);
    }

    void RotateTowardsTarget()
    {
        Vector2 direction = target.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    public float GetTargetingRange()
    {
        return targetingRange;
    }
}
