using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerClass : BaseClass
{
    HealerSkill_HeavyWeather heavyWeather;

    protected override void ClassInit(string name)
    {
        if (name == "Default")
            Name = name + "_Cleric";
        else
            Name = name;

        ClassName = "Cleric";
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
    }

    private void Update()
    {

    }
}
