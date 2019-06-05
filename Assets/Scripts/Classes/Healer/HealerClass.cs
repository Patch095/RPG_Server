using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerClass : BaseClass
{
    HealerSkill_HeavyWeather heavyWeather;
    HealerSkill_HealWind healWind;
    HealerSkill_ManaBreze manaBreze;
    HealerSkill_Miracle miracle;

    protected override void ClassInit(string name)
    {
        if (name == "Default")

            CharacterName = name + "_Healer";

        else
            CharacterName = name;

        ClassName = "Healer";
        MaxHp = 160;
        MaxMp = 180;
        BaseAtk = 6;
        Speed = 1.5f;
    }

    // Start is called before the first frame update
    void Start()
    {
        ClassSpells = new List<BaseAttack>();

        heavyWeather = this.gameObject.GetComponent<HealerSkill_HeavyWeather>();
        heavyWeather.owner = this;
        ClassSpells.Add(heavyWeather);

        healWind = this.gameObject.GetComponent<HealerSkill_HealWind>();
        ClassSpells.Add(healWind);

        manaBreze = this.gameObject.GetComponent<HealerSkill_ManaBreze>();
        ClassSpells.Add(manaBreze);

        miracle = this.gameObject.GetComponent<HealerSkill_Miracle>();
        ClassSpells.Add(miracle);
    }

    // Update is called once per frame
    void Update()
    {
        if (TeamTag == "BlueTeam")
        {
            manaBreze.AoeTarget = FSM.BSM.RedTeamInBattle;
            heavyWeather.AoeTarget = FSM.BSM.RedTeamInBattle;
        }
        else if (TeamTag == "RedTeam")
        {
            manaBreze.AoeTarget = FSM.BSM.BlueTeamInBattle;
            heavyWeather.AoeTarget = FSM.BSM.BlueTeamInBattle;
        }
    }
}