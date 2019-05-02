using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
abstract public class BaseClass : MonoBehaviour
{
    public string Name;
    public string ClassName;

    public float MaxHp;
    private float currentHp;
    public float CurrentHp { get { return currentHp; } set { ReceiveDamage(value); } }
    public void ReceiveDamage(float damage)
    {
        currentHp -= damage;
        Mathf.Clamp(currentHp, 0, MaxHp);
        if (currentHp == 0)
            FSM.currentState = CharacterStateMachine.TurnState.DEAD;
    }
    public bool IsAlive { get { return currentHp > 0; } }

    public float MaxMp;
    public float CurrentMp;

    public int BaseAtk;
    public float Speed; //it's a Multiplier value, Clamp betwen 0.1f - 2f

    public string TeamTag; //redTeam - blueTeam

    private CharacterStateMachine FSM;

    public List<BaseAttack> basicActions;

    void OnEnable()
    {
        ClassInit("Default");
        FSM = GetComponent<CharacterStateMachine>();
        FSM.owner = this;
        basicActions = new List<BaseAttack>();
        OnBattleStart();
    }

    protected abstract void ClassInit(string name);
    protected void OnBattleStart()
    {
        currentHp = MaxHp;
        CurrentMp = MaxMp;

        this.gameObject.tag = TeamTag;
    }

    protected void BasicAbiltyInit()
    {
        //Basic Attack Command
        BaseAttack basicAttack = new BaseAttack();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
