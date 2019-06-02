using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace GameServerForRPG
{
    public class GameServer
    {
        private IGameTransport serverTransport;
        private IMonotonicClock serverClock;
        private float currentNow;
        public float Now { get { return serverClock.GetNow(); } }

        private Dictionary<uint, ServerRoom> serverRooms;
        private uint roomID;
        private ServerRoom GetEmptyRoom()
        {
            if (serverRooms != null)
            {
                //find a empty room for a client to join
                for (uint i = 0; i < serverRooms.Count; i++)
                {
                    if (!serverRooms[i].IsOccupy)
                    {
                        Console.WriteLine("Selected  room ID: " + serverRooms[i].RoomID);
                        return serverRooms[i];
                    }
                }
                //if there isn't create a new
                ServerRoom newRoom = new ServerRoom(roomID, this);
                serverRooms.Add(roomID, newRoom);
                Console.WriteLine("Create new room ID: " + newRoom.RoomID);
                roomID++;
                return newRoom;
            }
            else
                return null;
        }
        public ServerRoom GetRoomFromID(uint id)
        {
            if (serverRooms.ContainsKey(id))
                return serverRooms[id];
            return null;
        }

        private Dictionary<EndPoint, GameClient> clientsTable;
        public GameClient GetClientFromID(uint id)
        {
            foreach (GameClient client in clientsTable.Values)
                if (client.ClientID == id)
                    return client;
            return null;
        }

        private delegate void GameCommand(byte[] data, EndPoint sender);
        private Dictionary<byte, GameCommand> commandsTable;

        public GameServer(IGameTransport transport, IMonotonicClock clock)
        {
            serverTransport = transport;
            serverClock = clock;

            clientsTable = new Dictionary<EndPoint, GameClient>();
            serverRooms = new Dictionary<uint, ServerRoom>();

            commandsTable = new Dictionary<byte, GameCommand>();
            commandsTable[0] = Join;
            // commandsTable[1] = Welcome;
            commandsTable[2] = Spawn;
            // commandsTable[3] = Update;
            commandsTable[4] = Ready;
            //commandsTable[5] = GameStart;
            //commandsTable[6] = UIStart;

            commandsTable[7] = TurnCreation;
            commandsTable[8] = SetTurnParameter;
            commandsTable[9] = ProcessTurn;

            commandsTable[254] = StatusServer;
        }

        private void ProcessTurn(byte[] data, EndPoint sender)
        {
            //after SetTurnParameter send a full create turn to both client, then they process it
            throw new NotImplementedException();
        }
        private void SetTurnParameter(byte[] data, EndPoint sender)
        {
            //receive a skillId/target from clients
            throw new NotImplementedException();
        }
        private void TurnCreation(byte[] data, EndPoint sender)
        {
            //receive a RPGhero and add it to the turnOrder 
            throw new NotImplementedException();
        }

        //commandsTable
        private void Join(byte[] data, EndPoint sender)
        {
            Console.WriteLine("Join");
            foreach (ServerRoom serverRoom in serverRooms.Values)
            {
                if(clientsTable.ContainsKey(sender))
                {
                    GameClient badClient = clientsTable[sender];
                    badClient.UpdateMalus();
                    Console.WriteLine("Error! Client already logged in; client endpoint IP {0} malus updated: {1}", badClient.ToString(), badClient.Malus);
                    return;
                }
            }
            GameClient newClient = new GameClient(sender, this);
            ServerRoom room = GetEmptyRoom();
            clientsTable[sender] = newClient;
            room.AddGameClient(newClient);

            GameLogicFST clientFST = new GameLogicFST(0, this, newClient);
            RegisterGameObject(room.RoomID, clientFST);

            newClient.SetClientFSM(clientFST);

            Console.WriteLine(newClient.TeamTag);
            Packet welcomePacket = new Packet(1, newClient.ClientID, room.RoomID, clientFST.ID, newClient.TeamTag);
            welcomePacket.NeedAck = true;
            newClient.Enqueue(welcomePacket);
        }
        /*
        public T Spawn<T>(uint roomID) where T : GameObject
        {
            object[] ctorParams = { this };

            T newGameObject = Activator.CreateInstance(typeof(T), ctorParams) as T;
            RegisterGameObject(roomID, newGameObject);
            return newGameObject;

            /*

            Packet welcome = new Packet(1, avatar.ObjectType, avatar.Id, avatar.X, avatar.Y, avatar.Z);
            welcome.NeedAck = true;
            newClient.Enqueue(welcome);

            // spawn all server's objects in the new client
            foreach (GameObject gameObject in gameObjectsTable.Values)
            {
                //ignore myself
                if (gameObject == avatar)
                    continue;
                Packet spawn = new Packet(2, gameObject.ObjectType, gameObject.Id, gameObject.X, gameObject.Y, gameObject.Z);//1 - 5*4
                spawn.NeedAck = true;
                newClient.Enqueue(spawn);
            }

            // informs the other clients about the new one
            Packet newClientSpawned = new Packet(2, avatar.ObjectType, avatar.Id, avatar.X, avatar.Y, avatar.Z);
            newClientSpawned.NeedAck = true;
            SendToAllClientsExceptOne(newClientSpawned, newClient);

            Console.WriteLine("client {0} joined with avatar {1}", newClient, avatar.Id);
        }

             *             // check if the client has already joined
            if (clientsTable.ContainsKey(sender))
            {
                GameClient badClient = clientsTable[sender];
                badClient.IncreaseMalus();
                return;
            }

            GameClient newClient = new GameClient(sender, this);
            clientsTable[sender] = newClient;
            //Avatar avatar = new Avatar(newClient.Server);
            Avatar avatar = Spawn<Avatar>();
            avatar.SetOwner(newClient);
            Packet welcome = new Packet(1, avatar.ObjectType, avatar.Id, avatar.X, avatar.Y, avatar.Z);
            welcome.NeedAck = true;
            newClient.Enqueue(welcome);

            // spawn all server's objects in the new client
            foreach (GameObject gameObject in gameObjectsTable.Values)
            {
                //ignore myself
                if (gameObject == avatar)
                    continue;
                Packet spawn = new Packet(2, gameObject.ObjectType, gameObject.Id, gameObject.X, gameObject.Y, gameObject.Z);//1 - 5*4
                spawn.NeedAck = true;
                newClient.Enqueue(spawn);
            }

            // informs the other clients about the new one
            Packet newClientSpawned = new Packet(2, avatar.ObjectType, avatar.Id, avatar.X, avatar.Y, avatar.Z);
            newClientSpawned.NeedAck = true;
            SendToAllClientsExceptOne(newClientSpawned, newClient);

            Console.WriteLine("client {0} joined with avatar {1}", newClient, avatar.Id);
          
            uint prefabType = BitConverter.ToUInt32(data, 1);
        uint objectID = BitConverter.ToUInt32(data, 5);
        string inGameName = BitConverter.ToString(data, 9);

        string teamTag = BitConverter.ToString(data, 13);

        float x = BitConverter.ToUInt32(data, 17);
        if (teamTag == "RedTeam")
            x = -x;
        float y = BitConverter.ToUInt32(data, 21);
        float z = BitConverter.ToUInt32(data, 25);
            
        }*/

        private void Spawn(byte[] data, EndPoint sender)
        {
            //2[0], myIdOnServer[1], serverRoomId[5], classIndex[9], x[13], y[17], z[21]
            uint clientId = BitConverter.ToUInt32(data, 1);
            GameClient client = GetClientFromID(clientId);

            uint roomId = BitConverter.ToUInt32(data, 5);
            ServerRoom room = GetRoomFromID(roomId);

            if (!room.GetClientTable().Contains(client))
            {
                client.UpdateMalus();
                return;
            }

            RPGHero newHero = SpawnHero(roomId);
            newHero.SetOwner(client);

            uint classId = BitConverter.ToUInt32(data, 9);
            //string inGameName = BitConverter.ToString(data, 13);
            newHero.SetInGameValues(classId, "");//, inGameName);
            float x = BitConverter.ToSingle(data, 13);// 17);
            float y = BitConverter.ToSingle(data, 17);// 21);
            float z = BitConverter.ToSingle(data, 21);// 25);
            newHero.SetPosition(x, y, z);

            Console.WriteLine("Spaned Object {0}", room.GetGameObjectTable().Count);
            //Packet spawnPacket = new Packet(2, newHero.ClassID, newHero.ID, newHero.InGameName, newHero.TeamTag, newHero.X, newHero.Y, newHero.Z);
            //Packet spawnPacket = new Packet(2, newHero.ClassID, newHero.ID, newHero.GetOwner().ClientID, newHero.X, newHero.Y, newHero.Z);
            //spawnPacket.NeedAck = true;
            //client.Enqueue(spawnPacket);
            //SendToOtherClientsInARoom(spawnPacket, room, client);
            //SendToAllClientsInARoom(spawnPacket, room);
            //Send(spawnPacket, room.Player1.GetEndPoint());
            //Send(spawnPacket, room.Player2.GetEndPoint());

        }
        private RPGHero SpawnHero(uint roomID)
        {
            RPGHero newHero = new RPGHero(2, this);
            RegisterGameObject(roomID, newHero);
            return newHero;
        }

        private void Ready(byte[] data, EndPoint sender)
        {
            uint clientId = BitConverter.ToUInt32(data, 1);
            GameClient client = GetClientFromID(clientId);
            uint roomId = BitConverter.ToUInt32(data, 5);
            bool isClientReady = BitConverter.ToBoolean(data, 9);
            Console.WriteLine(isClientReady);
            ServerRoom room = GetRoomFromID(roomId);
            if (room != null && room.GetClientTable().Contains(client))
                room.SetPlayersReady(client, isClientReady);
        }
        public void GameStart(ServerRoom room)
        {
            //Console.WriteLine("room " + room.RoomID + room.TeamSpawned);
            if (!room.GameStarted)
            { 
                Console.WriteLine("Game start in room " + room.RoomID);
                Packet startGamePacket = new Packet(5);
                startGamePacket.NeedAck = true;

                //SendToAllClientsInARoom(startGamePacket, room);
                Send(startGamePacket, room.Player1.GetEndPoint());
                Send(startGamePacket, room.Player2.GetEndPoint());

                room.StartGame();
            }
        }
        public void SpawnHeroes(ServerRoom room)
        {
            if (!room.SpawnTeam)
            {
                Dictionary<uint, GameObject> gameObjectTable = room.GetGameObjectTable();                

                foreach (GameObject obj in gameObjectTable.Values)
                {
                    if (obj is RPGHero)
                    {
                        RPGHero hero = (RPGHero)obj;
                        Packet spawnPacket = new Packet(2, hero.ClassID, hero.ID, hero.GetOwner().ClientID, hero.X, hero.Y, hero.Z);
                        spawnPacket.NeedAck = true;

                        Send(spawnPacket, room.Player1.GetEndPoint());
                        Send(spawnPacket, room.Player2.GetEndPoint());
                    }
                    else
                        continue;
                }
                room.TeamSpawned();
            }
        }

        public void StatusServer(byte[] data, EndPoint sender)
        {
            Packet serverStatus = new Packet(254, true);
            serverStatus.NeedAck = true;

            GameClient newClient = new GameClient(sender, this);
            newClient.Enqueue(serverStatus);
            newClient.Process();
            Console.WriteLine("{0} asked server status, server is ONLINE", sender);
        }

        public void Start()
        {
            Console.WriteLine("server started");
            while (true)
            {
                SingleStep();
            }
        }
        public void SingleStep()
        {
            currentNow = serverClock.GetNow();
            EndPoint sender = serverTransport.CreateEndPoint();
            byte[] data = serverTransport.Recv(256, ref sender);
            if (data != null)
            {
                //process command
                byte gameCommand = data[0];
                Console.WriteLine("command received: {0}", data[0]);
                if (commandsTable.ContainsKey(gameCommand))
                {
                    Console.WriteLine("{0} : {1}", data[0], commandsTable[gameCommand].Method);
                    commandsTable[gameCommand](data, sender);
                }
                else
                {
                    Console.WriteLine("{0} : Command Invalid", data[0], commandsTable[gameCommand].Method);
                    GameClient badClient = new GameClient(sender, this);
                    if (clientsTable.ContainsValue(badClient))
                        badClient.UpdateMalus();
                }

                foreach (ServerRoom room in serverRooms.Values)
                    room.Update();
            }
        }

        public bool Send(Packet packet, EndPoint endPoint)
        {
            return serverTransport.Send(packet.GetData(), endPoint);
        }
        public void SendToAllClientsInARoom(Packet packet, ServerRoom room)
        {
            //room.Player1.Enqueue(packet);
            //room.Player2.Enqueue(packet);
            foreach (GameClient client in room.GetClientTable())
            {
                client.Enqueue(packet);
                Console.WriteLine("Send");
            }
        }
        public void SendToOtherClientsInARoom(Packet packet, ServerRoom room, GameClient sender = null)
        {
            /*
            if (sender != room.Player1 && room.Player1 != null)
                room.Player1.Enqueue(packet);
            else if (sender != room.Player2 && room.Player2 != null)
                room.Player2.Enqueue(packet);
            */
            foreach (GameClient client in room.GetClientTable())
            {
                if (client != null && client != sender)
                {
                    client.Enqueue(packet);
                    Console.WriteLine("Send");
                }
            }
        }

        public bool RegisterGameObject(uint roomId, uint id, GameObject gameObj)
        {
            ServerRoom room = GetRoomFromID(roomId);
            if (room == null)
                return false;
            room.RegisterGameObject(id, gameObj);
            return true;
        }
        public bool RegisterGameObject(uint roomId, GameObject gameObject)
        {
            ServerRoom room = GetRoomFromID(roomId);
            if (room == null)
                return false;
            GetRoomFromID(roomId).RegisterGameObject(gameObject);
            return true;
        }
        public GameObject GetGameObject(uint roomId, uint objId)
        {
            ServerRoom room = GetRoomFromID(roomId);
            if (room == null)
                return null;

            if (room.GetGameObjectTable().ContainsKey(objId))
                return room.GetGameObjectFromId(objId);
            else
                return null;
        }



        //scripts for test
        public int GetNumberOfRoom()
        {
            return serverRooms.Count;
        }
        public int GetAllClientsConnetedToThisServer()
        {
            return clientsTable.Count();
        }
        public int GetAllGameObjectsSpawnedInThisServer()
        {
            int objectsNumbers = 0;
            foreach (ServerRoom room in serverRooms.Values)
                objectsNumbers += room.GetGameObjectsNumber();
            return objectsNumbers;
        }
    }
}