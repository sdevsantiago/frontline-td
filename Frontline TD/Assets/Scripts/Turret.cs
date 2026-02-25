using UnityEditor;
using UnityEngine;

public class Turret : MonoBehaviour
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
     * The prefab for the bullets that the turret shoots.
     */
    [SerializeField] private GameObject bulletPrefab;

    [Header("Attributes")]
    /**
     * The distance at which the turret can detect and shoot enemies.
     */
    [SerializeField] private float targetingRange = 5f;
    /**
     * The rate at which the turret shoots per second.
     */
    [SerializeField] private float fireRate = 1f;

    /**
     * The enemy the turret is currently targeting.
     */
    private GameObject enemyTarget;
    private float timeSinceLastShot;

    void Update()
    {
        if (enemyTarget == null)
        {
            FindTarget();
            return ;
        }

        if (!TargetIsInRange())
        {
            enemyTarget = null;
        }
        else
        {
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
            enemyTarget = hits[0].transform.gameObject;
        }
    }

    bool TargetIsInRange()
    {
        return Vector2.Distance(rigidBody.position, enemyTarget.transform.position) <= targetingRange;
    }

    private void Shoot()
    {
        // instantiate a bullet and set its target to the current enemy target
        GameObject bullet = Instantiate(bulletPrefab, rigidBody.position, Quaternion.identity);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.SetTarget(enemyTarget);
    }

    void OnDrawGizmosSelected()
    {
        // draw a red wire disc to visualize the turret's range when selected in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, targetingRange);
    }
}
