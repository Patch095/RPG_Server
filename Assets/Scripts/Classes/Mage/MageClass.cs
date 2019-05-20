using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageClass : BaseClass
{
    MageSkill_FireBall fireBall;
    protected override void ClassInit(string name)
    {
        if (name == "Default")
            Name = name + "_Mage";
        else
            Name = name;

        ClassName = "Mage";
        MaxHp = 120;
        MaxMp = 250;
        BaseAtk = 5;
        Speed = 2.5f;
    }

    // Start is called before the first frame update
    void Start()
    {
        ClassSpells = new List<BaseAttack>();
        fireBall = this.gameObject.GetComponent<MageSkill_FireBall>();
        if (TeamTag == "BlueTeam")
            fireBall.AoeTarget = FSM.BSM.RedTeamInBattle;
        ClassSpells.Add(fireBall);
    }

    // Update is called once per frame
    void Update()
    {
        if (TeamTag == "BlueTeam")
            fireBall.AoeTarget = FSM.BSM.RedTeamInBattle;
    }
}
