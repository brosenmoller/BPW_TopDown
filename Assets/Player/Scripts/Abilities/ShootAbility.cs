using System.Drawing;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShootAbility : PlayerAbility
{
    [Header("Movement Settings")]
    [SerializeField] private float bulletSpeed;
    [SerializeField] private GameObject bullet;

    private Camera mainCamera;

    private Vector2 shootDirection;
    private Quaternion targetRotation;

    protected override void SetAbilityType() => abilityType = PlayerAbilitys.Shoot;

    public override void Initialize()
    {
        mainCamera = Camera.main;

        abilityManager.controls.Default.Shoot.performed += _ => Shoot();
        abilityManager.controls.Default.Aiming.performed += ctx => SetShootDirection(ctx.ReadValue<Vector2>());
    }

    private void SetShootDirection(Vector2 vector2Input)
    {
        Debug.Log(abilityManager.usingGamepad);
        if (abilityManager.usingGamepad)
        {
            Debug.Log("dsf");
            shootDirection = vector2Input;
        }
        else
        {
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(vector2Input);
            Vector2 direction = (mousePosition - (Vector2)transform.position).normalized;

            shootDirection = direction;
        }

        float angle = Vector2.SignedAngle(transform.position, shootDirection);
        targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
    }

    private void Update()
    {
        if (targetRotation != transform.rotation)
        {
            Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime);
        }
    }

    private void Shoot()
    {
        if (shootDirection == null || shootDirection == Vector2.zero) { return; }

        GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.identity);
        newBullet.GetComponent<Rigidbody2D>().velocity = shootDirection * bulletSpeed;
    }
}
