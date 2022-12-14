using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerAbilityManager : MonoBehaviour
{
    [Header("Ability Collections")]
    public List<Type> accesableAbilitys = new() { typeof(MoveAbility), typeof(DodgeAbility), typeof(ShootAttackAbility), typeof(MeleeAttackAbility) };
    public BaseAttackAbility activeAttackAbility;

    [Header("Abilty References")]
    public Rigidbody2D rigidBody2D;
    public SpriteRenderer spriteHolder;
    public Animator animator;

    private readonly Dictionary<Type, BasePlayerAbility> playerAbilitysDictionary = new();

    [HideInInspector] public Controls controls;
    [HideInInspector] public bool usingGamepad = false;

    private const string GamepadControlScheme = "Gamepad";

    public void OnDeviceChange(PlayerInput playerInput)
    {
        usingGamepad = playerInput.currentControlScheme.Equals(GamepadControlScheme);
        Cursor.visible = !usingGamepad;
    }

    private void OnEnable()
    {
        controls ??= new Controls();
        controls.Default.Enable();

        controls.Default.SwitchAttack.performed += SwitchAttack;
    }

    private void SwitchAttack(InputAction.CallbackContext context)
    {
        if ()
    }

    private void OnDisable()
    {
        controls.Default.SwitchAttack.performed -= SwitchAttack;
        controls.Default.Disable();
    }

    private void Awake()
    {
        SetUpAbilityDictionary();
    }

    private void SetUpAbilityDictionary()
    {
        foreach (BasePlayerAbility ability in GetComponents<BasePlayerAbility>())
        {
            playerAbilitysDictionary.Add(ability.GetType(), ability);
        }
    }

    public void SetAttackAbility(Type attackAbilityType)
    {
        if (!accesableAbilitys.Contains(attackAbilityType)) { return; }
        if (attackAbilityType == activeAttackAbility.GetType()) { return; }

        UnsetActiveAttackAbility();

        playerAbilitysDictionary[attackAbilityType].enabled = true;
        playerAbilitysDictionary[attackAbilityType].Setup();
    }

    public void UnsetActiveAttackAbility()
    {
        if (activeAttackAbility != null)
        {
            activeAttackAbility.Unset();
            activeAttackAbility.enabled = false;
        }
    }

    public void GiveAbility(Type abilityType)
    {
        if (!accesableAbilitys.Contains(abilityType))
        {
            accesableAbilitys.Add(abilityType);
            playerAbilitysDictionary[abilityType].enabled = true;
            playerAbilitysDictionary[abilityType].Setup();
        }
    }

    public void RemoveAbility(Type abilityType)
    {
        if (accesableAbilitys.Contains(abilityType))
        {
            accesableAbilitys.Remove(abilityType);
            playerAbilitysDictionary[abilityType].Unset();
            playerAbilitysDictionary[abilityType].enabled = false;
        }
    }

    public void DeactivateAllMovementAbilities()
    {
        foreach (Type abilityType in accesableAbilitys)
        {
            BasePlayerAbility ability = playerAbilitysDictionary[abilityType];

            if (ability.isMovementAbility)
            {
                ability.Unset();
                ability.enabled = false;
            }
        }
    }

    public void ReactivateAllMovementAbilities()
    {
        foreach (Type abilityType in accesableAbilitys)
        {
            BasePlayerAbility ability = playerAbilitysDictionary[abilityType];

            if (ability.isMovementAbility)
            {
                ability.Setup();
                ability.enabled = true;
            }
        }
    }
}