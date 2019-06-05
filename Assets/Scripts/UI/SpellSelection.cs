using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpellSelection : MonoBehaviour
{
    public BaseAttack Spell;
    public UIManager UImanager;
    public Transform SelectedSpellMenu;

    public void Selection()
    {
        UImanager.MagicSelection(Spell);
    }

    public void TogleSpellMenuInfo(bool isActivated)
    {
        TextMeshProUGUI selectedSpellValue = SelectedSpellMenu.GetChild(1).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI selectedSpellDescription = SelectedSpellMenu.GetChild(0).GetComponent<TextMeshProUGUI>();

        selectedSpellDescription.text = Spell.AttackName + " :\n " + Spell.AttackDescription;
        selectedSpellValue.text = "Mana cost : " + (int)Spell.ManaCost;
        if(Spell.DamageValue >= 0)
            selectedSpellValue.text += "\n\nDamage: " + (int)Spell.DamageValue;
        else if (Spell.DamageValue < 0)
            selectedSpellValue.text += "\n\nHeal: " + Mathf.Abs((int)Spell.DamageValue);

        if (Spell.AoE)
            selectedSpellValue.text += "\n\nAoE";
        SelectedSpellMenu.gameObject.SetActive(isActivated);
    }
}