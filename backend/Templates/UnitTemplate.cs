using backend.Entities;
using System.Text.Json.Serialization;

namespace backend.Templates
{
    [JsonDerivedType(typeof(UnitTemplate), "unit")]
    public class UnitTemplate
    {
        public int Health { get;  set; }
        public List<Attack> Attacks { get; }
        public string Type { get; }   
        public int MovesAmount { get; private set; }
        public Dictionary<Attack.DamageTypes, double> Resistances { get; private set; }
        [JsonConstructor]
        public UnitTemplate(int health,List<Attack> attacks,int movesAmount,string type,Dictionary<Attack.DamageTypes,double> resistances)
        {
            Health = health;
            Attacks = attacks;
            MovesAmount = movesAmount;
            Type = type;
            Resistances = resistances;
        }
        public UnitTemplate(UnitTemplate other)
        {
            Health = other.Health;
            Attacks = other.Attacks;
            Type = other.Type;
            MovesAmount = other.MovesAmount;
            Resistances = other.Resistances;
        }
        
    }
}
