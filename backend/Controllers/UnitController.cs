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
                return new MovesDTO();
            MoveEngine moveEngine = new MoveEngine();
            _gameState.LastUnit = _gameState.UnitMap[(x, y)];
            return moveEngine.getAllMoves(_gameState.UnitMap, _gameState.UnitMap[(x, y)],_gameState.CurrentSide);
        }

        [HttpPatch("MoveUnitTo")]
        public void MoveUnitTo([FromQuery] int x, [FromQuery] int y, [FromQuery] int movesToReach)
        {
            MoveEngine moveEngine = new MoveEngine();
            moveEngine.moveToHex(_gameState.LastUnit, _gameState.UnitMap, x, y, movesToReach);
        }

        
        [HttpPost("Fight")]
        public void Fight([FromBody] FightRequestDTO data)
        {
            BattleEngine.fight(_gameState.UnitMap[(data.X1, data.Y1)], _gameState.UnitMap[(data.X2, data.Y2)], data.Attack1, data.Attack2, _gameState.UnitMap);
        }

        [HttpGet("IsLeaderDead")]
        public int checkIfLeaderIsDead()
        {
            return BattleEngine.checkIfLeadersAreDead(_gameState.UnitMap);
        }

        
    }
}