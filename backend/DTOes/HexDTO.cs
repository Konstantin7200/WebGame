namespace backend.DTOes
{
    public class HexDTO
    {
        public int X { get; }
        public int Y { get; }
        public int Moves { get; }

        public HexDTO(int x,int y,int moves)
        {
            Moves = moves;
            X = x;
            Y = y;
        }
    }
}
