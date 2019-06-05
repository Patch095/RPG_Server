using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerSkill_Miracle : BaseAttack
{
    public HealerSkill_Miracle() : base() { }

    protected override void AttackInit()
    {
        AoeTarget = new List<BaseClass>();
        AttackName = "Miracle";
        AttackDescription = "Use all of your MP for fully heal every team member";
        DamageValue = 0f;
        ManaCost = 200f;
        AoE = true;
    }

    public override void AdditionalEffect()
    {
        if (HaveAdditionEffects)
            foreach (BaseClass ally in AoeTarget)
                ally.ResetHP();
    }
}