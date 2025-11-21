using backend.DTOes;
using backend.Entities;
using backend.Infrastructure;
using backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitController : ControllerBase
    {
        UnitGenerator _unitGenerator;
        Dictionary<(int, int), Unit> _unitMap;
        Unit _lastUnit;
        Turn _currentTurn;
        public UnitController([FromServices] UnitGenerator unitGenerator,[FromServices] Dictionary<(int, int), Unit> UnitMap, [FromServices] Unit lastUnit, [FromServices] Turn currentTurn)
        {
            _unitGenerator = unitGenerator;
            _unitMap = UnitMap;
            _lastUnit = lastUnit;
            _currentTurn =currentTurn;
        }

        [HttpGet("Generation")]
        public List<Unit> GetInitialUnits()
        {
            return _unitGenerator.initialGeneration();
        }
        [HttpGet("GetUnits")]
        public List<Unit> GetUnits()
        {
            return _unitMap.Values.ToList();
        }
        [HttpPatch("MoveUnitTo")]
        public void MoveUnitTo([FromQuery] int x, [FromQuery] int y, [FromQuery] int movesToReach)
        {
            PathFinder pathFinder = new PathFinder();
            pathFinder.moveToHex(_lastUnit,_unitMap,x,y,movesToReach);
        }
        [HttpPatch("EndTurn")]
        public void EndTurn()
        {
            TurnEnder turnEnder = new TurnEnder();
            turnEnder.endTurn(_unitMap,_currentTurn);
        }
        [HttpGet("GetTurn")]
        public Turn GetTurn()
        {
            return _currentTurn;
        }
        [HttpGet("GetHexesForUnit")]
        public MovesDTO GetReachableHexes([FromQuery] int x, [FromQuery] int y)
        {
            if (_currentTurn.currentTurn != _unitMap[(x, y)].Side || _unitMap[(x,y)].attacked)
            {
                return new MovesDTO(new List<HexDTO>(),new List<EnemiesHex>());
            }
            PathFinder pathFinder = new PathFinder();
            return pathFinder.getAllMoves(_unitMap, _unitMap[(x, y)],_lastUnit);
        }
        [HttpGet("GetUnit")]
        public Unit GetUnit([FromQuery] int x, [FromQuery] int y)
        {
            return _unitMap[(x, y)];
        }
        [HttpGet("GetLastUnit")]
        public Unit GetLastUnit()
        {
            return _lastUnit;
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
            int result=battleEngine.fight(_unitMap[(data.x1, data.y1)], _unitMap[(data.x2, data.y2)],data.attack1, data.attack2);
            if(result==-1)
            {
                _unitMap.Remove((data.x1, data.y1));
            }
            else if (result == 1)
            {
                _unitMap.Remove((data.x2, data.y2));
            }

            foreach(var unit in _unitMap.Values)
            {
                Console.WriteLine(unit.X + " " + unit.Y);
            }
        }
    }
}
