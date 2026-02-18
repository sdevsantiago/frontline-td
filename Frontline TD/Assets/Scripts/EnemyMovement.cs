using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("References")]
    /**
     * The Rigidbody2D component of the enemy.
     */
    [SerializeField] private Rigidbody2D rigidBody;

    [Header("Attributes")]
    /**
     * The speed at which the enemy moves.
     */
    [SerializeField] public float movementSpeed = 2f;

    /**
     * The point the enemy is moving towards.
     */
    private Transform targetPathPoint;

    /**
     * The index of the current path point the enemy is moving towards.
     */
    private int currentPathPointIndex = 0;

    void Start()
    {
        // set the initial target path point to the first point in the path
        targetPathPoint = LevelManager.Instance.pathPoints[0];
    }

    void FixedUpdate()
    {
        // move the enemy towards the target path point
        Vector2 newPosition = Vector2.MoveTowards(rigidBody.position, targetPathPoint.position, movementSpeed * Time.fixedDeltaTime);
        rigidBody.MovePosition(newPosition);

        if (Vector2.Distance(rigidBody.position, targetPathPoint.position) == 0f)
        {
            // if the enemy is close enough to the target path point, move to the next one
            currentPathPointIndex++;
            if (currentPathPointIndex >= LevelManager.Instance.pathPoints.Length)
            {
                // if the enemy has reached the end of the path, destroy it
                Destroy(gameObject);
                return;
            }
            // set the new target path point
            targetPathPoint = LevelManager.Instance.pathPoints[currentPathPointIndex];
        }
    }
}
