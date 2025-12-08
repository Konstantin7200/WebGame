namespace backend.Entities
{
    public class Hex
    {
        public int X { get; }
        public int Y { get; }
        public int Moves { get; }

        public Hex(int x,int y,int moves)
        {
            Moves = moves;
            X = x;
            Y = y;
        }
    }
}
