using backend.DTOes;
using backend.Entities;
using backend.Services;
using Microsoft.AspNetCore.Mvc;
using static backend.Controllers.GameController;

namespace backend.GameConfig
{
    public class GameState
    {
        public Dictionary<(int, int), Unit> UnitMap { get; private set; }
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
        public GameState(List<Unit> units,int turnNumber)
        {
            CurrentSide = turnNumber % 2 == 0 ? Unit.UnitSide.Enemies : Unit.UnitSide.Yours;
            TurnNumber = turnNumber;
            Dictionary<(int, int), Unit> unitMap = new();
            foreach(Unit unit in units)
            {
                unitMap.Add((unit.X, unit.Y),unit);
            }
            UnitMap = unitMap;
            LastUnit = new();
        }
        public void copy(GameState other)
        {
            UnitMap = other.UnitMap;
            LastUnit = other.LastUnit;
            TurnNumber = other.TurnNumber;
            CurrentSide = other.CurrentSide;
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
            TurnNumber = 0;
            UnitGenerator unitGenerator = new(UnitMap);
            unitGenerator.initialGeneration();
        }
    }
}
