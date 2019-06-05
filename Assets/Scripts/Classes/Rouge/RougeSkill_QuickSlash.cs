using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RougeSkill_QuickSlash : BaseAttack
{
    public RougeSkill_QuickSlash() : base() { }

    public RougeClass owner;
    protected override void AttackInit()
    {
        AttackName = "Quick Slash";
        AttackDescription = "Attack a single Target, increment speed";
        DamageValue = 20f;
        ManaCost = 20f;
        HaveAdditionEffects = true;
        AoE = false;
    }

    public override void AdditionalEffect()
    {
        if (HaveAdditionEffects)
        {
            owner.Speed += 0.5f;
        }
    }
}
