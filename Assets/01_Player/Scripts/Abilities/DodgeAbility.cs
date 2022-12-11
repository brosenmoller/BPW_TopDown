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
        if (!canDodge || rigidBody2D.velocity == Vector2.zero) { return; }

        StartCoroutine(Dodging());
    }

    private IEnumerator Dodging()
    {
        abilityManager.RemoveAbility(typeof(MoveAbility));
        canDodge = false;

        Time.timeScale = .9f;

        Vector2 previousVelocity = rigidBody2D.velocity;
        rigidBody2D.velocity *= speedMultiplier;

        animator.SetBool("IsDashing", true);
        //AudioManager.Instance.Play("Dash");

        yield return new WaitForSeconds(duration);

        Time.timeScale = 1f;

        abilityManager.GiveAbility(typeof(MoveAbility));
        rigidBody2D.velocity = previousVelocity;

        animator.SetBool("IsDashing", false);

        yield return new WaitForSeconds(cooldown);

        canDodge = true;
    }
}

