using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn : MonoBehaviour
{
    public enum ActionType { MEELE, RANGED};

    public BaseClass Attacker;
    public List<BaseClass> Targets;

    public float damageValue;
    public ActionType actionType; // we will use this for basic animations

    public BaseAttack chosenAttack;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
