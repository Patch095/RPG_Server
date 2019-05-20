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
<<<<<<< HEAD
        RandomTargets = false;
=======
>>>>>>> 29bcd251cd6ebe137bb3f3e8dea1ce1c0321d20f
        HaveAdditionEffects = true;
        TargetAllies = false;

        AbilityType = ActionType.RANGED;
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
