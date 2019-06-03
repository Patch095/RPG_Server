using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    BattleStateMachine BSM;

    private Turn playerChoise;
    private bool sendInput;

    public enum GUIState
    {
        //disactive
        //creation
        ACTIVATED,
        PREPARATION,
        IDLE,
        ATTACK,
        MAGIC,
        TARGET_SELECTION,
        PLAYER_INPUT_DONE,
        DONE,
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
    public GameObject CancelButtonBlueTeam;
    public GameObject RedPlayerWaitingPanel;

    //Red Team UI
    public Transform RedTeamCharactersMenu;
    Dictionary<TargetSelectButton, Button> RedTeamTarges;
    public Transform RedTeamAllyTargetsMenu;
    public Transform RedTeamEnemyTargetsMenu;
    public Transform RedTeamAoETargetsMenu;
    public GameObject RedTeamCommandMenu;
    public Transform RedTeamSpellsMenu;
    public Transform RedTeamSelectedSpellInfo;
    public GameObject CancelButtonRedTeam;
    public GameObject BluePlayerWaitingPanel;


    public bool UIStarted;

    private void OnEnable()
    {
        UIStarted = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        BSM = GetComponent<BattleStateMachine>();
        
        BlueTeamTarges = new Dictionary<TargetSelectButton, Button>();

        RedTeamTarges = new Dictionary<TargetSelectButton, Button>();

        SetUIBlueTeam();
        SetUIRedTeam();
        sendInput = false;

        //SetUI();
        PlayerInput = GUIState.ACTIVATED;

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

    public void SetUIBlueTeam()
    {
        if (!UIStarted)
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
            BlueTeamCharactersMenu.gameObject.SetActive(true);

            PlayerInput = GUIState.ACTIVATED;

            UIStarted = true;
        }
    }
    public void SetUIRedTeam()
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
        //targetTeam prendi il figlio 0 , gli assegni lo script nuovo e come stringa il team blue , set active a false , stessa cosa per il team rosso ;
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
                    BlueTeamCommandMenu.SetActive(true);
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
                        SendTurnParameters();
                    break;
                
                case GUIState.PLAYER_INPUT_DONE:
                
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

        CancelButtonBlueTeam.SetActive(BlueTeamAllyTargetsMenu.gameObject.activeInHierarchy || BlueTeamEnemyTargetsMenu.gameObject.activeInHierarchy);
        CancelButtonRedTeam.SetActive(RedTeamAllyTargetsMenu.gameObject.activeInHierarchy || RedTeamEnemyTargetsMenu.gameObject.activeInHierarchy);

        if (playerChoise == null || (playerChoise != null && playerChoise.Attacker != null))
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
        playerChoise.actionType = Turn.AnimationType.MEELE;

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
                BlueTeamAoETargetsMenu.gameObject.SetActive(true);
                BlueTeamAoETargetsMenu.GetChild(0).GetComponent<Button>().interactable = false; //button team rosso not interactable
            }
            else if (playerChoise.TargetAlly)
                BlueTeamAllyTargetsMenu.gameObject.SetActive(true);
            else
                BlueTeamEnemyTargetsMenu.gameObject.SetActive(true);
        }
        else if (BSM.CharactersToManage[0].tag == "RedTeam")
        {
            RedTeamAoETargetsMenu.gameObject.SetActive(true);
            RedTeamAoETargetsMenu.GetChild(1).GetComponent<Button>().interactable = false;

            if (playerChoise.TargetAlly)
                RedTeamAllyTargetsMenu.gameObject.SetActive(true);
            else
                RedTeamEnemyTargetsMenu.gameObject.SetActive(true);
        }
    }
        
    void SendTurnParameters()
    {
        BaseAttack atk = playerChoise.ChosenAttack;
        BaseClass target = playerChoise.Target;

        BSM.SetTurnParameters();
        sendInput = true;
    }

    public void TargetSelection(BaseClass choosenTarget)
    {
        playerChoise.SetTarget(choosenTarget);
        BSM.ProcesessingTurn();
        PlayerInput = GUIState.DONE;
    }
    public void AoETargetSelection(List<BaseClass> targets)
    {
        if(targets == playerChoise.AoeTargetSkill)
        {
            BSM.ProcesessingTurn();
            PlayerInput = GUIState.DONE;
        }
    }

    void PlayerInputDone()
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