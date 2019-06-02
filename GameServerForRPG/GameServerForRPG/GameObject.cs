using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServerForRPG
{
    abstract public class GameObject
    {
        protected GameServer gameServer;
        public GameServer GameServer { get { return gameServer; } }

        private static uint idCounter;

        protected GameClient owner;
        public GameClient GetOwner()
        {
            return owner;
        }

        public bool isOwnedByThisClient(GameClient client)
        {
            return client == owner;
        }
        public void SetOwner(GameClient client)
        {
            owner = client;
        }

        //indica l'identità diversa di ogni prefab
        protected uint internalID;
        public uint ID
        {
            get
            {
                return internalID;
            }
        }

        //indica il tipo di prefab da istanziare 
        protected uint internalObjectType;
        public uint ObjectType
        {
            get
            {
                return internalObjectType;
            }
        }

        //costruttore di un gameogject
        public GameObject(uint objectType, GameServer server, GameClient client = null)
        {
            gameServer = server;
            internalID = ++idCounter;
            internalObjectType = objectType;

            if (client != null)
                this.owner = client;

            gameServer.RegisterGameObject(internalID, this);
        }

        public virtual void Tick()
        {

        }
    }
}
