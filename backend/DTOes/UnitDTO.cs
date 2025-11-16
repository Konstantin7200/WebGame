using backend.Entities;

namespace backend.DTOes
{
    public class UnitDTO
    {
        public Unit BaseUnit { get; }
        public int X { get; set; }
        public int Y { get; set; }

        public string Name { get; }

         string[] names = new[] { "Valera","Maksim","SanFran" };

        public UnitDTO(Unit unit,int x,int y)
        {
            Name = names[Random.Shared.Next(0, names.Length - 1)];
            BaseUnit = unit;
            X = x;
            Y = y;
        }
    }
}
