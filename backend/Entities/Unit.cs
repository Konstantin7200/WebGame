using backend.Entities;
using System.Text.Json.Serialization;

namespace backend.Entities
{
    [JsonDerivedType(typeof(Healer), "healer")]
    [JsonDerivedType(typeof(Leader), "leader")]
    [JsonDerivedType(typeof(Unit), "unit")]
    public class Unit
    {
        public enum UnitSide:byte
        {
            Yours,
            Enemies
        }
        public int Health { get; private set; }
        public List<Attack> Attacks { get; }
        public string Type { get; }
        
        public UnitSide Side { get; }
        public int MovesAmount { get; private set; }
        

        public Unit(int health,List<Attack> attacks,int movesAmount,UnitSide side,string type)
        {
            Health = health;
            Attacks = attacks;
            Side = side;
            MovesAmount = movesAmount;
            Type = type;
        }
        
    }
}
