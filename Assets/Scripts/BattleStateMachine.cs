using System.Collections.Generic;
using UnityEngine;

public class BattleStateMachine : MonoBehaviour
{
    public enum PerformAction
    {
        WAIT,
        TAKE_ACTION,
        PERFORM_ACTION
    }
    public PerformAction BattleState;

    public List<BaseClass> BlueTeamInBattle;
    public List<BaseClass> RedTeamInBattle;
    public List<BaseClass> DeathCharacters;

    public List<Turn> TurnOrder;

    private Turn selectedAction;

    // Start is called before the first frame update
    void Start()
    {
        TurnOrder = new List<Turn>();

        BlueTeamInBattle = new List<BaseClass>();        
        RedTeamInBattle = new List<BaseClass>();
        DeathCharacters = new List<BaseClass>();

        BattleState = PerformAction.WAIT;
    }

    public void AddToTeamList(BaseClass player)
    {
        if (player.CompareTag("BlueTeam"))
            BlueTeamInBattle.Add(player);
        else if (player.CompareTag("RedTeam"))
            RedTeamInBattle.Add(player);
    }
    public void OnCharacterDeath(BaseClass deadCharacter)
    {
        if (deadCharacter.GetComponent("BlueTeam"))
            BlueTeamInBattle.Remove(deadCharacter);
        else if (deadCharacter.GetComponent("RedTeam"))
            RedTeamInBattle.Remove(deadCharacter);
        DeathCharacters.Add(deadCharacter);
    }

    // Update is called once per frame
    void Update()
    {
        switch (BattleState)
        {
            case PerformAction.WAIT:
                //Idle
                if (TurnOrder.Count > 0)
                {
                    BattleState = PerformAction.TAKE_ACTION;
                }
                break;

            case PerformAction.TAKE_ACTION:
                BaseClass turnPerformer = TurnOrder[0].Attacker;
                if (TurnOrder[0].Targets.Count > 1) //aoe attack
                {
                    foreach (BaseClass target in TurnOrder[0].Targets)
                    {
                        CharacterStateMachine TargetSM = target.GetComponent<CharacterStateMachine>();
                        TargetSM.currentState = CharacterStateMachine.TurnState.ACTION;
                    }
                }
                else //signle attack
                {
                    BaseClass target = TurnOrder[0].Targets[0];
                    if(!target.IsAlive) //target is dead, find new target
                    {
                        BaseClass newTarget = target;
                        if (target.TeamTag == "BlueTeam")
                            newTarget = BlueTeamInBattle[Random.Range(0, BlueTeamInBattle.Count)];
                        else if (target.TeamTag == "RedTeam")
                            newTarget = RedTeamInBattle[Random.Range(0, RedTeamInBattle.Count)];
                        TurnOrder[0].Targets[0] = newTarget;
                        CharacterStateMachine TargetSM = newTarget.GetComponent<CharacterStateMachine>();
                        TargetSM.currentState = CharacterStateMachine.TurnState.ACTION;
                    }
                }
                break;

            case PerformAction.PERFORM_ACTION:
                break;
        }
    }

    public void ReceiveAction(Turn newTurn)
    {
        TurnOrder.Add(newTurn);
    }

    public void ReceiveInput()
    {
        //he receive from UIManager 
    }
}
