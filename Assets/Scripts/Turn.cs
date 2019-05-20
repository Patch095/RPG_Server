using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
//public class Turn : MonoBehaviour
public class Turn
{
    public enum AnimationType { MEELE, RANGED};

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

    public float ManaCost
    {
        get
        {
            if (chosenAttack == null)
                return 0;
            else
                return chosenAttack.ManaCost;
        }
    }

    public AnimationType actionType; // we will use this for basic animations
    public AnimationType Animation
    {
        get
        {
            if (chosenAttack == null)
                return actionType;
            else
            {
                if (chosenAttack.AbilityType == BaseAttack.ActionType.MEELE)
                    actionType = AnimationType.MEELE;
                else if (chosenAttack.AbilityType == BaseAttack.ActionType.RANGED)
                    actionType = AnimationType.RANGED;
            }
            return actionType;
        }
    }

    public BaseAttack chosenAttack;

    public bool IsAoE;
    public bool HaveAdditionEffects
    {
        get
        {
            if (chosenAttack == null)
                return false;
            else
                return chosenAttack.HaveAdditionEffects;
        }
    }

    public bool TargetAlly
    {
        get
        {
            if (chosenAttack == null)
                return false;
            else
                return chosenAttack.TargetAllies;
        }
    }
}
