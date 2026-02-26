using UnityEngine;

public class Tank : Unit
{
    [SerializeField] private float projectileExplosionRadius;

    void Awake()
    {
        projectileAttributes = new ProjectileAttributes(projectileSpeed, projectileDamage, projectileExplosionRadius);
    }
}
