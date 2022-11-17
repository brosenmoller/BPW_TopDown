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

    public void SetAttackAbility(PlayerAbilitys attackAbility)
    {
        if (attackAbility != activeAttackAbility)
        {
            playerAbilitysDictionary[activeAttackAbility].Unset();
            playerAbilitysDictionary[activeAttackAbility].enabled = false;

            playerAbilitysDictionary[attackAbility].enabled = true;
            playerAbilitysDictionary[attackAbility].Setup();
        }
    }

    public void UnsetActiveAttackAbility()
    {
        playerAbilitysDictionary[activeAttackAbility].Unset();
        playerAbilitysDictionary[activeAttackAbility].enabled = false;
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

    public void RemoveAbility(PlayerAbilitys ability)
    {
        if (accesableAbilitys.Contains(ability))
        {
            accesableAbilitys.Remove(ability);
            playerAbilitysDictionary[ability].Unset();
            playerAbilitysDictionary[ability].enabled = false;
        }
    }
}