using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaladinClass : BaseClass
{
    public List<BaseAttack> PaladinSpell;

    public PaladinClass() : base()
    {
    }

    protected override void ClassInit(string name)
    {
        if (name == "Default")
            Name = name + "_paladin";
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
