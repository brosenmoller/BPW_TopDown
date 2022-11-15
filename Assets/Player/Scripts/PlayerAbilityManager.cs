using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAbilityManager : MonoBehaviour
{
    public List<PlayerAbilitys> accesableAbilitys = new();
    private readonly Dictionary<PlayerAbilitys, PlayerAbility> playerAbilitysDictionary = new();

    [HideInInspector] public Controls controls;
    [HideInInspector] public bool usingGamepad = false;

    public void OnDeviceChange(PlayerInput playerInput)
    {
        usingGamepad = playerInput.currentControlScheme.Equals("Gamepad");
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
        playerAbilitysDictionary.Add(PlayerAbilitys.Shoot, GetComponent<ShootAbility>());
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
            playerAbilitysDictionary[ability].Initialize();
        }
    }
}
