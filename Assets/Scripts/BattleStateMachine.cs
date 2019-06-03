using System.Collections.Generic;
using UnityEngine;

public class BattleStateMachine : MonoBehaviour
{
    UIManager UI;
    private bool uiActivated;

    private GameClient client;
    public void SetClient(GameClient serverClient)
    {
        client = serverClient;
    }

    public enum PerformAction
    {
        IDLE,
        PROCESSING_TURN,
        PERFORM_ACTION
    }
    public PerformAction battleState;
    public PerformAction BattleState
    {
        get { return battleState; }
    }


    private List<BaseClass> blueTeamInBattle;
    private List<BaseClass> redTeamInBattle;
    private List<BaseClass> deathCharacters;
    public List<BaseClass> BlueTeamInBattle { get { return blueTeamInBattle; } }
    public List<BaseClass> RedTeamInBattle { get { return redTeamInBattle; } }
    public List<BaseClass> DeathCharacters { get { return deathCharacters; } }

    public List<Turn> TurnOrder;
    public bool TurnOrderContatinsHero(BaseClass hero)
    {
        foreach(Turn turn in TurnOrder)
        {
            if (turn.Attacker == hero)
                return true;
        }
        return false;
    }

    private Turn selectedAction;

    public List<CharacterStateMachine> CharactersToManage;

    // Start is called before the first frame update
    private void Start()
    {
        //UI = GetComponent<UIManager>();
        UI = GetComponentInChildren<UIManager>();
        uiActivated = false;

        TurnOrder = new List<Turn>();
        CharactersToManage = new List<CharacterStateMachine>();
        blueTeamInBattle = new List<BaseClass>();
        redTeamInBattle = new List<BaseClass>();
        deathCharacters = new List<BaseClass>();

        battleState = PerformAction.IDLE;
    }

    public void ActiveUI()
    {
        UI.SetUIBlueTeam();
        UI.SetUIRedTeam();
    }

    public void AddToTeamList(BaseClass player)
    {
        if (player.CompareTag("BlueTeam"))
            BlueTeamInBattle.Add(player);
        else if (player.CompareTag("RedTeam"))
            RedTeamInBattle.Add(player);

        if (BlueTeamInBattle.Count >= 4 && RedTeamInBattle.Count >= 4)
            client.StartUI();
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
                    battleState = PerformAction.PROCESSING_TURN;

                //if (TurnOrder.Count > 0)
                //    BattleState = PerformAction.PROCESSING_TURN;
                break;

            case PerformAction.PROCESSING_TURN:
                //if (TurnOrder[0].IsReady)
                //{
                    BaseClass turnPerformer = TurnOrder[0].Attacker;
                    CharacterStateMachine FSM = turnPerformer.GetFSM();
                    if (TurnOrder[0].IsAoE || TurnOrder[0].actionType == Turn.AnimationType.RANGED)
                        FSM.Target = TurnOrder[0].Attacker.transform;
                    else
                        FSM.Target = TurnOrder[0].Target.transform;
                    FSM.currentState = CharacterStateMachine.TurnState.ACTION;
                    battleState = PerformAction.PERFORM_ACTION;
                //}
                break;

            case PerformAction.PERFORM_ACTION:
                // waiting for player animation and calculate damage
                break;
        }
        /*
        switch (BattleState)
        {
            case PerformAction.IDLE:
                //if (TurnOrder.Count > 0)
                //    BattleState = PerformAction.PROCESSING_TURN;
                break;

            case PerformAction.PROCESSING_TURN: // per cambiare la visualizzazione della aoe
                BaseClass turnPerformer = TurnOrder[0].Attacker;
                CharacterStateMachine FSM = turnPerformer.GetFSM();
                if (TurnOrder[0].IsAoE)
                    FSM.Target= TurnOrder[0].Attacker.transform;
                else
                    FSM.Target = TurnOrder[0].Target.transform;
                FSM.currentState = CharacterStateMachine.TurnState.ACTION;
                battleState = PerformAction.PERFORM_ACTION;
                BattleState = PerformAction.PERFORM_ACTION;
                break;

            case PerformAction.PERFORM_ACTION:
                // waiting for player animation and calculate damage

                //BaseClass turnPerformer = TurnOrder[0].Attacker;
                //CharacterStateMachine FSM = turnPerformer.GetFSM();
                //FSM.Target = TurnOrder[0].Target.transform;
                //FSM.currentState = CharacterStateMachine.TurnState.ACTION;
                //BattleState = PerformAction.PERFORM_ACTION;

                break;
        }
        */
    }

    public void ProcesessingTurn()
    {
        if (TurnOrder.Count > 0)
            battleState = PerformAction.PROCESSING_TURN;
    }

    public void ReceiveAction(Turn newTurn)
    {
        TurnOrder.Add(newTurn);
    }


    //devo creare un menù con 2 bottoni team rosso e blu , questo bottone si attiva solo se è un attacco aoe ,il bottone con cui puoi interaggire te lo dice la skill , quando il bottone è attivo vai nello state done, quando un'altra azione è in corso non è possibile usare i bottoni target come per l'attacco base 
    public void DamageCalculation()
    {
        if (TurnOrder[0].IsAoE)
        {
            TurnOrder[0].Attacker.GetFSM().Target = TurnOrder[0].Attacker.transform;
            for (int i = 0; i < TurnOrder[0].AoeTargetSkill.Count; i++)
            {
                TurnOrder[0].SetTarget(TurnOrder[0].AoeTargetSkill[i]);//.CurrentHp = TurnOrder[0].DamageValue;
                TurnOrder[0].Target.CurrentHp = TurnOrder[0].DamageValue;
            }
        }
        else
            TurnOrder[0].Target.CurrentHp = TurnOrder[0].DamageValue;
    }
    public void ApplyAdditionEffects()
    {
        if (TurnOrder[0].HaveAdditionEffects)
            TurnOrder[0].ChosenAttack.AdditionalEffect();
    }
    public void OnTurnEnd()
    {
        TurnOrder.RemoveAt(0);
        battleState = PerformAction.IDLE;

        UI.PlayerInput = UIManager.GUIState.ACTIVATED;
    }

    public void SetTurnParameters()
    {
        client.SetTurnParameters();
    }
}
