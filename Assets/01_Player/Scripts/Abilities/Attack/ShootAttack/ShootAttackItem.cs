using UnityEngine;

[CreateAssetMenu(fileName = "NewShootAttackItem", menuName = "AttackItems")]
public class ShootAttackItem : BaseAttackItem
{
    [Header("Shoot Settings")]
    public float bulletSpeed;
    public GameObject bullet;
}
