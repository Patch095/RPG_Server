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
            turnReady = true;
        }

        public void Print()
        {
            if(Target != null)
                Console.WriteLine("{0} attack {1} with skill {2}", Attacker.ID, Target.ID, SkillID);
            else
                Console.WriteLine("{0} attack with skill {1}", Attacker.ID, SkillID);
        }
    }

    public class GameLogicFST : GameObject
    {
        private ServerRoom owner;
        public ServerRoom Room { get { return owner; } }

        public List<Turn> TurnOrder;
        public bool HeroIsRegisted(RPGHero hero)
        {
            foreach (Turn turn in TurnOrder)
                if (turn.Attacker.ID == hero.ID)
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

        public GameLogicFST(uint objectType, GameServer server, ServerRoom room) : base(1, server)
        {
            TurnOrder = new List<Turn>();
            owner = room;
        }

        public void PrintTurnOrder()
        {
            if (TurnOrder.Count > 0)
            {
                for (int i = 0; i < TurnOrder.Count; i++)
                {
                    Console.WriteLine("Attacker: " + TurnOrder[i].Attacker.ID);
                }
            }
            else
                Console.WriteLine("NO TURN WAITING");
        }
        public void AddTurn(RPGHero newAttacker)
        {
            if (!HeroIsRegisted(newAttacker))
            {
                Turn newTurn = new Turn(newAttacker);
                TurnOrder.Add(newTurn);
                PrintTurnOrder();
                Room.CreateTurn(newAttacker.ID);
            }
        }

        public void TurnSettings(RPGHero attacker, int skillID = -1, RPGHero target = null)
        {
            if (HeroIsRegisted(attacker))
            {
                Turn selectedTurn = GetTurnFromAttacker(attacker);
                selectedTurn.SetTurnParamaters(skillID, target);

                if(target != null)
                    Console.WriteLine("Turn Info: " + attacker.ID, skillID, target.ID);
                else
                    Console.WriteLine("Turn Info: " + attacker.ID, skillID);

                uint targetId = 142;
                if (target != null)
                    targetId = target.ID;

                StartProcessing();
                selectedTurn.Print();
                Room.ProcessingTurn(attacker.ID, skillID, targetId);
            }
        }
        public void TurnEnded()
        {
            if (processTurn)
            {
                TurnOrder.RemoveAt(0);
                processTurn = false;
            }
        }
    }
}
