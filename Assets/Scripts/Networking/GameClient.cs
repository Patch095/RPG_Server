﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class GameClient : MonoBehaviour
{

    [System.Serializable]
    struct NetPrefab
    {
        public uint Id;
        public GameObject Prefab;
    }

    [SerializeField]
    private NetPrefab[] prefabsTable;
    [SerializeField]
    private string address; //= 127.0.01;
    [SerializeField]
    private int port; // = 9999
    private Dictionary<uint, GameObject> serverPrefabs;

    private Dictionary<uint, GameObject> spawnedGameObjects;

    private Socket socket;
    private IPEndPoint endPoint;

    private uint myIdOnServer;
    public uint MyIDonServer { get { return myIdOnServer; } }
    
    private uint serverRoomId;
    public uint ServerRoomID { get { return serverRoomId; } }

    void Awake()
    {
        serverPrefabs = new Dictionary<uint, GameObject>();
        foreach (NetPrefab prefab in prefabsTable)
        {
            serverPrefabs[prefab.Id] = prefab.Prefab;
        }
        spawnedGameObjects = new Dictionary<uint, GameObject>();

        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        socket.Blocking = false;
        endPoint = new IPEndPoint(IPAddress.Parse(address), port);
    }

    //game logic 
    public GameObject GameLogic;
    BattleStateMachine bsm;
    private uint battleStateMachineServerId;
    public GameObject StartingMenu;
    StartingMenu startMenu;

    //game start
    private bool gameStarted;
    public bool GameStarted { get { return gameStarted; } }
    private bool initializeUI;
    public bool InizitializeUI { get { return initializeUI; } }

    //commands
    private delegate void GameCommand(byte[] data, EndPoint sender);
    private Dictionary<byte, GameCommand> commandsTable;

    //join command
    private bool clientHaveLoggedIn;
    public bool ClientHaveLoggedIn { get { return clientHaveLoggedIn; } }

    private string clientTeamTag;
    public string ClientTeamTag { get { return clientTeamTag; } }
    private string enemyTeamTag;

    //server status command
    private bool serverIsOnline;
    public bool SeverIsOnline { get { return serverIsOnline; } }

    float askServerStatusTimer;
    float serverOfflineTimer;
    float defaultAskServerStatusTimer = 2.5f;
    float defaultServerOfflineTimer = 3f;

    private bool clientIsReadyForStartTheGame;
    public bool ClientIsReadyForStartTheGame { get { return clientIsReadyForStartTheGame; } }

    void Start()
    {
        //GameLogic.SetActive(false);
        startMenu = StartingMenu.GetComponent<StartingMenu>();
        startMenu.SetOwner(this);
        bsm = GameLogic.GetComponent<BattleStateMachine>();
        bsm.SetClient(this);
        clientHaveLoggedIn = false;
        clientIsReadyForStartTheGame = false;

        //networking stuff
        address = "127.0.01";
        port = 9999;

        //   clientID = (uint)DateTime.Now.Ticks;

        commandsTable = new Dictionary<byte, GameCommand>();
        //commandsTable[0] = Join;
        commandsTable[1] = Welcome;
        commandsTable[2] = Spawn;
        //commandsTable[3] = Update;

        //commandsTable[4] = Ready;

        commandsTable[5] = GameStart;
        //commandsTable[6] = UIStart;

        commandsTable[7] = TurnCreation;
        commandsTable[8] = SetTurnParameter;
        commandsTable[9] = ProcessTurn;

        commandsTable[254] = StatusServer;

        //commandsTable[255] = Ack;
    }

    private void ProcessTurn(byte[] data, EndPoint sender)
    {
        //Server make you process the complete turn
        throw new NotImplementedException();
    }
    private void SetTurnParameter(byte[] data, EndPoint sender)
    {
        //add skill/target to your turn 
        throw new NotImplementedException();
    }
    private void TurnCreation(byte[] data, EndPoint sender)
    {
        //Server add a turn to your local turnList
        throw new NotImplementedException();
    }
    public void CharacterATBReadt(BaseClass characterReady)
    {
        Packet createTurnPacket = new Packet(7);
    }

    public void LogIn()//call [0]Join
    {
        Packet joinPacket = new Packet(0);
        socket.SendTo(joinPacket.GetData(), endPoint);       
    }
    private void Welcome(byte[] data, EndPoint sender)
    {
        myIdOnServer = BitConverter.ToUInt32(data, 1);

        serverRoomId = BitConverter.ToUInt32(data, 5);

        battleStateMachineServerId = BitConverter.ToUInt32(data, 9);
        spawnedGameObjects.Add(battleStateMachineServerId, bsm.gameObject);

        byte[] teamTag = new byte[8];
        for(int i = 0; i < teamTag.Length; i++)
        {
            teamTag[i] = data[14 + i];
        }
        string tag = System.Text.Encoding.UTF8.GetString(teamTag);
        if (tag[tag.Length - 1] == '\0')
            tag = "RedTeam";

        clientTeamTag = tag;
        if (ClientTeamTag == "BlueTeam")
            enemyTeamTag = "RedTeam";
        else if (ClientTeamTag == "RedTeam")
            enemyTeamTag = "BlueTeam";

        clientHaveLoggedIn = true;
        
        Debug.Log(serverRoomId);
        Debug.Log("Object Spawned : " + spawnedGameObjects.Count + ";    bst ID : " + battleStateMachineServerId);
        Debug.Log(clientTeamTag);
    }
    private void Spawn(byte[] data, EndPoint sender)
    {
        //Packet spawnPacket = new Packet(2, newHero.ClassID, newHero.ID, newHero.GetOwner().ClientID, newHero.X, newHero.Y, newHero.Z);

        uint prefabType = BitConverter.ToUInt32(data, 1);
        uint objectID = BitConverter.ToUInt32(data, 5);
        //string inGameName = BitConverter.ToString(data, 9);   
        uint ClientID = BitConverter.ToUInt32(data, 9);
        string teamTag = "";
        if (MyIDonServer == ClientID)
            teamTag = ClientTeamTag;
        else if (MyIDonServer != ClientID)
            teamTag = enemyTeamTag;

        float x = BitConverter.ToSingle(data, 13);
        if (teamTag == "BlueTeam")
            x = -x;
        float y = BitConverter.ToSingle(data, 17);
        float z = BitConverter.ToSingle(data, 21);

        if (serverPrefabs.ContainsKey(prefabType))
        {
            GameObject newGameObject = Instantiate(serverPrefabs[prefabType]);

            Vector3 position = new Vector3(x, y, z);
            // newGameObject.name = inGameName + "_" + newGameObject.name;//string.Format("my avatar ID {0}", clientID);
            newGameObject.transform.position = position;
            spawnedGameObjects[objectID] = newGameObject;

            //rotation
            Vector3 rotation = new Vector3(0, 90, 0);
            if (teamTag == "RedTeam")
                rotation *= -1;
            newGameObject.transform.rotation = Quaternion.Euler(rotation);
            BaseClass objectClass = newGameObject.GetComponent<BaseClass>();

            objectClass.SetID(objectID);

            objectClass.TeamTag = teamTag;
            // newGameObject.GetComponent<BaseClass>().CharacterName = inGameName;
            newGameObject.GetComponent<CharacterStateMachine>().BSM = bsm;

            newGameObject.SetActive(true);
            bsm.AddToTeamList(objectClass);
            Console.WriteLine("Spawned");
        }
    }
    public void SetReady()//call [4]Ready
    {
        clientIsReadyForStartTheGame = !clientIsReadyForStartTheGame;
        Debug.Log(ClientIsReadyForStartTheGame);
        Packet readyPacket = new Packet(4, myIdOnServer, serverRoomId, clientIsReadyForStartTheGame);
        socket.SendTo(readyPacket.GetData(), endPoint);
    }
    private void GameStart(byte[] data, EndPoint sender)
    {
        Console.WriteLine("GameStarted!");
        if (!gameStarted)
        {
            int classIndex = int.MaxValue;
            string name = "";
            float x = 7f;
            float y = 1.5f;
            float z = 1f;

            //spawn first hero
            startMenu.GetHerosInfo(0, ref classIndex, ref name);
            //Packet spawn0Packet = new Packet(2, myIDonServer, serverRoomId, classIndex, name, 7, 1, 1);//z = 1,5,10,15
            Packet spawn0Packet = new Packet(2, myIdOnServer, serverRoomId, classIndex, x, y, z);//z = 1,5,10,15
            socket.SendTo(spawn0Packet.GetData(), endPoint);

            //spawn second hero
            z = 5f;
            startMenu.GetHerosInfo(1, ref classIndex, ref name);
            Packet spawn1Packet = new Packet(2, myIdOnServer, serverRoomId, classIndex, x, y, z);
            socket.SendTo(spawn1Packet.GetData(), endPoint);

            //spawn third hero
            z += 5f;
            startMenu.GetHerosInfo(2, ref classIndex, ref name);
            Packet spawn2Packet = new Packet(2, myIdOnServer, serverRoomId, classIndex, x, y, z);
            socket.SendTo(spawn2Packet.GetData(), endPoint);

            //spawn fourth hero
            z += 5f;
            startMenu.GetHerosInfo(3, ref classIndex, ref name);
            Packet spawn3Packet = new Packet(2, myIdOnServer, serverRoomId, classIndex, x, y, z);
            socket.SendTo(spawn3Packet.GetData(), endPoint);

            gameStarted = true;
        }
    }

    public void StartUI()
    {
        startMenu.SetCamera(clientTeamTag);

        bsm.ActiveUI();

        StartingMenu.SetActive(false);

        Console.WriteLine("UI Activated");
    }

    private void StatusServer(byte[] data, EndPoint sender)
    {
        serverIsOnline = BitConverter.ToBoolean(data, 1);
        serverOfflineTimer = defaultServerOfflineTimer;
    }
    void Update()
    {
        //if(!serverIsOnline && clientHaveLoggedIn)
         //   UnityEditor.EditorApplication.isPlaying = false;

        //check server status
        askServerStatusTimer -= Time.deltaTime;
        serverOfflineTimer -= Time.deltaTime;

        if (askServerStatusTimer <= 0)
        {
            Packet serverStatusPacket = new Packet(254);
            socket.SendTo(serverStatusPacket.GetData(), endPoint);
            askServerStatusTimer = defaultAskServerStatusTimer;
        }
        if (serverOfflineTimer < 0)
            serverIsOnline = false;

        //receive command
        const int maxPackets = 100;
        for (int i = 0; i < maxPackets; i++)
        {
            byte[] data = new byte[256];

            int rlen = -1;
            try
            {
                rlen = socket.Receive(data);
            }
            catch
            {
                break;
            }

            if (rlen > 0)
            {
                byte command = data[0];
                Console.WriteLine("command received: {0}", data[0]);
                if (commandsTable.ContainsKey(command))
                    commandsTable[command](data, endPoint);
            }
        }
    }
}