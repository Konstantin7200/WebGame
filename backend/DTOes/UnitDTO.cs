using backend.Entities;

namespace backend.DTOes
{
    public class UnitDTO
    {
        public Unit BaseUnit { get; private set; }
        public int X { get; set; }
        public int Y { get; set; }

        public string Name { get; private set; }

         string[] names = new[] { "Valera","Maksim","SanFran" };

        public UnitDTO(Unit unit,int x,int y)
        {
            Name = names[Random.Shared.Next(0, names.Length - 1)];
            BaseUnit = unit;
            X = x;
            Y = y;
        }
        public UnitDTO()
        {

        }
        public void copy(UnitDTO otherUnit)
        {
            X = otherUnit.X;
            Y = otherUnit.Y;
            BaseUnit = otherUnit.BaseUnit;
            Name = otherUnit.Name;
        }
    }
}
