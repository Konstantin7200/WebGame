using backend.Entities;

namespace backend.GameConfig
{
    public class GameState
    {
        public Dictionary<(int, int), Unit> UnitMap { get; }
        public Unit LastUnit { get; set; }
        public Turn CurrentTurn { get; set; }
        public GameState(Dictionary<(int,int),Unit> unitMap, Unit lastUnit, Turn currentTurn)
        {
            UnitMap = unitMap;
            LastUnit = lastUnit;
            CurrentTurn = currentTurn;
        }
        public void endTurn()
        {
            CurrentTurn.switchTurn();
            startNewTurn();
        }
        public void startNewTurn()
        {
            foreach (var unit in UnitMap.Values)
            {
                if (unit.Side == CurrentTurn.currentTurn)
                    unit.resetMoves();
            }
        }
    }
}
