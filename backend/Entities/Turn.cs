namespace backend.Entities
{
    public class Turn
    {
        public Unit.UnitSide currentTurn { get; private set; }
        public Turn()
        {
            currentTurn = Unit.UnitSide.Enemies;
        }
        public void switchTurn()
        {
            currentTurn = currentTurn == Unit.UnitSide.Yours ? Unit.UnitSide.Enemies : Unit.UnitSide.Yours;
        }
    }
}
