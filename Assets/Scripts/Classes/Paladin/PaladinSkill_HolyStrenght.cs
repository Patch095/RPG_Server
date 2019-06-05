using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaladinSkill_HolyStrenght : BaseAttack
{
    public PaladinSkill_HolyStrenght() : base() { }

    public PaladinClass owner;

    protected override void AttackInit()
    {
        AttackName = "Holy Strenght";
        AttackDescription = "Attack a single Target skill that boost your damage, but increase it's mana cost";
        ManaCost = 12f;

        AoE = false;
        HaveAdditionEffects = true;
        TargetAllies = false;
    }
    public void SetDamage(float damage)
    {
        DamageValue = damage;
    }

    public override void AdditionalEffect()
    {
        if (HaveAdditionEffects)
        {
            owner.BaseAtk += 3;
            ManaCost += ManaCost / 2;
        }
    }
}
