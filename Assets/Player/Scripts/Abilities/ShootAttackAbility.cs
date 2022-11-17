using UnityEngine;

public class ShootAttackAbility : BaseAttackAbility
{
    [Header("Movement Settings")]
    [SerializeField] private float bulletSpeed;
    [SerializeField] private GameObject bullet;

    protected override void SetAbilityType() => abilityType = PlayerAbilitys.ShootAttack;

    protected override void PerformAttack()
    {
        if (attackDirection == null || attackDirection == Vector2.zero) { return; }

        GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.identity);
        newBullet.GetComponent<Rigidbody2D>().velocity = attackDirection * bulletSpeed;
    }
}
