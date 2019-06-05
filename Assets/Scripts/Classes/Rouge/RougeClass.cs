using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RougeClass : BaseClass
{
    RougeSkill_QuickSlash skill_QuickSlash;
    RougeSkill_DaggersThrow skill_DaggersThrow;

    RougeSkill_LeechStab skill_LeechStab;

    RougeSkill_PlayingDirty skill_PlayingDirty;
   
    protected override void ClassInit(string name)
    {
        if (name == "Default")
        {
            if (TeamTag == "BlueTeam")
                CharacterName = "Blue_";
            else if (TeamTag == "RedTeam")
                CharacterName = "Red_";
            CharacterName += name + "_Rouge";
        }
        else
            CharacterName = name;

        ClassName = "Rouge";
        MaxHp = 145;
        MaxMp = 230;
        BaseAtk = 25;
        Speed = 2.2f;
    }

    // Start is called before the first frame update
    void Start()
    {
        ClassSpells = new List<BaseAttack>();

        skill_QuickSlash = this.gameObject.GetComponent<RougeSkill_QuickSlash>();
        skill_QuickSlash.owner = this;
        ClassSpells.Add(skill_QuickSlash);

        skill_DaggersThrow = this.gameObject.GetComponent<RougeSkill_DaggersThrow>();
        skill_DaggersThrow.ownerRougeDaggersThrow = this;
        ClassSpells.Add(skill_DaggersThrow);


        skill_LeechStab = this.gameObject.GetComponent<RougeSkill_LeechStab>();
        skill_LeechStab.owner = this;
        ClassSpells.Add(skill_LeechStab);


        skill_PlayingDirty = this.gameObject.GetComponent<RougeSkill_PlayingDirty>();
        skill_PlayingDirty.ownerRougePlayingDirty = this;
        ClassSpells.Add(skill_PlayingDirty);

    }

    // Update is called once per frame
    void Update()
    {
        if (TeamTag == "BlueTeam")
        {
            skill_DaggersThrow.AoeTarget = FSM.BSM.RedTeamInBattle;
        }
        else if (TeamTag == "BlueTeam")
        {
            skill_DaggersThrow.AoeTarget = FSM.BSM.BlueTeamInBattle;
        }
    }


}
