using UnityEngine;

[CreateAssetMenu(fileName = "NewShootAttackItem", menuName = "AttackItems/ShootAttack", order = 1)]
public class ShootAttackItem : BaseAttackItem
{
    [Header("Shoot Settings")]
    [SerializeField] private float cooldownTime;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private GameObject bullet;

    private Timer cooldownTimer;
    private bool canShoot;

    private SpriteRenderer weaponSprite;

    protected override void SetupSpecificItem()
    {
        cooldownTimer = new Timer(cooldownTime, () => { canShoot = true; });
        weaponSprite = attackManager.weaponAnimator.gameObject.GetComponent<SpriteRenderer>();
    }

    public override void OnUpdate()
    {
        float angle = attackManager.weaponHolder.rotation.eulerAngles.z;

        if (90f <= angle && angle <= 270f) { weaponSprite.flipX = true; }
        else { weaponSprite.flipX = false; }
    }

    public override void PerformAttack()
    {
        if (!canShoot) { return; }

        attackManager.weaponAnimator.SetTrigger("AttackTrigger");

        GameObject newBullet = Instantiate(bullet, attackManager.weaponAnimator.transform.position, Quaternion.identity);
        newBullet.GetComponent<AttackProjectile>().Setup(bulletSpeed, attackManager.attackDirection, damage, force);

        canShoot = false;
        cooldownTimer.Reset();
    }
}
