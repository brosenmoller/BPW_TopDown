
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class DodgeAbility : BasePlayerAbility
{
    [Header("Movement Settings")]
    [SerializeField] float speedMultiplier;
    [SerializeField] float duration;
    [SerializeField] float cooldown;

    private bool canDodge = true;

    protected override void SetAbilityType() => abilityType = PlayerAbilitys.Move;

    public override void Setup()
    {
        abilityManager.controls.Default.Dodge.started += StartDodge;
    }

    public override void Unset()
    {
        abilityManager.controls.Default.Dodge.started -= StartDodge;
    }

    public void StartDodge(InputAction.CallbackContext context)
    {
        if (!canDodge || rb.velocity == Vector2.zero) { return; }

        //anim.SetTrigger("Dash");
        //AudioManager.Instance.Play("Dash");
        StartCoroutine(Dodging());
    }

    private IEnumerator Dodging()
    {
        abilityManager.RemoveAbility(PlayerAbilitys.Move);
        canDodge = false;

        Time.timeScale = .9f;

        Vector2 previousVelocity = rb.velocity;
        rb.velocity *= speedMultiplier;

        yield return new WaitForSeconds(duration);

        Time.timeScale = 1f;

        abilityManager.GiveAbility(PlayerAbilitys.Move);
        rb.velocity = previousVelocity;

        yield return new WaitForSeconds(cooldown);

        canDodge = true;
    }
}

