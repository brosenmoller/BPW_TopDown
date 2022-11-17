
using UnityEngine;

public class DodgeAbility : BasePlayerAbility
{
    [Header("Movement Settings")]
    [SerializeField] float speed;
    [SerializeField] float duration;
    [SerializeField] float cooldown;

    private float cooldownTimer;

    protected override void SetAbilityType() => abilityType = PlayerAbilitys.Move;

    public override void Setup()
    {
        abilityManager.controls.Default.Dodge.started += _ => StartDodge();
    }

    public override void Unset()
    {
        abilityManager.controls.Default.Dodge.started -= _ => StartDodge();
    }

    public void StartDodge()
    {
        if (Time.time < cooldownTimer) { return; }
        cooldownTimer = Time.time + cooldown;

        Debug.Log("Dodge");
    }
}

