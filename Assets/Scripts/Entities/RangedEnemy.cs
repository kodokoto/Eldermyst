using UnityEngine;

public class RangedEnemy : Enemy
{
    public Transform projectileSpawnPoint;
    public Projectile projectile;

    [field: SerializeField] protected override int Health { get; set; } = 50;
    [field: SerializeField] protected override int XpValue { get; set; } = 10;
    [field: SerializeField] protected override float SearchRange { get; set; } = 20f;
    [field: SerializeField] protected override float AttackRate { get; set; } = 3f;
    [field: SerializeField] protected override float AttackRange { get; set; } = 20f;
    [field: SerializeField] protected override int AttackDamage { get; set; } = 10;

    protected override void Attack()
    {
        Animator.SetTrigger("Attack");
        Debug.Log("Attack");
        // aim projectilspawnpoint at player so that transform.right is pointing at the player
        projectileSpawnPoint.LookAt(Player.GetComponent<Collider>().bounds.center);
        // spawn projectile
        Instantiate(projectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
    }

    protected override void Chase()
    {
        return;
    }
}
