namespace backend.DTOes
{
    public class EnemiesHex
    {
        public HexDTO Previous { get; }
        public int X { get; }
        public int Y { get; }

        public EnemiesHex(int x,int y,HexDTO previous)
        {
            X = x;
            Y = y;
            Previous = previous;
        }
    }
}
