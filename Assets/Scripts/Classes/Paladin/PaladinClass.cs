using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaladinClass : BaseClass
{
    PaladinSkill_ShieldSlam shieldSlam;

    protected override void ClassInit(string name)
    {
        if (name == "Default")
            Name = name + "_Paladin";
        else
            Name = name;

        ClassName = "Paladin";
        MaxHp = 200;
        MaxMp = 120;
        BaseAtk = 18;
        Speed = 0.8f;
    }

    // Start is called before the first frame update
    void Start()
    {
        ClassSpells = new List<BaseAttack>();
        shieldSlam = this.gameObject.GetComponent<PaladinSkill_ShieldSlam>();
        ClassSpells.Add(shieldSlam);
    }

    private void Update()
    {
        //Shield Slam
        float hpScalingValue = CurrentHp / (MaxHp * 2);
        float hpScaling = BaseAtk * hpScalingValue;
        shieldSlam.SetDamage(BaseAtk + hpScaling);
    }
}
