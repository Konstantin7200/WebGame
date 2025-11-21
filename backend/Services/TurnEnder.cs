using backend.Entities;

namespace backend.Services
{
    public class TurnEnder
    {
        public void endTurn(Dictionary<(int,int),Unit> unitMap,Turn currentTurn)
        {
            currentTurn.switchTurn();
            foreach(var unit in unitMap.Values)
            {
                unit.resetMoves();
            }
        }
    }
}
