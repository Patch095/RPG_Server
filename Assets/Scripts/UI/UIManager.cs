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
        //disactive
        //creation
        ACTIVATED,
        PREPARATION,
        IDLE,
        ATTACK,
        MAGIC,
        TARGET_SELECTION,
        DONE,
        CANCEL
    }
    public GUIState PlayerInput;

    //Blue Team UI
    public Transform BlueTeamCharactersMenu;
    public Transform BlueTeamAllyTargetsMenu;
    public Transform BlueTeamEnemyTargetsMenu;
    Dictionary<TargetSelectButton, Button> BlueTeamTarges;
    public GameObject BlueTeamCommandMenu;
    public Transform BlueTeamSpellsMenu;
    public Transform BlueTeamSelectedSpellInfo;
    public GameObject CancelButtonBlueTeam;

    //Red Team UI

    public bool UIStarted;

    private void OnEnable()
    {
        UIStarted = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        BSM = GetComponent<BattleStateMachine>();

        //SetUI();
        PlayerInput = GUIState.ACTIVATED;
        BlueTeamCharactersMenu.gameObject.SetActive(false);
        BlueTeamCommandMenu.SetActive(false);
        BlueTeamAllyTargetsMenu.gameObject.SetActive(false);
        BlueTeamEnemyTargetsMenu.gameObject.SetActive(false);
        BlueTeamSpellsMenu.gameObject.SetActive(false);
        BlueTeamSelectedSpellInfo.gameObject.SetActive(false);
    }

    public void SetUI()
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
                targerButton.UImanager = this;
                targerButton.Target = BSM.RedTeamInBattle[i];
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
                targerButton.UImanager = this;
                targerButton.Target = BSM.BlueTeamInBattle[i];
                BlueTeamTarges.Add(targerButton, button.GetComponent<Button>());
            }
            BlueTeamCharactersMenu.gameObject.SetActive(true);

            PlayerInput = GUIState.ACTIVATED;

            UIStarted = true;
        }
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
                    break;

                case GUIState.DONE:
                    PlayerInputDone();
                    break;

                case GUIState.CANCEL:
                    CancelInput();
                    break;

                case GUIState.DONE:
                    PlayerInputDone();
                    break;
        }

        foreach(KeyValuePair<TargetSelectButton, Button> keyValue in BlueTeamTarges)
        {
            TargetSelectButton key = keyValue.Key;
            Button value = keyValue.Value;
            if(BSM.BattleState == BattleStateMachine.PerformAction.PERFORM_ACTION)
                value.interactable = false;
            else
                value.interactable = key.Target.IsAlive;

            Text buttonText = value.GetComponentInChildren<Text>();
            buttonText.text = key.Target.Name + "\n" + (int) key.Target.CurrentHp + "/" + key.Target.MaxHp;

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
        }

        CancelButtonBlueTeam.SetActive(BlueTeamAllyTargetsMenu.gameObject.activeInHierarchy || BlueTeamEnemyTargetsMenu.gameObject.activeInHierarchy);
    }

    public void AttackInput()
    {
        BlueTeamSpellsMenu.gameObject.SetActive(false);
        playerChoise.DamageValue = (float)BSM.CharactersToManage[0].owner.BaseAtk;
        playerChoise.actionType = Turn.AnimationType.MEELE;

        if (BSM.CharactersToManage[0].tag == "BlueTeam")
            BlueTeamEnemyTargetsMenu.gameObject.SetActive(true);
    }

    public void CancelInput()
    {
        PlayerInput = GUIState.ACTIVATED;
        BlueTeamCommandMenu.SetActive(false);
        BlueTeamAllyTargetsMenu.gameObject.SetActive(false);
        BlueTeamEnemyTargetsMenu.gameObject.SetActive(false);
        BlueTeamSpellsMenu.gameObject.SetActive(false);
        BlueTeamSelectedSpellInfo.gameObject.SetActive(false);
        playerChoise.chosenAttack = null;

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
    }

    public void MagicSelection(BaseAttack Spell)
    {
        Spell.TurnInfo = playerChoise;
        playerChoise.chosenAttack = Spell;
        if (playerChoise.IsAoE)
            AoETargetSelection();
        else
        {
            //check team e target
            if (BSM.CharactersToManage[0].tag == "BlueTeam")
            {
                if(playerChoise.TargetAlly)
                    BlueTeamAllyTargetsMenu.gameObject.SetActive(true);
                else
                    BlueTeamEnemyTargetsMenu.gameObject.SetActive(true);
            }
        }
    }
    void AoETargetSelection()
    {
        PlayerInput = GUIState.DONE;
    }

    public void TargetSelection(BaseClass choosenTarget)
    {
        playerChoise.Target = choosenTarget;
        BSM.ProcesessingTurn();
        PlayerInput = GUIState.DONE;
    }

    void PlayerInputDone()
    {
        BSM.CharactersToManage[0].owner.CurrentMp -= playerChoise.ManaCost;
        if (BSM.CharactersToManage[0].tag == "BlueTeam")
        {
            BlueTeamAllyTargetsMenu.gameObject.SetActive(false);
            BlueTeamEnemyTargetsMenu.gameObject.SetActive(false);
        }
        BlueTeamSpellsMenu.gameObject.SetActive(false);
        BlueTeamSelectedSpellInfo.gameObject.SetActive(false);
        BlueTeamCommandMenu.gameObject.SetActive(false);
        BSM.CharactersToManage[0].OnSelection(false);
        BSM.CharactersToManage.RemoveAt(0);
        PlayerInput = GUIState.ACTIVATED;
    }
}