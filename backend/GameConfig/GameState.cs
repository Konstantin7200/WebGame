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
        public int TurnNumber{get;private set;}
        public Unit.UnitSide CurrentSide { get; private set; }
        public GameState(Dictionary<(int,int),Unit> unitMap, Unit lastUnit, Unit.UnitSide currentSide,int turnNumber)
        {
            UnitMap = unitMap;
            LastUnit = lastUnit;
            CurrentSide = currentSide;
            TurnNumber = turnNumber;
        }
        public void endTurn()
        {
            CurrentSide=CurrentSide == Unit.UnitSide.Yours ? Unit.UnitSide.Enemies : Unit.UnitSide.Yours;
            startNewTurn();
            TurnNumber++;
        }
        public void startNewTurn()
        {
            foreach (var unit in UnitMap.Values)
            {
                if (unit.Side == CurrentSide)
                {
                    unit.OnTurnStart();
                }
            }
        }
        public void startNewGame(PlayerTypes playerTypes,PlayerConfig _playerConfig)
        {
            _playerConfig.createNewGame(playerTypes.PlayerType1, playerTypes.PlayerType2);
            CurrentSide = Unit.UnitSide.Enemies;
            UnitGenerator unitGenerator = new(UnitMap);
            unitGenerator.initialGeneration();
        }
    }
}
