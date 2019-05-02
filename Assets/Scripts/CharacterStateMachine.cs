using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CharacterStateMachine : MonoBehaviour
{
    public BattleStateMachine BSM;

    public BaseClass owner;

    public GameObject Selector;
    private bool isSelected;
    public void OnSelection(bool selection)
    {
        isSelected = selection;
    }

    public enum TurnState
    {
        WAITING,
        PROCESSING_TURN,
        CHOOSE_ACTION,
        ACTION,
        DEAD
    }

    public TurnState currentState;

    private float currentCooldown;
    private float maxCooldown = 5f;

    // Start is called before the first frame update
    void Start()
    {
        isSelected = false;

        currentState = TurnState.PROCESSING_TURN;

        BSM.AddToTeamList(owner);
    }

    // Update is called once per frame
    void Update()
    {
        Selector.SetActive(isSelected);

        switch (currentState)
        {
            case TurnState.WAITING:
                //Idle
                break;

            case TurnState.PROCESSING_TURN:
                //fill the turn metter accourding to your speed
                FillTurnMetter();
                break;

            case TurnState.CHOOSE_ACTION:
                ChooseAction();
                currentState = TurnState.WAITING;
                break;

            case TurnState.ACTION:
                //can have animations, will later add a baseMoveAnimation or a ProjectielLaunchAnimation
                Action();
                break;

            case TurnState.DEAD:
                //Disable UI Character Button
                BSM.OnCharacterDeath(owner);
                this.gameObject.tag = "Dead";
                isSelected = false;
                //disable Character Button on GUI
                foreach(Turn turn in BSM.TurnOrder)
                {
                    if (turn.Attacker == owner)
                        BSM.TurnOrder.Remove(turn);
                }
                //set BaseModel.color == Grey
                break;
        }
    }

    void FillTurnMetter()
    {
        currentCooldown += Time.deltaTime * owner.Speed;

        float fillGauge = currentCooldown / maxCooldown;
        //fill UI Turn Bar

        if(currentCooldown >= maxCooldown)
        {
            currentState = TurnState.CHOOSE_ACTION;
        }
    }

    void ChooseAction()
    {
        Turn action = new Turn();
        action.Attacker = owner;

        action.Targets = new List<BaseClass>();

        //for debuggin now action are choose random
        if (owner.CompareTag("BlueTeam")) // redTeam
        {
            if (BSM.RedTeamInBattle.Count > 0)
                action.Targets.Add(BSM.RedTeamInBattle[Random.Range(0, BSM.RedTeamInBattle.Count)]);
        }
        else if (owner.CompareTag("RedTeam")) // blueTeam
        {
            if (BSM.BlueTeamInBattle.Count > 0)
                action.Targets.Add(BSM.BlueTeamInBattle[Random.Range(0, BSM.BlueTeamInBattle.Count)]);
        }

        BSM.ReceiveAction(action);

        Debug.Log(owner.Name + "Action Selected");
    }

    void Action()
    {
        //add base animation, ex: attacker move on the target

        //DAMAGE CALCULATION
        Turn action = BSM.TurnOrder[0];
        owner.CurrentHp -= action.damageValue;
        
        BSM.TurnOrder.RemoveAt(0);
        BSM.BattleState = BattleStateMachine.PerformAction.WAIT;
        currentCooldown = 0f;
        currentState = TurnState.PROCESSING_TURN;
    }
}
