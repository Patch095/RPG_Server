using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
abstract public class BaseAttack : MonoBehaviour
{
    public enum ActionType { MEELE, RANGED };
    public ActionType AbilityType;

    public string AttackName;
    public string AttackDescription;
    public float ManaCost;
    public float DamageValue;

    public bool AOE;
    public bool RandomTargets;
    public bool HaveAdditionEffects;

    protected abstract void AttackInit();

    public BaseAttack() { }

    private void Start()
    {
        AttackInit();
    }

    public virtual void AdditionalEffect()
    {
        if (HaveAdditionEffects) { }
            //Apply Addition Effect
    }
}