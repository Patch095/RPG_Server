﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageSkill_Blizzard : BaseAttack
{
    public MageSkill_Blizzard() : base() { }
    protected override void AttackInit()
    {
        AoeTarget = new List<BaseClass>();
        AttackName = "Blizzard";
        AttackDescription = "Attack multiple Target";
        DamageValue = 20f;
        ManaCost = 25f;
        AoE = true;
    }
}
