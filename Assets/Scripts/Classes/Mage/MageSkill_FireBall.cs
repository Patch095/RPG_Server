using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageSkill_FireBall : BaseAttack
{
    public MageSkill_FireBall():base(){}
    protected override void AttackInit()
    {
        AoeTarget = new List<BaseClass>();
        AttackName = "Fire Ball";
        AttackDescription = "Attack a single Target, damage scales with your current HP";
        DamageValue = 20f;
        ManaCost = 15f;
        AoE = true;
    }
}
