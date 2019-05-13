using System.Collections.Generic;
using UnityEngine;

public class BattleStateMachine : MonoBehaviour
{
    public enum PerformAction
    {
        IDLE,
        PROCESSING_TURN,
        PERFORM_ACTION
    }
    public PerformAction BattleState;

    public List<BaseClass> BlueTeamInBattle;
    public List<BaseClass> RedTeamInBattle;
    public List<BaseClass> DeathCharacters;

    public List<Turn> TurnOrder;

    private Turn selectedAction;

    public List<CharacterStateMachine> CharactersToManage;
    // Start is called before the first frame update
    void Start()
    {
        TurnOrder = new List<Turn>();
        CharactersToManage = new List<CharacterStateMachine>();
        BlueTeamInBattle = new List<BaseClass>();        
        RedTeamInBattle = new List<BaseClass>();
        DeathCharacters = new List<BaseClass>();

        BattleState = PerformAction.IDLE;
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
        if (deadCharacter.TeamTag == "BlueTeam")
            BlueTeamInBattle.Remove(deadCharacter);
        else if (deadCharacter.TeamTag == "RedTeam")
            RedTeamInBattle.Remove(deadCharacter);
        DeathCharacters.Add(deadCharacter);
    }
    public void OnCharacterResurection(BaseClass resurrectedCharacter)
    {
        DeathCharacters.Remove(resurrectedCharacter);
        resurrectedCharacter.tag = resurrectedCharacter.TeamTag;
        if (resurrectedCharacter.TeamTag == "BlueTeam")
            BlueTeamInBattle.Add(resurrectedCharacter);
        else if (resurrectedCharacter.TeamTag == "RedTeam")
            RedTeamInBattle.Remove(resurrectedCharacter);
    }

    // Update is called once per frame
    void Update()
    {
        switch (BattleState)
        {
            case PerformAction.IDLE:
                if (TurnOrder.Count > 0)
                    BattleState = PerformAction.PROCESSING_TURN;
                break;

            case PerformAction.PROCESSING_TURN:
                BaseClass turnPerformer = TurnOrder[0].Attacker;
                CharacterStateMachine FSM = turnPerformer.GetFSM();
                FSM.Target = TurnOrder[0].Target.transform;
                FSM.currentState = CharacterStateMachine.TurnState.ACTION;
                BattleState = PerformAction.PERFORM_ACTION;
                break;

            case PerformAction.PERFORM_ACTION:
                // waiting for player animation and calculate damage
                break;
        }
    }

    public void ReceiveAction(Turn newTurn)
    {
        TurnOrder.Add(newTurn);
    }

    public void DamageCalculation()
    {
        TurnOrder[0].Target.CurrentHp = TurnOrder[0].DamageValue;
    }
    public void ApplyAdditionEffects()
    {
        if(TurnOrder[0].HaveAdditionEffects)
            TurnOrder[0].chosenAttack.AdditionalEffect();
    }
    public void OnTurnEnd()
    {
        TurnOrder.RemoveAt(0);
        BattleState = PerformAction.IDLE;
    }
}
