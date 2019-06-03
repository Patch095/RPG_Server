using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
//public class Turn : MonoBehaviour
public class Turn
{
    public enum AnimationType { MEELE, RANGED};

    private BaseClass attacker;
    public BaseClass Attacker { get { return attacker; } }
    public void SetAttacker(BaseClass atk)
    {
        attacker = atk;
    }

    private BaseClass target;
    public BaseClass Target { get { return Target; } }
    public void SetTarget(BaseClass targ)
    {
        target = targ;
    }

    private float damageValue;
    public float DamageValue
    {
        get
        {
            if (ChosenAttack == null)
                return damageValue;
            else
                return ChosenAttack.DamageValue;
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
            if (ChosenAttack == null)
                return 0;
            else
                return ChosenAttack.ManaCost;
        }
    }

    public AnimationType actionType; // we will use this for basic animations
    public AnimationType Animation
    {
        get
        {
            if (ChosenAttack == null)
                return actionType;
            else
            {
                if (ChosenAttack.AbilityType == BaseAttack.ActionType.MEELE)
                    actionType = AnimationType.MEELE;
                else if (ChosenAttack.AbilityType == BaseAttack.ActionType.RANGED)
                    actionType = AnimationType.RANGED;
            }
            return actionType;
        }
    }

    private BaseAttack chosenAttack;
    public BaseAttack ChosenAttack { get { return chosenAttack; } }
    public void SetChosenAttack(BaseAttack attack)
    {
        chosenAttack = attack;
    }

    public bool IsAoE
    {
        get
        {
            if (ChosenAttack == null)
                return false;
            else
                return ChosenAttack.AoE;
        }
    }
    public List<BaseClass> AoeTargetSkill
    {
        get
        {
            if(ChosenAttack.AoE)
            {
                return ChosenAttack.AoeTarget;
            }
            else
            {
                return null; 
            }
        }
        
    }
    public bool HaveAdditionEffects
    {
        get
        {
            if (ChosenAttack == null)
                return false;
            else
                return ChosenAttack.HaveAdditionEffects;
        }
    }

    public bool TargetAlly
    {
        get
        {
            if (ChosenAttack == null)
                return false;
            else
                return ChosenAttack.TargetAllies;
        }
    }

    private bool isReady;
    public bool IsReady { get { return isReady; } }
    public void SetReady()
    {
        isReady = true;
    }
}
