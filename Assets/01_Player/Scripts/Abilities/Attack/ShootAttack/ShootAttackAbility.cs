using UnityEngine;

public class ShootAttackAbility : BaseAttackAbility
{
    [Header("Movement Settings")]
    [SerializeField] private float bulletSpeed;
    [SerializeField] private GameObject bullet;

    protected override void PerformAttack()
    {
        GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.identity);
        newBullet.GetComponent<Rigidbody2D>().velocity = attackDirection * bulletSpeed;
    }
}
