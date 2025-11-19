using System.Collections.Generic;

namespace backend.Entities
{
    public class HealerTemplate:UnitTemplate
    {
        public int HealingAmount { get; private set; }
        public HealerTemplate(int health, List<Attack> attacks, int movesAmount,int healingAmount,string type):base(health,attacks, movesAmount, type)
        {
            HealingAmount = healingAmount;
        }
    }
}
