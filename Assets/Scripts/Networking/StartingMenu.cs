using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartingMenu : MonoBehaviour
{
    GameClient owner;
    public void SetOwner(GameClient client)
    {
        owner = client;
    }

    public TextMeshProUGUI ServerStatusText;
    public Button LogInButton;

    public Dropdown[] InputClasses;
    public InputField[] InputNames;

    public void GetHerosInfo(int heroIndex, ref int classIndex, ref string name)
    {
        classIndex = InputClasses[heroIndex].value;
        name = InputNames[heroIndex].text;
    }

    public GameObject ReadyMenu;
    public TextMeshProUGUI RoomId;
    public TextMeshProUGUI TeamTag;
    public Button ReadyButton;

    public GameObject blueCamera;
    public GameObject BlueUI;
    public RectTransform blueUIpannel;
    public GameObject redCamera;
    public GameObject RedUI;
    public RectTransform redUIpannel;

    // Update is called once per frame
    void Update()
    {
        if (owner != null)
        {
            //Server Status
            ServerStatusText.text = "SERVER STATUS: ";
            if (owner.SeverIsOnline)
                ServerStatusText.text += "<color=green> ONLINE </color>";
            else if (!owner.SeverIsOnline)
            {
                ServerStatusText.text += "<color=red> OFFLINE </color>";
                LogInButton.GetComponentInChildren<Text>().text = "Log In";
            }

            LogInButton.interactable = owner.SeverIsOnline;

            //Log In
            if (owner.ClientHaveLoggedIn)
            {
                LogInButton.interactable = false;
                LogInButton.GetComponentInChildren<Text>().text = "<color=green> LOGGED IN </color>";
            }

            //Ready Menu
            ReadyMenu.SetActive(owner.ClientHaveLoggedIn && owner.SeverIsOnline);
            if (ReadyMenu.activeInHierarchy)
            {
                //Room ID
                RoomId.text = "Room ID: " + owner.ServerRoomID;

                //TeamTag
                string text = owner.ClientTeamTag;
                TeamTag.text = text;

                //Ready Button
                if (owner.ClientIsReadyForStartTheGame)
                    ReadyMenu.GetComponentInChildren<Text>().text = "<color=green> READY!!! </color>";
                else if (!owner.ClientIsReadyForStartTheGame)
                    ReadyMenu.GetComponentInChildren<Text>().text = " Ready? ";

                if (owner.GameStarted)
                    ReadyButton.interactable = false;
            }
        }
    }

    public void LogIn()
    {
        owner.LogIn();
    }

    public void SetCamera(string cameraTag)
    {
        if (cameraTag == "BlueTeam")
        {           
            redUIpannel.anchoredPosition = new Vector2(redUIpannel.anchoredPosition.x, 30000);
            blueCamera.SetActive(true);
            redCamera.SetActive(false);
        }
        else if (cameraTag == "RedTeam")
        {
            blueUIpannel.anchoredPosition = new Vector2(blueUIpannel.anchoredPosition.x, 30000);
            blueCamera.SetActive(false);
            redCamera.SetActive(true);
        }
    }
    public void ActiveUI()
    {
        BlueUI.SetActive(true);
        RedUI.SetActive(true);
    }
}