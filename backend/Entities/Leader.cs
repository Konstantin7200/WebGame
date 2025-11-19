namespace backend.Entities
{
    public class LeaderTemplate:UnitTemplate
    {
        public LeaderTemplate(int health, List<Attack> attacks, int movesAmount,  string type) : base(health, attacks, movesAmount, type) 
        { 

        }
    }
}
