using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellSelection : MonoBehaviour
{
    public BaseAttack Spell;
    public UIManager UImanager;

    public void Selection()
    {
        UImanager.MagicSelection(Spell);
    }
}
