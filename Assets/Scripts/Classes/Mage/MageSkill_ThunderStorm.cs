using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageSkill_ThunderStorm : BaseAttack
{
    public MageClass mage;
    public MageSkill_ThunderStorm() : base() {}

    protected override void AttackInit()
    {
        AoeTarget = new List<BaseClass>();
        AttackName = "Thunder Storm";
        AttackDescription = "Attack multiple Target ,the thunder-storm broke out on the arena, destroying the enemies";
        DamageValue = 40f;
        ManaCost = 50f;
        HaveAdditionEffects = true;
        AoE = true;
    }

    public override void AdditionalEffect()
    {
        if (HaveAdditionEffects)
        {
            mage.Speed -= 0.5f;
            mage.CurrentHp = +5f;
        }
    }
    

}
