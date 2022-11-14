using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityManager : MonoBehaviour
{
    public List<PlayerAbilitys> accesableAbilitys = new();
    private readonly Dictionary<PlayerAbilitys, PlayerAbility> playerAbilitysDictionary = new();

    private void Start()
    {
        SetUpAbilityDictionary();
    }

    private void SetUpAbilityDictionary()
    {
        playerAbilitysDictionary.Add(PlayerAbilitys.Move, GetComponent<MoveAbility>());
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
