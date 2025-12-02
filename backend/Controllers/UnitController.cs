using backend.DTOes;
using backend.Entities;
using backend.GameConfig;
using backend.Infrastructure;
using backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitController : ControllerBase
    {
        GameState _gameState;

        public UnitController([FromServices] GameState gameState)
        {
            _gameState = gameState;
        }
        
        [HttpPost("Generation")]
        public void GetInitialUnits()
        {
            Console.WriteLine("___________________________________________");
            _gameState.CurrentTurn =new Turn();
            UnitGenerator unitGenerator = new(_gameState.UnitMap);
            unitGenerator.initialGeneration();
        }
        [HttpGet("GetUnits")]
        public List<Unit> GetUnits()
        {
            return _gameState.UnitMap.Values.ToList();
        }
        [HttpGet("GetUnit")]
        public Unit GetUnit([FromQuery] int x, [FromQuery] int y)
        {
            return _gameState.UnitMap[(x, y)];
        }

        [HttpGet("GetLastUnit")]
        public Unit GetLastUnit()
        {
            return _gameState.LastUnit;
        }


        [HttpGet("GetHexesForUnit")]
        public MovesDTO GetReachableHexes([FromQuery] int x, [FromQuery] int y)
        {
            if (!_gameState.UnitMap.ContainsKey((x, y)))
                return new MovesDTO(new(), new());

            if (_gameState.CurrentTurn.currentTurn != _gameState.UnitMap[(x, y)].Side || _gameState.UnitMap[(x, y)].attacked)
            {
                return new MovesDTO(new List<HexDTO>(), new List<EnemiesHex>());
            }

            MoveEngine pathFinder = new MoveEngine();

            _gameState.LastUnit = _gameState.UnitMap[(x, y)];

            MovesDTO result = pathFinder.getAllMoves(_gameState.UnitMap, _gameState.UnitMap[(x, y)]);
            return result;
        }

        [HttpPatch("MoveUnitTo")]
        public void MoveUnitTo([FromQuery] int x, [FromQuery] int y, [FromQuery] int movesToReach)
        {
            MoveEngine pathFinder = new MoveEngine();
            pathFinder.moveToHex(_gameState.LastUnit, _gameState.UnitMap, x, y, movesToReach);
        }

        public class FightRequest
        {
            public Attack attack1 { get; set; }
            public Attack attack2 { get; set; }
            public int x1 { get; set; }
            public int y1 { get; set; }
            public int x2 { get; set; }
            public int y2 { get; set; }
        }

        [HttpPost("Fight")]
        public void Fight([FromBody] FightRequest data)
        {
            BattleEngine battleEngine = new BattleEngine();
            battleEngine.fight(_gameState.UnitMap[(data.x1, data.y1)], _gameState.UnitMap[(data.x2, data.y2)], data.attack1, data.attack2, _gameState.UnitMap);
        }

        [HttpGet("IsLeaderDead")]
        public int checkIfLeaderIsDead()
        {
            BattleEngine battleEngine = new BattleEngine();
            return battleEngine.checkIfLeadersAreDead(_gameState.UnitMap);
        }

        
    }
}