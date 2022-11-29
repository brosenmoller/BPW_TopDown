using UnityEngine;

public interface IAttackInteractable
{
    public void Interaction(Vector2 direction, int damage, float force);
}
