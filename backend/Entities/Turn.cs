using backend.DTOes;

namespace backend.Entities
{
    public class Turn
    {
        public Unit.UnitSide currentTurn { get; private set; }
        public Turn()
        {
            currentTurn = Unit.UnitSide.Yours;
        }
        public void switchTurn()
        {
            currentTurn = currentTurn == Unit.UnitSide.Yours ? Unit.UnitSide.Enemies : Unit.UnitSide.Yours;
        }
    }
}
