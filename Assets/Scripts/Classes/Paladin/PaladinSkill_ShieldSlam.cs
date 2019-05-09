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
    }
    public void SetDamage(float damage)
    {
        DamageValue = damage;
    }
}
