namespace backend.Entities
{
    public class EnemiesHex
    {
        public Hex Previous { get; }
        public int X { get; }
        public int Y { get; }

        public EnemiesHex(int x,int y,Hex previous)
        {
            X = x;
            Y = y;
            Previous = previous;
        }
    }
}
