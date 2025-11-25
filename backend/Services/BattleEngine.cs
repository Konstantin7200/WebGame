using backend.Entities;
using System.Reflection.Metadata.Ecma335;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace backend.Services
{
    public class BattleEngine
    {
        public void fight(Unit attacker, Unit defender, Attack attackersAttack, Attack defendersAttack, Dictionary<(int, int), Unit> unitMap)
        {
            attacker.attacked = true;
            int aAttacksLeft = attackersAttack.AttacksAmount;
            int dAttacksLeft = defendersAttack.AttacksAmount;
            int result = 0;
            while (aAttacksLeft != 0 || dAttacksLeft != 0)
            {

                if (aAttacksLeft != 0)
                {
                    defender.takeDamage(attackersAttack.Damage * 1);
                    aAttacksLeft -= 1;
                    if (defender.isDead())
                    {
                        result = 1;
                        break;
                    }
                }
                if (dAttacksLeft != 0)
                {
                    attacker.takeDamage(defendersAttack.Damage * 1);
                    dAttacksLeft -= 1;
                    if (attacker.isDead())
                    {
                        result = -1;
                        break;
                    }

                    
                }
            }
            if (result == -1)
            {
                unitMap.Remove((attacker.X, attacker.Y));
            }
            else if (result == 1)
            {
                unitMap.Remove((defender.X, defender.Y));
            }
        }

        public int checkIfLeadersAreDead(Dictionary<(int, int), Unit> unitMap)
        {
            bool yourLeader = false;
            bool enemiesLeader= false;
            foreach(var unit in unitMap.Values)
            {
                if(unit is Leader)
                {
                    if(unit.Side==Unit.UnitSide.Yours)
                    {
                        yourLeader = true;
                    }
                    else
                    {
                        enemiesLeader = true;
                    }
                    if (yourLeader && enemiesLeader)
                    {
                        Console.WriteLine("Everyone is alive");
                        return 0;
                    }
                }
            }
            if (yourLeader)
            {
                Console.WriteLine("EnemyDied");
                return -1;
            }
            Console.WriteLine("YouDied");
            return 1; 
        }
    }
}
