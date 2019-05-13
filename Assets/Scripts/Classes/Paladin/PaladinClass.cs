using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaladinClass : BaseClass
{
    PaladinSkill_ShieldSlam shieldSlam;
    PaladinSkill_HolyStrenght holyStrenght;

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
        holyStrenght = this.gameObject.GetComponent<PaladinSkill_HolyStrenght>();
        holyStrenght.owner = this;
        ClassSpells.Add(holyStrenght);
    }

    private void Update()
    {
        //Shield Slam
        float hpScalingValue = CurrentHp / (MaxHp * 2);
        float hpScaling = BaseAtk * hpScalingValue;
        shieldSlam.SetDamage(BaseAtk + hpScaling);

        //Holy Strenght
        holyStrenght.SetDamage(BaseAtk);
    }
}
