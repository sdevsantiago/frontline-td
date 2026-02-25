using UnityEditor;
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
    [SerializeField] private Rigidbody2D rigidBody;
    /**
     * The prefab for the projectile that the turret shoots.
     */
    [SerializeField] private Projectile projectile;

    [Header("Attributes")]
    /**
     * The distance at which the turret can detect and shoot enemies.
     */
    [SerializeField] private float targetingRange;
    /**
     * The rate at which the turret shoots per second.
     */
    [SerializeField] private float fireRate;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private int projectileDamage;

    /**
     * The enemy the turret is currently targeting.
     */
    private Transform target;
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

    private void Shoot()
    {
        // instantiate a bullet and set its target to the current enemy target
        GameObject bullet = Instantiate(projectile.prefab, rigidBody.position, Quaternion.identity);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.SetTarget(target.transform);
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
}
