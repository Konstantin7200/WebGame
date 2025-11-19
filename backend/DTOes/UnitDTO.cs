using backend.Entities;

namespace backend.DTOes
{
    public class Unit
    {
        public UnitTemplate BaseUnit { get; private set; }
        public enum UnitSide : byte
        {
            Yours,
            Enemies
        }
        public UnitSide Side { get; private set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int MovesLeft { get; set; }
        public string Name { get; private set; }

        string[] names = new[] { "Valera","Maksim","SanFran" };

        public Unit(UnitTemplate unit,int x,int y,UnitSide side)
        {
            Side = side;
            Name = names[Random.Shared.Next(0, names.Length - 1)];
            BaseUnit = unit;
            X = x;
            Y = y;
            MovesLeft = BaseUnit.MovesAmount;
        }
        public Unit()
        {

        }
        public void copy(Unit otherUnit)
        {
            X = otherUnit.X;
            Y = otherUnit.Y;
            BaseUnit = otherUnit.BaseUnit;
            Name = otherUnit.Name;
            MovesLeft = otherUnit.MovesLeft;
            Side = otherUnit.Side;
        }
        public void resetMoves()
        {
            MovesLeft = BaseUnit.MovesAmount;
        }
    }
}
