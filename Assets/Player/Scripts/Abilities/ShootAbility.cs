using System.Drawing;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShootAbility : PlayerAbility
{
    [Header("Movement Settings")]
    [SerializeField] private float bulletSpeed;
    [SerializeField] private GameObject bullet;

    private Camera mainCamera;
    private Vector2 mouseScreenPosition;
    private Vector2 shootDirection;

    protected override void SetAbilityType() => abilityType = PlayerAbilitys.Shoot;

    public override void Initialize()
    {
        mainCamera = Camera.main;

        abilityManager.controls.Default.Shoot.performed += _ => Shoot();
        abilityManager.controls.Default.Aiming.performed += ctx => SetShootDirection(ctx.ReadValue<Vector2>());
        abilityManager.controls.Default.Movement.started += _ => UpdateShootDirection();
    }

    private void SetShootDirection(Vector2 vector2Input)
    {
        if (abilityManager.usingGamepad)
        {
            if (vector2Input != Vector2.zero) shootDirection = vector2Input;
        }
        else
        {
            mouseScreenPosition = vector2Input;
            Vector2 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);
            shootDirection = (mouseWorldPosition - (Vector2)transform.position).normalized;
        }

        LookTowardsShootDirection();
    }

    private void UpdateShootDirection()
    {
        if (abilityManager.usingGamepad) { return; }

        Vector2 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);
        shootDirection = (mouseWorldPosition - (Vector2)transform.position).normalized;

        LookTowardsShootDirection();
    }

    private void LookTowardsShootDirection()
    {
        float angle = Vector2.SignedAngle(Vector2.right, shootDirection);
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
    }

    private void Shoot()
    {
        if (shootDirection == null || shootDirection == Vector2.zero) { return; }

        GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.identity);
        newBullet.GetComponent<Rigidbody2D>().velocity = shootDirection * bulletSpeed;
    }
}
