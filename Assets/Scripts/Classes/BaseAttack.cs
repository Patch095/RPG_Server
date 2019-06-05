using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
abstract public class BaseAttack : MonoBehaviour
{
    public string AttackName;
    public string AttackDescription;
    public float ManaCost;
    public float DamageValue;

    public bool AoE;
    public List<BaseClass> AoeTarget;     
    public bool HaveAdditionEffects;
    public Turn TurnInfo;
    public bool TargetAllies;

    protected abstract void AttackInit();

    public BaseAttack() { }

    private void Start()
    {
        AttackInit();
    }

    public virtual void AdditionalEffect()
    {
        if (HaveAdditionEffects)
        {
            //Apply Addition Effect
        }
    }
}