using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RougeSkill_DaggersThrow : BaseAttack
{
    public RougeSkill_DaggersThrow() :base(){}

   public RougeClass ownerRougeDaggersThrow;


    protected override void AttackInit()
    {
        AoeTarget = new List<BaseClass>();
        AttackName = "Daggers Throw";
        AttackDescription = "Attack multiple Target";
        DamageValue = 40f;
        ManaCost = 15f;
        HaveAdditionEffects = true;
        AoE = true;
    }

    public override void AdditionalEffect()
    {
        if (HaveAdditionEffects)
        {
            ownerRougeDaggersThrow.CurrentHp = +5f; // + damage life , - add life
        }
    }

}
