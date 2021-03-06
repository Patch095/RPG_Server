﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageClass : BaseClass
{
    MageSkill_FireBall fireBall;
    MageSkill_Blizzard blizzard;
    MageSkill_Arcanium arcanium;
    MageSkill_ThunderStorm thunderStorm;
    protected override void ClassInit(string name)
    {
        if (name == "Default")
        {
            if (TeamTag == "BlueTeam")
                CharacterName = "Blue_";
            else if (TeamTag == "RedTeam")
                CharacterName = "Red_";
            CharacterName += name + "_Mage";
        }
        else
            CharacterName = name;

        ClassName = "Mage";
        MaxHp = 160;
        MaxMp = 250;
        BaseAtk = 15;
        Speed = 1.5f;
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

        thunderStorm = this.gameObject.GetComponent<MageSkill_ThunderStorm>();
        thunderStorm.mage = this;
        ClassSpells.Add(thunderStorm);
    }

    // Update is called once per frame
    void Update()
    {
        if (TeamTag == "BlueTeam")
        {
            blizzard.AoeTarget = FSM.BSM.RedTeamInBattle;
            thunderStorm.AoeTarget = FSM.BSM.RedTeamInBattle;
        }
        else if (TeamTag == "RedTeam")
        {
            blizzard.AoeTarget = FSM.BSM.BlueTeamInBattle;
            thunderStorm.AoeTarget = FSM.BSM.BlueTeamInBattle;
        }
    }
}
