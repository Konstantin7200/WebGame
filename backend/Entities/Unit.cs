using backend.Templates;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace backend.Entities
{
    public class Unit
    {
        public UnitTemplate BaseUnit { get; protected set; }
        public enum UnitSide : byte
        {
            Yours,
            Enemies
        }
        public UnitSide Side { get; protected set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int MovesLeft { get; set; }
        public string Name { get; protected set; }
        public bool Attacked { get; set; }
        public int Health { get; protected set; }

        string[] names = new[] { "Valera","Maksim","SanFran" };
        [JsonConstructor]
        public Unit(UnitTemplate baseUnit, int x, int y, UnitSide side, int health, int movesLeft,bool attacked,string name)
        {
            BaseUnit = baseUnit;
            X = x;
            Y = y;
            Side = side;
            Health = health;
            MovesLeft = movesLeft;
            Attacked = attacked;
            Name = name;
        }
        public Unit(UnitTemplate unit,int x,int y,UnitSide side)
        {
            Side = side;
            Name = names[Random.Shared.Next(0, names.Length)];
            BaseUnit = unit;
            X = x;
            Y = y;
            MovesLeft = BaseUnit.MovesAmount;
            Attacked = false;
            Health = BaseUnit.Health;
        }
        
        public Unit()
        {

        }
        public virtual void OnTurnStart()
        {
            MovesLeft = BaseUnit.MovesAmount;
            Attacked = false;
        }
        public void takeDamage(double damage)
        {
            Health -=(int)damage;
        }
        public void getHealed(int healingAmount)
        {
            Health = Math.Min(Health + healingAmount, BaseUnit.Health);
        }
        public bool isDead()
        {
            return Health <= 0;
        }
    }
}
