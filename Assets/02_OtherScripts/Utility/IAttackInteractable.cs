using UnityEngine;

public interface IAttackInteractable
{
    public void OnAttackInteract(Vector2 direction, int damage, float force);
}
