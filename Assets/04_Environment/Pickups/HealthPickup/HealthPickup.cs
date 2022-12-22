using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] private int healthIncrease;
    [SerializeField] private AudioObject pickupSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.TryGetComponent(out PlayerHealthManager player);
        if (player != null)
        {
            player.AddHealth(healthIncrease);
            pickupSound.Play();
            Destroy(gameObject);
        }
    }
}
