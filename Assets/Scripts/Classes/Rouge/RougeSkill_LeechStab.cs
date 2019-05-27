using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RougeSkill_LeechStab : BaseAttack
{

    public RougeSkill_LeechStab() : base() { }

    public RougeClass ownerRougeLeech;
    protected override void AttackInit()
    {
        AttackName = "Leech Stab";
        AttackDescription = "Buff steal Mp";
        DamageValue = 0f;
        ManaCost = 10f;
        HaveAdditionEffects = true;
        AoE = false; //random attack false
    }

    public override void AdditionalEffect()
    {
        if (HaveAdditionEffects)
        {
            float randomHpMp = Random.Range(1, 10);
            TurnInfo.Target.MaxHp -= randomHpMp;
            ownerRougeLeech.MaxHp += randomHpMp;//increment maxHp for Rouge class

            TurnInfo.Target.MaxMp -= randomHpMp;
            ownerRougeLeech.MaxMp += randomHpMp;
        }
    }
}
