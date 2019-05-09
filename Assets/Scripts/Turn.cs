using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Turn : MonoBehaviour
{
    public enum ActionType { MEELE, RANGED};

    public BaseClass Attacker;
    public BaseClass Target;

    private float damageValue;
    public float DamageValue
    {
        get
        {
            if (chosenAttack == null)
                return damageValue;
            else
                return chosenAttack.DamageValue;
        }
        set
        {
            damageValue = value;
        }
    }
    public ActionType actionType; // we will use this for basic animations

    public BaseAttack chosenAttack;

    public bool AoE;
}
