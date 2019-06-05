using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RougeSkill_LeechStab : BaseAttack
{

    public RougeSkill_LeechStab() : base() { }

    public RougeClass owner;
    protected override void AttackInit()
    {
        AttackName = "Leech Stab";
        AttackDescription = "Buff steal Mp";
        DamageValue = 25f;
        ManaCost = 10f;
        HaveAdditionEffects = true;
        AoE = false; //random attack false
    }

    public override void AdditionalEffect()
    {
        if (HaveAdditionEffects)
        {
            float randomHpMp = Random.Range(10, 25);
            owner.MaxHp += randomHpMp;//increment maxHp for Rouge class

            TurnInfo.Target.MaxMp -= randomHpMp;
            owner.MaxMp += randomHpMp;
        }
    }
}
