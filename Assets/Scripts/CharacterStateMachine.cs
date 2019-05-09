using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

[System.Serializable]
public class CharacterStateMachine : MonoBehaviour
{
    public BattleStateMachine BSM;

    public BaseClass owner;

    // this is the target on the character avatar, it will appeare when seleceted
    private GameObject selector;
    private bool isSelected;
    public void OnSelection(bool selection)
    {
        isSelected = selection;
    }

    public enum TurnState
    {
        IDLE,
        PROCESSING_TURN,
        BSM_PROCESSING,
        CHOOSE_ACTION,
        ACTION,
        DEAD
    }
    public TurnState currentState;

    // ATB
    private float currentCooldown;
    private float maxCooldown = 5f;
    public void ModifyTimer(int time = 0)
    {
        currentCooldown -= time;
        if (currentCooldown < 0)
            currentCooldown = 0;
    }
    public void ResetTimer()
    {
        currentCooldown = 0f;
    }
    public Image CharacterPannel;
    public Image ATBbar;

    // Animations
    bool onAnimation;
    Vector3 startPos;
    public Transform Target;
    Vector3 endPos;
    
    // Start is called before the first frame update
    void Start()
    {
        onAnimation = false;
        startPos = this.transform.position;
        currentCooldown = Random.RandomRange(0, maxCooldown / 2f);
        selector = this.transform.GetChild(0).gameObject;
        isSelected = false;

        currentState = TurnState.PROCESSING_TURN;

        BSM.AddToTeamList(owner);
    }

    // Update is called once per frame
    void Update()
    {
        selector.SetActive(isSelected);

        switch (currentState)
        {
            case TurnState.IDLE:
                break;

            case TurnState.PROCESSING_TURN:
                //fill the turn metter accourding to your speed
                FillTurnMetter();
                break;

            case TurnState.BSM_PROCESSING:
                BSM.CharactersToManage.Add(this);
                currentState = TurnState.CHOOSE_ACTION;
                break;

            case TurnState.CHOOSE_ACTION:
                ChooseAction();
                currentState = TurnState.IDLE;
                break;

            case TurnState.ACTION:
                //can have animations, will later add a baseMoveAnimation or a ProjectielLaunchAnimation
                StartCoroutine(Action());
                break;

            case TurnState.DEAD:
                //Disable UI Character Button
                if (!owner.IsAlive)
                {
                    BSM.OnCharacterDeath(owner);
                    this.gameObject.tag = "Dead";
                    isSelected = false;
                    //disable Character Button on GUI
                    foreach (Turn turn in BSM.TurnOrder)
                    {
                        if (turn.Attacker == owner)
                            BSM.TurnOrder.Remove(turn);
                    }
                }
                else if (owner.IsAlive)
                {
                    BSM.OnCharacterResurection(owner);
                    currentState = TurnState.PROCESSING_TURN;
                }
                break;
        }
    }

    void FillTurnMetter()
    {
        currentCooldown += Time.deltaTime * owner.Speed;

        float fillGauge = currentCooldown / maxCooldown;
        CharacterPannel.color = new Color(CharacterPannel.color.r, CharacterPannel.color.g, CharacterPannel.color.b, fillGauge);
        ATBbar.transform.localScale = new Vector3(Mathf.Clamp01(fillGauge), 1f, 1f);

        if(currentCooldown >= maxCooldown)
            currentState = TurnState.BSM_PROCESSING;
    }

    void ChooseAction()
    {
        Turn action = new Turn();
        action.Attacker = owner;

        BSM.ReceiveAction(action);
    }

    private IEnumerator Action()
    {
        while (onAnimation)
            yield break;

        onAnimation = true;
        //Animation
        Vector3 offset = Target.forward * 1.8f;
        endPos = Target.position + offset;
        while (MoveTowardTarget(endPos)) //move toward attack target
            yield return null;

        yield return new WaitForSeconds(0.35f);//Damage Calculation
        BSM.DamageCalculation();

        while (MoveTowardTarget(startPos)) //return to your startPosition
            yield return null;

        //animation is finished
        BSM.OnTurnEnd();
        BSM.BattleState = BattleStateMachine.PerformAction.IDLE;

        onAnimation = false;

        //reset character for a new turn
        currentCooldown = 0f;
        currentState = TurnState.PROCESSING_TURN;
    }

    bool MoveTowardTarget(Vector3 targetPosition)
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, targetPosition, Time.deltaTime * 3f);
        return targetPosition != this.transform.position;
    }
}