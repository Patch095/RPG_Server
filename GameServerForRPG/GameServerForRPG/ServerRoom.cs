﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace GameServerForRPG
{
    public class ServerRoom
    {
        private GameServer server;
        private uint roomID;
        public uint RoomID { get { return roomID; } }

        private Dictionary<uint, GameObject> gameObjectsTable;
        public Dictionary<uint, GameObject> GetGameObjectTable()
        {
            return gameObjectsTable;
        }
        public void RegisterGameObject(uint id, GameObject newObject)
        {
            gameObjectsTable.Add(id, newObject);
            Console.WriteLine("Room {0}, Spawned GameObject: {1}", roomID, gameObjectsTable.Count);
        }
        public void RegisterGameObject(GameObject gameObject)
        {
            if (gameObjectsTable.ContainsKey(gameObject.ID))
                throw new Exception("GameObject already registered");
            gameObjectsTable[gameObject.ID] = gameObject;
            Console.WriteLine("Room {0}, Spawned GameObject: {1}", roomID, gameObjectsTable.Count);
        }
        public GameObject GetGameObjectFromId(uint id)
        {
            if (gameObjectsTable.ContainsKey(id))
                return gameObjectsTable[id];
            return null;
        }

        const int ROOM_SIZE = 2;

        private List<GameClient> clientsTable;
        public bool RoomContainsThisClient(GameClient newClient)
        {
            foreach (GameClient client in clientsTable)
                if (client.ClientID == newClient.ClientID)
                    return true;
            return false;
        }

        public List<GameClient> GetClientTable()
        {
            return clientsTable;
        }
        public GameClient GetClient(int clientIndex)
        {
            if(clientsTable.Count > clientIndex)
            {
                return clientsTable[clientIndex];
            }
            return null;
        }
        public GameClient Player1 { get { return clientsTable[0]; } }
        public GameClient Player2
        {
            get
            {
                if(clientsTable.Count > 1)
                    return clientsTable[1];
                return null;
            }
        }
        public bool IsOccupy
        {
            get
            {
                return clientsTable.Count >= ROOM_SIZE;
            }
        }

        public void SetPlayersReady(GameClient client, bool isClientReady)
        {
            if (clientsTable.Contains(client))
            {
                Console.WriteLine(client.IsReady);
                client.SetReady(isClientReady);
                Console.WriteLine(client.IsReady);

                if (Player1 != null && Player2 != null)
                    if (Player1.IsReady && Player2.IsReady && !gameStarted)
                        server.GameStart(this);
            }
        }

        private bool gameStarted;
        public bool GameStarted { get { return gameStarted; } }
        public void StartGame()
        {
            gameStarted = true;
        }
        private bool spawnTeam;
        public bool SpawnTeam { get { return spawnTeam; } }
        public void TeamSpawned()
        {
            spawnTeam = true;
        }

        public void AddGameClient(GameClient newGameClient)
        {
            if (clientsTable.Count < ROOM_SIZE)
            {
                clientsTable.Add(newGameClient);
                if (newGameClient == Player1)
                    newGameClient.EnterInARoom(this, "BlueTeam");
                else if (newGameClient == Player2)
                    newGameClient.EnterInARoom(this, "RedTeam");
            }
        }

        private GameLogicFST gameLogic;
        public void AddNewTurn(RPGHero attacker)
        {
            gameLogic.AddTurn(attacker);
        }
        public void SetTurnParameters(RPGHero attacker, int skillId = -1, RPGHero target = null)
        {
            gameLogic.TurnSettings(attacker, skillId, target);
        }

        public ServerRoom(uint id, GameServer gameServer)
        {
            roomID = id;
            server = gameServer;

            clientsTable = new List<GameClient>(ROOM_SIZE);
            gameObjectsTable = new Dictionary<uint, GameObject>();

            gameLogic = new GameLogicFST(1, server);

            gameStarted = false;
            spawnTeam = false;
        }

        public void Update()
        {
            foreach (GameClient client in clientsTable)
                client.Process();

            foreach (GameObject gameObj in gameObjectsTable.Values)
                gameObj.Tick();

            if (Player1 != null && Player2 != null)
                if (Player1.IsReady && Player2.IsReady && !gameStarted)
                    server.GameStart(this);

            if (gameStarted && !spawnTeam && gameObjectsTable.Count >= 8)
                server.SpawnHeroes(this);

            if (gameLogic.TurnReadyToBeProcessed && !gameLogic.ProcessTurn)
            {
                uint attackerId = uint.MinValue;
                int skillId = int.MinValue;
                uint targetId = uint.MinValue;
                gameLogic.GetTurnInfo(ref attackerId, ref skillId, ref targetId);
                if (targetId == uint.MaxValue)
                    targetId = 172;

                server.ProcessingTurn(this, attackerId, skillId, targetId);
                gameLogic.StartProcessing();
            }

        }



        //scripts for test
        public int GetGameObjectsNumber()
        {
            return gameObjectsTable.Count;
        }
        public int GetClientsNumber()
        {
            Console.WriteLine("Room ID: {0}, {1} clients", RoomID, clientsTable.Count);
            return clientsTable.Count;
        }
    }
}
