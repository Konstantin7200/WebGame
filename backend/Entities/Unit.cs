using backend.Templates;
using Microsoft.AspNetCore.Mvc;

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
        public bool attacked;
        public int Health { get; protected set; }

        string[] names = new[] { "Valera","Maksim","SanFran" };

        public Unit(UnitTemplate unit,int x,int y,UnitSide side)
        {
            Side = side;
            Name = names[Random.Shared.Next(0, names.Length - 1)];
            BaseUnit = unit;
            X = x;
            Y = y;
            MovesLeft = BaseUnit.MovesAmount;
            attacked = false;
            Health = BaseUnit.Health;
        }
        public Unit()
        {

        }
        public void resetMoves()
        {
            MovesLeft = BaseUnit.MovesAmount;
            attacked = false;
        }
        public void takeDamage(double damage)
        {
            Health -=(int)damage;
        }
        public bool isDead()
        {
            return Health <= 0;
        }
    }
}
