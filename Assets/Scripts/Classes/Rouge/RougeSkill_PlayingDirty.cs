using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RougeSkill_PlayingDirty : BaseAttack
{

    public RougeSkill_PlayingDirty() : base() { }

    public RougeClass ownerRougePlayingDirty;
    protected override void AttackInit()
    {
        AttackName = "Playing Dirty";
        AttackDescription = "Remove enemy base attack";
        DamageValue = 0f;
        ManaCost = 45f;
        HaveAdditionEffects = true;
        AoE = false; //random attack false
    }

    public override void AdditionalEffect()
    {
        if (HaveAdditionEffects)
        {
            int randomBaseAttack = Random.Range(1, 7);
            TurnInfo.Target.BaseAtk = randomBaseAttack;
            ownerRougePlayingDirty.BaseAtk += randomBaseAttack;
        }
    }
}
