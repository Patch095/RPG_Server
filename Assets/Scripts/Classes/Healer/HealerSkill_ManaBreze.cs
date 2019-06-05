using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerSkill_ManaBreze : BaseAttack
{
    protected override void AttackInit()
    {
        AoeTarget = new List<BaseClass>();
        AttackName = "Mana Breze";
        AttackDescription = "Summon a magic wind that increase allies MP";
        DamageValue = 0f;
        ManaCost = 0f;
        AoE = true;
    }

    public override void AdditionalEffect()
    {
        if (HaveAdditionEffects)
        {
            foreach (BaseClass ally in AoeTarget)
            {
                ally.CurrentHp += 10;
                ally.CurrentMp += 30;
            }
        }
    }
}