using backend.Entities;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace backend.Templates
{
    public class HealerTemplate:UnitTemplate
    {
        public int HealingPower { get;private set; }
        [JsonConstructor]
        public HealerTemplate(int health, List<Attack> attacks, int movesAmount, string type, Dictionary<Attack.DamageTypes, double> resistances,int healingPower):base(health, attacks,  movesAmount, type, resistances)
        {
            HealingPower = healingPower;
        }
        public HealerTemplate(HealerTemplate other):base(other)
        {
            HealingPower = other.HealingPower;
        }

     
    }
}
