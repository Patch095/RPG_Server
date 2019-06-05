using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerSkill_HeavyWeather : BaseAttack
{
    public HealerClass owner;

    protected override void AttackInit()
    {
        AoeTarget = new List<BaseClass>();
        AttackName = "Heavy Weather";
        AttackDescription = "Attack multiple Target";
        DamageValue = 20f;
        ManaCost = 10f;
        HaveAdditionEffects = true;
        AoE = true;
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