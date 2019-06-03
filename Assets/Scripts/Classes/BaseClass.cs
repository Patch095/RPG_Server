using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
abstract public class BaseClass : MonoBehaviour
{
    public string CharacterName;
    public string ClassName;

    public float MaxHp;
    private float currentHp;
    public float CurrentHp { get { return currentHp = Mathf.Clamp(currentHp, 0, MaxHp); } set { ReceiveDamage(value); } }
    private void ReceiveDamage(float damage)
    {
        currentHp -= damage;
        currentHp = Mathf.Clamp(currentHp, 0, MaxHp);
        currentHp = (int)currentHp;
        if (!IsAlive)
            FSM.currentState = CharacterStateMachine.TurnState.DEAD;
    }
    public bool IsAlive { get { return currentHp > 0; } }

    public float MaxMp;
    public float CurrentMp;

    public int BaseAtk;
    public float Speed; //it's a Multiplier value, Clamp betwen 0.1f - 2f

    public string TeamTag; //redTeam - blueTeam

    protected CharacterStateMachine FSM;
    public CharacterStateMachine GetFSM()
    {
        return FSM;
    }

    private uint serverID;
    public uint ServerID { get { return serverID; } }
    public void SetID(uint id)
    {
        serverID = id;
    }

    void OnEnable()
    {
        ClassInit("Default");
        FSM = GetComponent<CharacterStateMachine>();
        FSM.owner = this;
        OnBattleStart();
    }
    private void Start()
    {

    }
    protected abstract void ClassInit(string name);
    protected void OnBattleStart()
    {
        currentHp = MaxHp;
        CurrentMp = MaxMp;

        this.gameObject.tag = TeamTag;
    }

    public List<BaseAttack> ClassSpells;
    public BaseAttack GetSkillFromID(int id)
    {
        return ClassSpells[id];
    }
}
