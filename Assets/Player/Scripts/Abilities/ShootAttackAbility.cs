using UnityEngine;
using UnityEngine.InputSystem;

public class ShootAttackAbility : BaseAttackAbility
{
    [Header("Movement Settings")]
    [SerializeField] private float bulletSpeed;
    [SerializeField] private GameObject bullet;

    protected override void PerformAttack(InputAction.CallbackContext context)
    {
        if (attackDirection == null || attackDirection == Vector2.zero) { return; }

        GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.identity);
        newBullet.GetComponent<Rigidbody2D>().velocity = attackDirection * bulletSpeed;
    }
}
