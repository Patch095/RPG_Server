using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 1) Create a RunTime Red/Blue Team UI with a button with reference to every Red/Blue party member
    // 2) Create a ActionUI with:
    //      a) BaseAttack Input
    //      b) SpellMenu Input
    //          c) when selected open a new ClassSpellMenu; every class has his own Spell
    //      d) when a spell is Selected open the Red/Blue UI for character selection;
    //      e) Send all this info to BattleStateMachine.ReceviInput()
    // 3) Make visible and update Team's status (HP, MP, ATB)
    // 4) Manage victory/lose pannels

}
