using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerSkill_ManaBreze : BaseAttack
{
    public HealerClass owner;

    protected override void AttackInit()
    {

        AoeTarget = new List<BaseClass>();
        AttackName = "Mana Breze";
        AttackDescription = "AoE mana, heal";
        

        DamageValue = 10f;
        ManaCost = 20f;
        AoE = true;
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