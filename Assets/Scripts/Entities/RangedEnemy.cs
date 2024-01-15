using UnityEngine;

public class RangedEnemy : Enemy, IFiresProjectiles
{
    [field: SerializeField] public Transform ProjectileSpawnPoint { get; set; }
    public Projectile projectile;

    [field: SerializeField] protected override int Health { get; set; } = 100;
    [field: SerializeField] protected override int XpValue { get; set; } = 10;
    [field: SerializeField] protected override float SearchRange { get; set; } = 20f;
    [field: SerializeField] protected override float AttackRate { get; set; } = 3f;
    [field: SerializeField] protected override float AttackRange { get; set; } = 20f;
    [field: SerializeField] protected override int AttackDamage { get; set; } = 10;

    [SerializeField] private AudioSignalSO _sfxAudioSignal;
    [SerializeField] private AudioClip _attackSFX = default;

    protected override void Attack()
    {
        if (Player.IsGhost)
        {
            return;
        }
        Animator.SetTrigger("Attack");
        Debug.Log("Attack");
        _sfxAudioSignal.Trigger(_attackSFX, Player.transform.position, 20f);
        // aim projectilspawnpoint at player so that transform.right is pointing at the player
        ProjectileSpawnPoint.LookAt(Player.GetComponent<Collider>().bounds.center);
        // spawn projectile
        Instantiate(projectile, ProjectileSpawnPoint.position, ProjectileSpawnPoint.rotation);
    }

    protected override void Chase()
    {
        return;
    }
}
