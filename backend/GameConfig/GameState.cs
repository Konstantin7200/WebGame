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
        public Unit.UnitSide CurrentTurn { get; private set; }
        public GameState(Dictionary<(int,int),Unit> unitMap, Unit lastUnit, Unit.UnitSide currentTurn)
        {
            UnitMap = unitMap;
            LastUnit = lastUnit;
            CurrentTurn = currentTurn;
        }
        public void endTurn()
        {
            CurrentTurn=CurrentTurn == Unit.UnitSide.Yours ? Unit.UnitSide.Enemies : Unit.UnitSide.Yours;
            startNewTurn();
        }
        public void startNewTurn()
        {
            foreach (var unit in UnitMap.Values)
            {
                if (unit.Side == CurrentTurn)
                {
                    unit.OnTurnStart();
                }
            }
        }
        public void startNewGame(PlayerTypes playerTypes,PlayerConfig _playerConfig)
        {
            _playerConfig.createNewGame(playerTypes.PlayerType1, playerTypes.PlayerType2);
            CurrentTurn = Unit.UnitSide.Enemies;
            UnitGenerator unitGenerator = new(UnitMap);
            unitGenerator.initialGeneration();
        }
    }
}
