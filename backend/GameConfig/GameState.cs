using backend.DTOes;
using backend.Entities;
using backend.Services;
using Microsoft.AspNetCore.Mvc;
using static backend.Controllers.GameController;

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
        public void startNewGame(PlayerTypes playerTypes, [FromServices] PlayerConfig _playerConfig)
        {
            _playerConfig.createNewGame(playerTypes.PlayerType1, playerTypes.PlayerType2);
            CurrentTurn = new Turn();
            UnitGenerator unitGenerator = new(UnitMap);
            unitGenerator.initialGeneration();
        }
    }
}
