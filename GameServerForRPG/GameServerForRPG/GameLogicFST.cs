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

        private bool turnReady;
        public bool TurnReady { get { return turnReady; } }
        public void SetTurnReady()
        {
            turnReady = true;
        }

        public Turn(RPGHero turnOwner)
        {
            attacker = turnOwner;
            skillID = -1;
            target = null;
            turnReady = false;
        }

        public void SetTurnParamaters(int skillID = -1, RPGHero target = null)
        {
            this.skillID = skillID;
            this.target = target;
        }       
    }

    public class GameLogicFST : GameObject
    {
        public List<Turn> TurnOrder;
        public bool HeroIsRegisted(RPGHero hero)
        {
            foreach (Turn turn in TurnOrder)
                if (turn.Attacker == hero)
                    return true;
            return false;
        }
        public Turn GetTurnFromAttacker(RPGHero attacker)
        {
            foreach (Turn turn in TurnOrder)
                if (turn.Attacker == attacker)
                    return turn;
            return new Turn(null);
        }

        private bool processTurn;
        public bool ProcessTurn { get { return processTurn; } }
        public bool TurnReadyToBeProcessed
        {
            get
            {
                if (TurnOrder.Count > 0)
                    return TurnOrder[0].TurnReady;
                return false;
            }
        }
        public void StartProcessing()
        {
            if (!ProcessTurn)
            {
                processTurn = true;
            }
        }

        public GameLogicFST(uint objectType, GameServer server, GameClient client = null) : base(1, server, client)
        {
            TurnOrder = new List<Turn>();
        }

        public void PrintTurnOrder()
        {
            if (TurnOrder.Count > 0)
            {
                for (int i = 0; i < TurnOrder.Count; i++)
                {
                    Console.WriteLine(TurnOrder[i]);
                }
            }
            else
                Console.WriteLine("NO TURN WAITING");
        }
        public void AddTurn(RPGHero newAttacker)
        {
            if (HeroIsRegisted(newAttacker))
            {
                Turn newTurn = new Turn(newAttacker);
            }
        }

        public void TurnSettings(RPGHero attacker, int skillID = -1, RPGHero target = null)
        {
            if (HeroIsRegisted(attacker))
            {
                Turn selectedTurn = GetTurnFromAttacker(attacker);
                selectedTurn.SetTurnParamaters(skillID, target);

                selectedTurn.SetTurnReady();
            }
        }
        public void GetTurnInfo(ref uint attackerId, ref int skillId, ref uint targetId)
        {
            if (TurnOrder[0].TurnReady)
            {
                Turn turn = TurnOrder[0];
                attackerId = turn.Attacker.ID;
                skillId = turn.SkillID;
                if (turn.Target != null)
                    targetId = turn.Target.ID;
                else
                    targetId = uint.MaxValue;
            }
        }
    }
}
