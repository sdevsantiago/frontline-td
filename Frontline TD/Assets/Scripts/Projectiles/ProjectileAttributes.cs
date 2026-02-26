public class ProjectileAttributes
{
    public float Speed { get; set; }
    public int Damage { get; set; }
    public float ExplosionRadius { get; set; }

    public ProjectileAttributes(float speed, int damage, float explosionRadius = 0f)
    {
        Speed = speed;
        Damage = damage;
        ExplosionRadius = explosionRadius;
    }
}
