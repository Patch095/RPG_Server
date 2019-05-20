﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerSkill_HeavyWeather : BaseAttack
{
    public HealerClass owner;

    protected override void AttackInit()
    {
        AttackName = "Heavy Weather";
        AttackDescription = "Your only offensive ability, apply a heal based on damage";
        ManaCost = 15f;
        DamageValue = 5f;

        AoE = true;
<<<<<<< HEAD
        RandomTargets = false;
=======
>>>>>>> 29bcd251cd6ebe137bb3f3e8dea1ce1c0321d20f
        HaveAdditionEffects = true;
        TargetAllies = false;

        AbilityType = ActionType.MEELE;
    }

    public override void AdditionalEffect()
    {
        if (HaveAdditionEffects)
        {
            if (owner.TeamTag == "BlueTeam")
            {
                int randomIndex = Random.Range(0, owner.GetFSM().BSM.BlueTeamInBattle.Count);
                BaseClass randomTarget = owner.GetFSM().BSM.BlueTeamInBattle[randomIndex];
                float heal = DamageValue * owner.GetFSM().BSM.RedTeamInBattle.Count;
                randomTarget.CurrentHp = -heal;
            }
        }
    }
}