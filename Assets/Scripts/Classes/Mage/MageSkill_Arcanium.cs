using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageSkill_Arcanium : BaseAttack
{
    public MageSkill_Arcanium() : base() { }

    public MageClass owner;

    protected override void AttackInit()
    {
        AttackName = "Arcanium";
        AttackDescription = "restore Mana";
        ManaCost = 10f;
        DamageValue = 0f;

        AoE = false;
        RandomTargets = false;
        HaveAdditionEffects = true;
        TargetAllies = true;

        AbilityType = ActionType.MEELE;
    }
    public override void AdditionalEffect()
    {
        if (HaveAdditionEffects)
        {
            owner.MaxMp += 10f;
            owner.CurrentMp += 20f;
            owner.BaseAtk += 3;
            owner.Speed += 0.2f;
        }
    }
    
}
