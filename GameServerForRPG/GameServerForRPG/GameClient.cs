using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace GameServerForRPG
{
    public class GameClient
    {
        private GameServer gameServer;
        private ServerRoom room;
        public ServerRoom Room
        {
            get
            {
                if (gameServer != null)
                    return room;
                else
                    return null;
            }
        }

        private static uint idCounter;
        private uint clientID;
        public uint ClientID { get { return clientID; } }

        private string teamTag;
        public string TeamTag { get { return teamTag; } }

        public void EnterInARoom(ServerRoom serverRoom, string tag)
        {
            room = serverRoom;
            teamTag = tag;
        }

        private GameLogicFST clientFST;
        public void SetClientFSM(GameLogicFST FSM)
        {
            if (clientFST == null)
                clientFST = FSM;
        }
        public GameLogicFST GetClientFST()
        {
            return clientFST;
        }

        //indirizzo 
        private EndPoint endPoint;
        public EndPoint GetEndPoint()
        {
            return endPoint;
        }

        //numero di tentativi per i pacchetti non inviati
        private int maxAttemps = 3;
        //coda di pacchetti da inviare
        private Queue<Packet> sendQueue;

        //rappresenta l'identità di ogni pacchetto che ha bisogno di un ACK
        private Dictionary<uint, Packet> waitingForAck;
        //punti negativi per i client 
        private uint malus;
        public uint Malus { get{ return malus; } }
        public void UpdateMalus(uint malusValue = 1)
        {
            malus += malusValue;
        }

        private float lastUpdate;
        public void MarkAsAlive()
        {
            lastUpdate = gameServer.Now;
        }
        public bool isDead
        {
            get
            {
                return gameServer.Now - lastUpdate > 30;
            }
        }

        private bool isReady;
        public bool IsReady { get { return isReady; } }
        public void SetReady(bool ready)
        {
            if (room != null)
                isReady = ready;
        }
        
        //costruttore di un client
        public GameClient(EndPoint endPoint, GameServer server)
        {
            gameServer = server;
            clientID = ++idCounter;
            //crea solo un punto di ricezione e un contenitore di pacchetti
            this.endPoint = endPoint;
            sendQueue = new Queue<Packet>();
            waitingForAck = new Dictionary<uint, Packet>();
        }

        //rappresenta l'update del client
        public void Process()
        {
            //grandezza della coda di pacchetti
            int packetsToSend = sendQueue.Count;

            //mette in coda i pacchetti
            for (int i = 0; i < packetsToSend; i++)
            {
                //assegna il primo pacchetto della coda
                Packet packet = sendQueue.Dequeue();

                //se l'orario corrente(now) è sucessivo all'orario del pacchetto , invialo
                if (gameServer.Now >= packet.SendAfter)
                {
                    //aumenta di un tentativo
                    packet.IncreaseAttempts();

                    //ritorna true se riesce ad inviare il pacchetto
                    if (gameServer.Send(packet, endPoint))
                    {
                        //se il pacchetto ha bisogno di un Ack
                        if (packet.NeedAck)
                        {
                            waitingForAck[packet.Id] = packet;//???
                        }
                    }
                    else if (!packet.OneShot)//se è un pacchetto che può essere perso
                    {
                        //finchè non ha superato il limite di tentativi stabilito
                        if (packet.Attempts < maxAttemps)
                        {
                            //dopo un secondo lo rimette in coda
                            packet.SendAfter = gameServer.Now + 1.0f;
                            sendQueue.Enqueue(packet);
                        }
                    }
                }
                else
                {
                    //se non riesce ad inviare un pacchetto lo rimette in coda
                    sendQueue.Enqueue(packet);
                }
            }

            //lista dei pacchetti scaduti
            List<uint> packetsToRemove = new List<uint>();

            //per ogni pacchetto che necessità di una risposta di ritorno
            foreach (uint key in waitingForAck.Keys)
            {
                Packet packet = waitingForAck[key];

                //se il pacchetto è scaduto
                if (packet.IsExpired(gameServer.Now))
                {
                    //se non ha superato i tentativi massimi
                    if (packet.Attempts < maxAttemps)
                    {
                        sendQueue.Enqueue(packet);//lo rimette in coda per essere inviato 
                    }
                    else
                    {
                        packetsToRemove.Add(key);//lo aggiunge alla lista pacchetti da eliminare
                    }
                }
            }

            foreach (uint packetId in packetsToRemove)
            {
                //rimuove dalla lista i pacchetti da eliminare
                waitingForAck.Remove(packetId);
            }
        }

        //controllo delle risposte dei pacchetti ricevuti da un client
        public void CheckAck(uint packetID)
        {
            //se l'ACK appartiene a un pacchetto non presente nel dizionario lo rimuove
            if (waitingForAck.ContainsKey(packetID))
            {
                waitingForAck.Remove(packetID);
            }
            else
            {
                //incrementa il malus
            }
        }

        public void Enqueue(Packet packet)
        {
            sendQueue.Enqueue(packet);
        }

        public override string ToString()
        {
            return endPoint.ToString();
        }
    }
}
