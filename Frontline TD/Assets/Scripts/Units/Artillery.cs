using UnityEngine;

public class Artillery : Unit
{
    [SerializeField] private float projectileExplosionRadius;

    void Awake()
    {
        projectileAttributes = new ProjectileAttributes(projectileSpeed, projectileDamage, projectileExplosionRadius);
    }
}
