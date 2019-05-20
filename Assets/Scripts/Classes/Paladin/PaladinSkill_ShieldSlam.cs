using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaladinSkill_ShieldSlam : BaseAttack
{
    public PaladinSkill_ShieldSlam() : base() { }

    protected override void AttackInit()
    {
        AttackName = "Shield Slam";
        AttackDescription = "Attack a single Target, damage scales with your current HP";
        ManaCost = 8f;

        AoE = false;
<<<<<<< HEAD
        RandomTargets = false;
=======
>>>>>>> 29bcd251cd6ebe137bb3f3e8dea1ce1c0321d20f
        HaveAdditionEffects = false;
        TargetAllies = false;

        AbilityType = ActionType.MEELE;
    }
    public void SetDamage(float damage)
    {
        DamageValue = damage;
    }
}
