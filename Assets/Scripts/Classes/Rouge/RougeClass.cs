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
            Name = name + "_Rouge";
        else
            Name = name;

        ClassName = "Rouge";
        MaxHp = 145;
        MaxMp = 230;
        BaseAtk = 13;
        Speed = 2.2f;
    }

    // Start is called before the first frame update
    void Start()
    {
        ClassSpells = new List<BaseAttack>();

        skill_QuickSlash = this.gameObject.GetComponent<RougeSkill_QuickSlash>();
        skill_QuickSlash.ownerRouge = this;
        ClassSpells.Add(skill_QuickSlash);

        skill_DaggersThrow = this.gameObject.GetComponent<RougeSkill_DaggersThrow>();
        skill_DaggersThrow.ownerRougeDaggersThrow = this;
        ClassSpells.Add(skill_DaggersThrow);


        skill_LeechStab = this.gameObject.GetComponent<RougeSkill_LeechStab>();
        skill_LeechStab.ownerRougeLeech = this;
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
    }


}
