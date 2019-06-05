using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaladinSkill_SacredSmite : BaseAttack
{
    public PaladinClass owner;

    public PaladinSkill_SacredSmite() : base() { }

    protected override void AttackInit()
    {
        AttackName = "Sacred Smite";
        AttackDescription = "A powerfull attack that reduce target max hp, strong against enemy with high healing";
        ManaCost = 40f;
        DamageValue = 10f;

        AoE = false;

        HaveAdditionEffects = true;
        TargetAllies = false;
    }

    public override void AdditionalEffect()
    {
        if (HaveAdditionEffects)
        {            
            TurnInfo.Target.MaxHp -= 20f;            
            TurnInfo.Target.CurrentHp = 20f;
            owner.Speed -= 0.15f;
        }
    }
}