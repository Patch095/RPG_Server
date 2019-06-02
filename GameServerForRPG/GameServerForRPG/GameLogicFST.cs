using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServerForRPG
{
    public struct Turn
    {
        private RPGHero attacker;
        public RPGHero Attacker { get { return attacker; } }
        private int skillID;
        public int SkillID { get { return skillID; } }
        private RPGHero target;
        public RPGHero Target { get { return target; } }

        public Turn(RPGHero turnOwner)
        {
            attacker = turnOwner;
            skillID = int.MinValue;
            target = null;
        }

        public void SetTurnParamaters(int skillID = int.MinValue, RPGHero target = null)
        {
            this.skillID = skillID;
            this.target = target;
        }
    }

    public class GameLogicFST : GameObject
    {
        public Dictionary<RPGHero, Turn> TurnOrder;

        public GameLogicFST(uint objectType, GameServer server, GameClient client = null) : base(1, server, client)
        {
            TurnOrder = new Dictionary<RPGHero, Turn>();
        }

        public void AddTurn(RPGHero attacker)
        {
            if(TurnOrder.ContainsKey(attacker))
            {
                Turn newTurn = new Turn(attacker);
            }
        }

        public void TurnSettings(RPGHero attacker, int skillID = int.MinValue, RPGHero target = null)
        {
            if (TurnOrder.ContainsKey(attacker))
            {
                Turn selectedTurn = TurnOrder[attacker];
                if (selectedTurn.Attacker == attacker)
                    selectedTurn.SetTurnParamaters(skillID, target);
            }
        }

        public void ProcessTurn()
        {

        }
    }
}
