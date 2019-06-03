using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServerForRPG
{
    public class RPGHero : GameObject
    {
        public RPGHero(uint objectType, GameServer server, GameClient client = null) : base(2, server, client)
        {
            classID = uint.MaxValue;
        }

        private float x, y, z;
        public float X { get { return x; } }
        public float Y { get { return y; } }
        public float Z { get { return z; } }
        public void SetPosition(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        private uint classID;
        public uint ClassID { get { return classID; } }
        private string inGameName;
        public string InGameName { get { return inGameName; } }
        public void SetInGameValues(uint classId, string name)
        {
            classID = classId;
            inGameName = name;
        }
        public string TeamTag { get { return owner.TeamTag; } }
    }
}
