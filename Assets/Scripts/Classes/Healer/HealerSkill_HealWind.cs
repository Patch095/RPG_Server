using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerSkill_HealWind : BaseAttack
{
    public HealerClass owner;

    protected override void AttackInit()
    {
        AttackName = "Heal Wind";
        AttackDescription = "A strong single Target heal";
        ManaCost = 15f;
        DamageValue = 50f;

        AoE = false;
        RandomTargets = false;
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