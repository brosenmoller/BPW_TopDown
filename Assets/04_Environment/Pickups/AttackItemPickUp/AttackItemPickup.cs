using UnityEngine;

public class AttackItemPickup : MonoBehaviour
{
    [SerializeField] private BaseAttackItem attackItem;
    [SerializeField] private AudioObject pickupSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.TryGetComponent(out PlayerAttackManager playerAttackManager);
        if (playerAttackManager != null)
        {
            playerAttackManager.GiveAttackItem(attackItem);
            pickupSound.Play();
            Destroy(gameObject);
        }
    }
}
