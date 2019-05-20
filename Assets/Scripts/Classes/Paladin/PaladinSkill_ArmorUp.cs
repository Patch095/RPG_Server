using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaladinSkill_ArmorUp : BaseAttack
{
    public PaladinSkill_ArmorUp() : base() { }

    protected override void AttackInit()
    {
        AttackName = "Armor Up!";
        AttackDescription = "A single Target buff that increase target max hp";
        ManaCost = 25f;
        DamageValue = 0f;

        AoE = false;
<<<<<<< HEAD
        RandomTargets = false;
=======
>>>>>>> 29bcd251cd6ebe137bb3f3e8dea1ce1c0321d20f
        HaveAdditionEffects = true;
        TargetAllies = true;

        AbilityType = ActionType.MEELE;
    }

    public override void AdditionalEffect()
    {
        if (HaveAdditionEffects)
        {
            TurnInfo.Target.MaxHp += 8f;
            TurnInfo.Target.CurrentHp = -8f;
        }
    }
}
