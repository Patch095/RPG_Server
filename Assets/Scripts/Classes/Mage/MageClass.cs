using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageClass : BaseClass
{
    MageSkill_FireBall fireBall;
    MageSkill_Blizzard blizzard;
    MageSkill_Arcanium arcanium;
    MageSkill_ThunderStorm thunder_Storm;
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
        ClassSpells.Add(fireBall);

        blizzard = this.gameObject.GetComponent<MageSkill_Blizzard>();
        ClassSpells.Add(blizzard);


        arcanium = this.gameObject.GetComponent<MageSkill_Arcanium>();
        arcanium.owner = this;
        ClassSpells.Add(arcanium);

        thunder_Storm = this.gameObject.GetComponent<MageSkill_ThunderStorm>();
        thunder_Storm.mage = this;
        ClassSpells.Add(thunder_Storm);

    }

    // Update is called once per frame
    void Update()
    {
        if (TeamTag == "BlueTeam")
        {
            blizzard.AoeTarget = FSM.BSM.RedTeamInBattle;
            thunder_Storm.AoeTarget = FSM.BSM.RedTeamInBattle;
        }
    }
}
