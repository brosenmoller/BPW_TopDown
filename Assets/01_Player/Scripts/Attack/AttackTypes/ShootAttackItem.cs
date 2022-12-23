using UnityEngine;

[CreateAssetMenu(fileName = "NewShootAttackItem", menuName = "AttackItems/ShootAttack", order = 1)]
public class ShootAttackItem : BaseAttackItem
{
    [Header("Shoot Data")]
    [SerializeField] private float cooldownTime;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject shootParticles;

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
        
        if (!(attackSound.loop && attackSound.IsPlaying)) 
        {
            attackSound.Play();
        }

        GameObject newBullet = Instantiate(bullet, attackManager.weaponAnimator.transform.position, Quaternion.identity);
        newBullet.GetComponent<AttackProjectile>().Setup(bulletSpeed, attackManager.attackDirection, damage, force);

        Instantiate(
            shootParticles, 
            attackManager.weaponAnimator.transform.position + (Vector3)attackManager.attackDirection, 
            attackManager.weaponHolder.rotation
        );

        canShoot = false;
        cooldownTimer.Reset();
    }

    public override void OnAttackEnd()
    {
        if (attackSound.loop) { attackSound.Stop(); }
    }
}
