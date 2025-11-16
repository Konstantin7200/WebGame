using System.Collections.Generic;

namespace backend.Entities
{
    public class Healer:Unit
    {
        public int HealingAmount { get; private set; }
        public Healer(int health, List<Attack> attacks, int movesAmount, UnitSide side,int healingAmount,string type):base(health,attacks, movesAmount,  side,type)
        {
            HealingAmount = healingAmount;
        }
    }
}
