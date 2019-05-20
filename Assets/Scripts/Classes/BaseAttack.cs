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

    public bool AoE;
<<<<<<< HEAD
    public List<BaseClass> AoeTarget;     
    public bool RandomTargets;
=======
>>>>>>> 29bcd251cd6ebe137bb3f3e8dea1ce1c0321d20f
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
        if (HaveAdditionEffects) { }
            //Apply Addition Effect
    }
}