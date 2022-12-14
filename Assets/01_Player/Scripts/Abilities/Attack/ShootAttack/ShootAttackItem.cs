using UnityEngine;

[CreateAssetMenu(fileName = "NewShootAttackItem", menuName = "AttackItems")]
public class ShootAttackItem
{
    [Header("Shoot Settings")]
    public float bulletSpeed;
    public GameObject bullet;
}
