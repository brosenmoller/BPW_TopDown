using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerAbilitys
{
    Move,
    Dodge,
    ShootAttack,
    SwordAttack,
}

//public class PlayerAbilitys
//{
//    public const int Move = 0;
//    public const int Dodge = 1;
//}

//public class AttackAbilitys : PlayerAbilitys
//{
//    public const int ShootAttack = 100;
//    public const int SwordAttack = 101;
//}

public class PlayerAbilityManager : MonoBehaviour
{
    public List<PlayerAbilitys> accesableAbilitys = new();
    private readonly Dictionary<PlayerAbilitys, BasePlayerAbility> playerAbilitysDictionary = new();
    public PlayerAbilitys activeAttackAbility;

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
    }

    private void OnDisable()
    {
        controls.Default.Disable();
    }

    private void Awake()
    {
        SetUpAbilityDictionary();
    }

    private void SetUpAbilityDictionary()
    {
        playerAbilitysDictionary.Add(PlayerAbilitys.Move, GetComponent<MoveAbility>());
        playerAbilitysDictionary.Add(PlayerAbilitys.Dodge, GetComponent<DodgeAbility>());
        playerAbilitysDictionary.Add(PlayerAbilitys.ShootAttack, GetComponent<ShootAttackAbility>());
        playerAbilitysDictionary.Add(PlayerAbilitys.SwordAttack, GetComponent<SwordAttackAbility>());
    }

    public void ChangeAttackAbility(PlayerAbilitys attackAbility)
    {

    }

    public void GiveAbility(int intAbility)
    {
        if (!Enum.IsDefined(typeof(PlayerAbilitys), intAbility))
        {
            Debug.LogWarning($"Enum PlayerAbilitys doesn't contain definition for, intValue: {intAbility}");
        }

        PlayerAbilitys ability = (PlayerAbilitys)intAbility;
        GiveAbility(ability);
    }
    public void GiveAbility(PlayerAbilitys ability)
    {
        if (!accesableAbilitys.Contains(ability))
        {
            accesableAbilitys.Add(ability);
            playerAbilitysDictionary[ability].enabled = true;
            playerAbilitysDictionary[ability].Setup();
        }
    }

    public void RemoveAbility(int intAbility)
    {
        if (!Enum.IsDefined(typeof(PlayerAbilitys), intAbility))
        {
            Debug.LogWarning($"Enum PlayerAbilitys doesn't contain definition for, intValue: {intAbility}");
        }

        PlayerAbilitys ability = (PlayerAbilitys)intAbility;
        RemoveAbility(ability);
    }

    public void RemoveAbility(PlayerAbilitys ability)
    {
        if (accesableAbilitys.Contains(ability))
        {
            accesableAbilitys.Remove(ability);
            playerAbilitysDictionary[ability].enabled = false;
            playerAbilitysDictionary[ability].Setup();
        }
    }
}