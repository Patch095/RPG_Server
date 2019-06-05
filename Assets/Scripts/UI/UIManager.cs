using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    BattleStateMachine BSM;

    private Turn playerChoise;

    public enum GUIState
    {
        ACTIVATED,
        PREPARATION,
        IDLE,
        ATTACK,
        MAGIC,
        TARGET_SELECTION,
        PLAYER_INPUT_DONE,
        DONE,
        TURN_END,
        CANCEL
    }
    public GUIState PlayerInput;

    //Blue Team UI
    public Transform BlueTeamCharactersMenu;
    Dictionary<TargetSelectButton, Button> BlueTeamTarges;
    public Transform BlueTeamAllyTargetsMenu;
    public Transform BlueTeamEnemyTargetsMenu;
    public Transform BlueTeamAoETargetsMenu;
    public GameObject BlueTeamCommandMenu;
    public Transform BlueTeamSpellsMenu;
    public Transform BlueTeamSelectedSpellInfo;
    public GameObject BlueTeamCancelButton;
    public GameObject BluePlayerWaitingPanel;

    //Red Team UI
    public Transform RedTeamCharactersMenu;
    Dictionary<TargetSelectButton, Button> RedTeamTarges;
    public Transform RedTeamAllyTargetsMenu;
    public Transform RedTeamEnemyTargetsMenu;
    public Transform RedTeamAoETargetsMenu;
    public GameObject RedTeamCommandMenu;
    public Transform RedTeamSpellsMenu;
    public Transform RedTeamSelectedSpellInfo;
    public GameObject RedTeamCancelButton;
    public GameObject RedPlayerWaitingPanel;

    public bool UIStarted;
    private bool sendInput;
    private void OnEnable()
    {
        UIStarted = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        BSM = GetComponent<BattleStateMachine>();

        sendInput = false;

        //PlayerInput = GUIState.ACTIVATED;

        //Blue team
        BlueTeamCharactersMenu.gameObject.SetActive(false);
        BlueTeamCommandMenu.SetActive(false);
        BlueTeamAllyTargetsMenu.gameObject.SetActive(false);
        BlueTeamEnemyTargetsMenu.gameObject.SetActive(false);
        BlueTeamAoETargetsMenu.gameObject.SetActive(false);
        BlueTeamSpellsMenu.gameObject.SetActive(false);
        BlueTeamSelectedSpellInfo.gameObject.SetActive(false);
        BluePlayerWaitingPanel.gameObject.SetActive(false);//in attesa del player rosso che fa la mossa 

        //Red team
        RedTeamCharactersMenu.gameObject.SetActive(false);
        RedTeamCommandMenu.SetActive(false);
        RedTeamAllyTargetsMenu.gameObject.SetActive(false);
        RedTeamEnemyTargetsMenu.gameObject.SetActive(false);
        RedTeamAoETargetsMenu.gameObject.SetActive(false);
        RedTeamSelectedSpellInfo.gameObject.SetActive(false);
        RedTeamSpellsMenu.gameObject.SetActive(false);
        RedPlayerWaitingPanel.gameObject.SetActive(false); // in attesa del player blu che fa la mossa
    }

    public void SetUI()
    {
        if (!UIStarted)
        {
            SetUIBlueTeam();
            SetUIRedTeam();

            PlayerInput = GUIState.ACTIVATED;
            UIStarted = true;
        }
    }
    private void SetUIBlueTeam()
    {
        //Blue Team UI
        //Characters
        foreach (Transform child in BlueTeamCharactersMenu)
            child.gameObject.SetActive(false);
        for (int i = 0; i < BSM.BlueTeamInBattle.Count; i++)
        {
            GameObject characterPannelInfo = BlueTeamCharactersMenu.GetChild(i).gameObject;
            characterPannelInfo.AddComponent<CharacterUISettings>();
            characterPannelInfo.GetComponent<CharacterUISettings>().Owner = BSM.BlueTeamInBattle[i];
            characterPannelInfo.SetActive(true);
        }
        //Targets Enemy
        BlueTeamTarges = new Dictionary<TargetSelectButton, Button>();
        foreach (Transform child in BlueTeamEnemyTargetsMenu)
            child.gameObject.SetActive(false);
        for (int i = 0; i < BSM.RedTeamInBattle.Count; i++)
        {
            GameObject button = BlueTeamEnemyTargetsMenu.GetChild(i).gameObject;
            button.SetActive(true);
            Text text = button.GetComponentInChildren<Text>();
            text.text = BSM.RedTeamInBattle[i].CharacterName;
            TargetSelectButton targerButton = button.GetComponent<TargetSelectButton>();
            targerButton.SetUIManager(this);
            targerButton.SetTarget(BSM.RedTeamInBattle[i]);
            BlueTeamTarges.Add(targerButton, button.GetComponent<Button>());
        }
        //Targets Allies
        foreach (Transform child in BlueTeamAllyTargetsMenu)
            child.gameObject.SetActive(false);
        for (int i = 0; i < BSM.BlueTeamInBattle.Count; i++)
        {
            GameObject button = BlueTeamAllyTargetsMenu.GetChild(i).gameObject;
            button.SetActive(true);
            Text text = button.GetComponentInChildren<Text>();
            text.text = BSM.BlueTeamInBattle[i].CharacterName;
            TargetSelectButton targerButton = button.GetComponent<TargetSelectButton>();
            targerButton.SetUIManager(this);
            targerButton.SetTarget(BSM.BlueTeamInBattle[i]);
            BlueTeamTarges.Add(targerButton, button.GetComponent<Button>());
        }
        //Targets Team
        //blueTeam
        GameObject blueTeamButton = BlueTeamAoETargetsMenu.GetChild(0).gameObject;
        TargetTeam targetBlue = blueTeamButton.GetComponent<TargetTeam>();
        targetBlue.SetUIManager(this);
        targetBlue.SetTargets(BSM, "BlueTeam");
        //redTeam
        GameObject redTeamButton = BlueTeamAoETargetsMenu.GetChild(1).gameObject;
        TargetTeam targetRed = blueTeamButton.GetComponent<TargetTeam>();
        targetRed.SetUIManager(this);
        targetRed.SetTargets(BSM, "RedTeam");

        BlueTeamCharactersMenu.gameObject.SetActive(true);
    }
    private void SetUIRedTeam()
    {
        //Red Team UI
        //Characters      
        foreach (Transform child in RedTeamCharactersMenu)
            child.gameObject.SetActive(false);
        for (int i = 0; i < BSM.RedTeamInBattle.Count; i++)
        {
            GameObject characterPannelInfo = RedTeamCharactersMenu.GetChild(i).gameObject;
            characterPannelInfo.AddComponent<CharacterUISettings>();
            characterPannelInfo.GetComponent<CharacterUISettings>().Owner = BSM.RedTeamInBattle[i];
            characterPannelInfo.SetActive(true);
        }
        //Targets Enemy
        RedTeamTarges = new Dictionary<TargetSelectButton, Button>();
        foreach (Transform child in RedTeamEnemyTargetsMenu)
            child.gameObject.SetActive(false);
        for (int i = 0; i < BSM.BlueTeamInBattle.Count; i++)
        {
            GameObject button = RedTeamEnemyTargetsMenu.GetChild(i).gameObject;
            button.SetActive(true);
            Text text = button.GetComponentInChildren<Text>();
            text.text = BSM.BlueTeamInBattle[i].CharacterName;
            TargetSelectButton targerButton = button.GetComponent<TargetSelectButton>();
            targerButton.SetUIManager(this);
            targerButton.SetTarget(BSM.BlueTeamInBattle[i]);
            RedTeamTarges.Add(targerButton, button.GetComponent<Button>());
        }
        //Targets Allies
        foreach (Transform child in RedTeamAllyTargetsMenu)
            child.gameObject.SetActive(false);
        for (int i = 0; i < BSM.RedTeamInBattle.Count; i++)
        {
            GameObject button = RedTeamAllyTargetsMenu.GetChild(i).gameObject;
            button.SetActive(true);
            Text text = button.GetComponentInChildren<Text>();
            text.text = BSM.RedTeamInBattle[i].CharacterName;
            TargetSelectButton targerButton = button.GetComponent<TargetSelectButton>();
            targerButton.SetUIManager(this);
            targerButton.SetTarget(BSM.RedTeamInBattle[i]);
            RedTeamTarges.Add(targerButton, button.GetComponent<Button>());
        }

        //Targets Team
        //blueTeam
        GameObject blueTeamButton = RedTeamAoETargetsMenu.GetChild(0).gameObject;
        TargetTeam targetBlue = blueTeamButton.GetComponent<TargetTeam>();
        targetBlue.SetUIManager(this);
        targetBlue.SetTargets(BSM, "BlueTeam");
        //redTeam
        GameObject redTeamButton = RedTeamAoETargetsMenu.GetChild(1).gameObject;
        TargetTeam targetRed = blueTeamButton.GetComponent<TargetTeam>();
        targetRed.SetUIManager(this);
        targetRed.SetTargets(BSM, "RedTeam");

        RedTeamCharactersMenu.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (UIStarted)
        {
            switch (PlayerInput)
            {
                case GUIState.ACTIVATED:
                    if (BSM.CharactersToManage.Count > 0)
                        PlayerInput = GUIState.PREPARATION;
                    break;

                case GUIState.PREPARATION:
                    BSM.CharactersToManage[0].OnSelection(true);
                    playerChoise = BSM.TurnOrder[0];
                    Debug.Log(playerChoise.Attacker);
                    if (playerChoise.Attacker.TeamTag == "BlueTeam")
                        BlueTeamCommandMenu.SetActive(true);
                    else if (playerChoise.Attacker.TeamTag == "RedTeam")
                        RedTeamCommandMenu.SetActive(true);
                    PlayerInput = GUIState.IDLE;
                    break;

                case GUIState.IDLE:
                    break;

                case GUIState.ATTACK:
                    //Basic attack command
                    AttackInput();
                    break;

                case GUIState.MAGIC:
                    //Magic command
                    MagicSubMenuInit();
                    break;

                case GUIState.TARGET_SELECTION:
                    if (!sendInput)
                    {
                        SendTurnParameters();
                    }
                    break;

                case GUIState.PLAYER_INPUT_DONE:
                    break;

                case GUIState.CANCEL:
                    CancelInputBlueTeam();
                    CancelInputRedTeam();
                    break;

                case GUIState.DONE:
                    PlayerInputDone();
                    break;
            }

        foreach (KeyValuePair<TargetSelectButton, Button> keyValue in BlueTeamTarges)
        {
            TargetSelectButton key = keyValue.Key;
            Button value = keyValue.Value;
            if (BSM.BattleState == BattleStateMachine.PerformAction.PERFORM_ACTION)
                value.interactable = false;
            else
                value.interactable = key.Target.IsAlive;

            Text buttonText = value.GetComponentInChildren<Text>();
            buttonText.text = key.Target.CharacterName + "\n" + (int)key.Target.CurrentHp + "/" + key.Target.MaxHp;
        }

        foreach (KeyValuePair<TargetSelectButton, Button> keyValue in RedTeamTarges)
        {
            TargetSelectButton key = keyValue.Key;
            Button value = keyValue.Value;
            if (BSM.BattleState == BattleStateMachine.PerformAction.PERFORM_ACTION)
                value.interactable = false;
            else
                value.interactable = key.Target.IsAlive;

            Text buttonText = value.GetComponentInChildren<Text>();
           
            buttonText.text = key.Target.CharacterName + "\n" + (int) key.Target.CurrentHp + "/" + key.Target.MaxHp;
            }
        }

        BlueTeamCancelButton.SetActive(BlueTeamAllyTargetsMenu.gameObject.activeInHierarchy || BlueTeamEnemyTargetsMenu.gameObject.activeInHierarchy || BlueTeamAoETargetsMenu.gameObject.activeInHierarchy);
        RedTeamCancelButton.SetActive(RedTeamAllyTargetsMenu.gameObject.activeInHierarchy || RedTeamEnemyTargetsMenu.gameObject.activeInHierarchy || RedTeamAoETargetsMenu.gameObject.activeInHierarchy);

        if (playerChoise == null )
        {
            BluePlayerWaitingPanel.gameObject.SetActive(false);
            RedPlayerWaitingPanel.gameObject.SetActive(false);
        }
        else if (playerChoise != null && playerChoise.Attacker != null)
        {
            if (playerChoise.Attacker.tag == "BlueTeam")
            {
                BluePlayerWaitingPanel.gameObject.SetActive(false);
                RedPlayerWaitingPanel.gameObject.SetActive(true);
            }
            else if (playerChoise.Attacker.tag == "RedTeam")
            {
                RedPlayerWaitingPanel.gameObject.SetActive(false);
                BluePlayerWaitingPanel.gameObject.SetActive(true);
            }
        }
    }

    public void AttackInput()
    {
        BlueTeamSpellsMenu.gameObject.SetActive(false);
        RedTeamSpellsMenu.gameObject.SetActive(false);

        playerChoise.DamageValue = (float)BSM.CharactersToManage[0].owner.BaseAtk;

        if (BSM.CharactersToManage[0].tag == "BlueTeam")
            BlueTeamEnemyTargetsMenu.gameObject.SetActive(true);
        else if (BSM.CharactersToManage[0].tag == "RedTeam")
            RedTeamEnemyTargetsMenu.gameObject.SetActive(true);
    }

    public void CancelInputBlueTeam()
    {
        PlayerInput = GUIState.ACTIVATED;
        BlueTeamCommandMenu.SetActive(true);
        BlueTeamAllyTargetsMenu.gameObject.SetActive(false);
        BlueTeamEnemyTargetsMenu.gameObject.SetActive(false);
        BlueTeamAoETargetsMenu.gameObject.SetActive(false);
        BlueTeamSpellsMenu.gameObject.SetActive(false);
        BlueTeamSelectedSpellInfo.gameObject.SetActive(false);
        playerChoise.SetChosenAttack(null);
    }
    public void CancelInputRedTeam()
    {
        PlayerInput = GUIState.ACTIVATED;
        RedTeamCommandMenu.SetActive(true);
        RedTeamAllyTargetsMenu.gameObject.SetActive(false);
        RedTeamEnemyTargetsMenu.gameObject.SetActive(false);
        RedTeamAoETargetsMenu.gameObject.SetActive(false);
        RedTeamSpellsMenu.gameObject.SetActive(false);
        RedTeamSelectedSpellInfo.gameObject.SetActive(false);
        playerChoise.SetChosenAttack(null);
    }

    public void MagicSubMenuInit()
    {
        BaseClass attackerClass = BSM.CharactersToManage[0].owner;
        //Spells submenu creation
        foreach (Transform child in BlueTeamSpellsMenu)
            child.gameObject.SetActive(false);
        for (int i = 0; i < attackerClass.ClassSpells.Count; i++)
        {
            BaseAttack currentSkill = attackerClass.ClassSpells[i];
            GameObject skillButton = BlueTeamSpellsMenu.GetChild(i).gameObject;
            Button button = skillButton.GetComponent<Button>();
            Text text = skillButton.GetComponentInChildren<Text>();
            text.text = currentSkill.AttackName;
            button.interactable = attackerClass.CurrentMp >= currentSkill.ManaCost;
            skillButton.GetComponent<SpellSelection>().UImanager = this;
            skillButton.GetComponent<SpellSelection>().Spell = currentSkill;
            skillButton.GetComponent<SpellSelection>().SelectedSpellMenu = BlueTeamSelectedSpellInfo;

            skillButton.SetActive(true);
        }
        BlueTeamSpellsMenu.gameObject.SetActive(true);

        //Red team
        BaseClass attackerClassRed = BSM.CharactersToManage[0].owner;
        //Spells submenu creation
        foreach (Transform child in RedTeamSpellsMenu)
            child.gameObject.SetActive(false);
        for (int i = 0; i < attackerClass.ClassSpells.Count; i++)
        {
            BaseAttack currentSkill = attackerClassRed.ClassSpells[i];
            GameObject skillButton = RedTeamSpellsMenu.GetChild(i).gameObject;
            Button button = skillButton.GetComponent<Button>();
            Text text = skillButton.GetComponentInChildren<Text>();
            text.text = currentSkill.AttackName;
            button.interactable = attackerClass.CurrentMp >= currentSkill.ManaCost;
            skillButton.GetComponent<SpellSelection>().UImanager = this;
            skillButton.GetComponent<SpellSelection>().Spell = currentSkill;
            skillButton.GetComponent<SpellSelection>().SelectedSpellMenu = RedTeamSelectedSpellInfo;

            skillButton.SetActive(true);
        }
        RedTeamSpellsMenu.gameObject.SetActive(true);
    }

    public void MagicSelection(BaseAttack Spell)
    {
        Spell.TurnInfo = playerChoise;
        playerChoise.SetChosenAttack(Spell);
        //check team and target
        if (BSM.CharactersToManage[0].tag == "BlueTeam")
        {
            if (playerChoise.IsAoE)
            {
                if (playerChoise.AoeTargetSkill == BSM.BlueTeamInBattle)
                {
                    BlueTeamAoETargetsMenu.gameObject.SetActive(true);
                    BlueTeamAoETargetsMenu.GetChild(0).GetComponent<Button>().interactable = true;
                    BlueTeamAoETargetsMenu.GetChild(1).GetComponent<Button>().interactable = false;
                }
                else if (playerChoise.AoeTargetSkill == BSM.RedTeamInBattle)
                {
                    BlueTeamAoETargetsMenu.gameObject.SetActive(true);
                    BlueTeamAoETargetsMenu.GetChild(0).GetComponent<Button>().interactable = false;
                    BlueTeamAoETargetsMenu.GetChild(1).GetComponent<Button>().interactable = true;
                }
            }
            else if (playerChoise.TargetAlly)
                BlueTeamAllyTargetsMenu.gameObject.SetActive(true);
            else
                BlueTeamEnemyTargetsMenu.gameObject.SetActive(true);
        }
        else if (BSM.CharactersToManage[0].tag == "RedTeam")
        {
            if (playerChoise.IsAoE)
            {
                if (playerChoise.AoeTargetSkill == BSM.BlueTeamInBattle)
                {
                    RedTeamAoETargetsMenu.gameObject.SetActive(true);
                    RedTeamAoETargetsMenu.GetChild(0).GetComponent<Button>().interactable = true;
                    RedTeamAoETargetsMenu.GetChild(1).GetComponent<Button>().interactable = false;
                }
                else if (playerChoise.AoeTargetSkill == BSM.RedTeamInBattle)
                {
                    RedTeamAoETargetsMenu.gameObject.SetActive(true);
                    RedTeamAoETargetsMenu.GetChild(0).GetComponent<Button>().interactable = false;
                    RedTeamAoETargetsMenu.GetChild(1).GetComponent<Button>().interactable = true;
                }
            }
            if (playerChoise.TargetAlly)
                RedTeamAllyTargetsMenu.gameObject.SetActive(true);
            else
                RedTeamEnemyTargetsMenu.gameObject.SetActive(true);
        }
    }

    public void TargetSelection(BaseClass choosenTarget)
    {
        playerChoise.SetTarget(choosenTarget);
        PlayerInput = GUIState.TARGET_SELECTION;
        //BSM.ProcesessingTurn();
        //PlayerInput = GUIState.DONE;
    }
    public void AoETargetSelection(List<BaseClass> targets)
    {
        PlayerInput = GUIState.TARGET_SELECTION;

        //BSM.ProcesessingTurn();
        //PlayerInput = GUIState.DONE;
    }

    void SendTurnParameters()
    {
        BaseClass attacker = playerChoise.Attacker;
        uint attackerId = attacker.ServerID;
        int skillId = attacker.GetIDFromSkill(playerChoise.ChosenAttack);
        uint targetId = 172;
        if (playerChoise.Target != null)
            targetId = playerChoise.Target.ServerID;

        BSM.SetTurnParameters(attackerId, skillId, targetId);
        sendInput = true;
    }

    public void TurnReady()
    {
        BSM.ProcesessingTurn();
        PlayerInput = GUIState.TURN_END;
    }

    public void PlayerInputDone()
    {
        BSM.CharactersToManage[0].owner.CurrentMp -= playerChoise.ManaCost;

        //Blue team
        if (BSM.CharactersToManage[0].tag == "BlueTeam")
        {
            BlueTeamAllyTargetsMenu.gameObject.SetActive(false);
            BlueTeamEnemyTargetsMenu.gameObject.SetActive(false);
            BlueTeamAoETargetsMenu.gameObject.SetActive(false);
            BlueTeamSpellsMenu.gameObject.SetActive(false);
            BlueTeamSelectedSpellInfo.gameObject.SetActive(false);
            BlueTeamCommandMenu.gameObject.SetActive(false);
        }

        if (BSM.CharactersToManage[0].tag == "RedTeam")
        {
            RedTeamAllyTargetsMenu.gameObject.SetActive(false);
            RedTeamEnemyTargetsMenu.gameObject.SetActive(false);
            RedTeamAoETargetsMenu.gameObject.SetActive(false);
            RedTeamSpellsMenu.gameObject.SetActive(false);
            RedTeamSelectedSpellInfo.gameObject.SetActive(false);
            RedTeamCommandMenu.gameObject.SetActive(false);
        }
        BSM.CharactersToManage[0].OnSelection(false);
        BSM.CharactersToManage.RemoveAt(0);
        PlayerInput = GUIState.ACTIVATED;
    }
}