namespace backend.Entities
{
    public class Leader:Unit
    {
        public Leader(int health, List<Attack> attacks, int movesAmount, UnitSide side, string type) : base(health, attacks, movesAmount, side, type) 
        { 

        }
    }
}
