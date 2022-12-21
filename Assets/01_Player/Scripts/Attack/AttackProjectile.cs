using UnityEngine;

public class AttackProjectile : MonoBehaviour
{
    private Rigidbody2D rigidBody2D;

    private Vector2 direction;
    private int damage;
    private float force;

    private void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    public void Setup(float speed, Vector2 direction, int damage, float force)
    {
        this.direction = direction;
        this.damage = damage;
        this.force = force;

        rigidBody2D.velocity = direction * speed;

        float angle = Vector2.SignedAngle(Vector2.right, direction);
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.TryGetComponent(out IAttackInteractable attackInteractable);
        attackInteractable?.OnAttackInteract(direction, damage, force);
    }
}
