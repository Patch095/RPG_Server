using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerSkill_HealWind : BaseAttack
{
    protected override void AttackInit()
    {
        AttackName = "Heal Wind";
        AttackDescription = "A strong single Target heal";
        ManaCost = 50f;
        DamageValue = -50f;

        AoE = false;
        HaveAdditionEffects = false;
        TargetAllies = true;
    }
}