using backend.Entities;
using System.Text.Json.Serialization;

namespace backend.Templates
{
    [JsonDerivedType(typeof(HealerTemplate), "healer")]
    [JsonDerivedType(typeof(UnitTemplate), "unit")]
    public class UnitTemplate
    {
        public int Health { get;  set; }
        public List<Attack> Attacks { get; }
        public string Type { get; }   
        public int MovesAmount { get; private set; }

        [JsonConstructor]
        public UnitTemplate(int health,List<Attack> attacks,int movesAmount,string type)
        {
            Health = health;
            Attacks = attacks;
            MovesAmount = movesAmount;
            Type = type;
        }
        public UnitTemplate(UnitTemplate other)
        {
            Health = other.Health;
            Attacks = other.Attacks;
            Type = other.Type;
            MovesAmount = other.MovesAmount;
        }
        
    }
}
