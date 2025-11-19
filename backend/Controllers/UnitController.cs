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
        [HttpGet("GetHexesForUnit")]
        public List<HexDTO> GetReachableHexes([FromQuery] int x, [FromQuery] int y)
        {
            if (_currentTurn.currentTurn != _unitMap[(x, y)].Side)
            {
                return new List<HexDTO>();
            }
            PathFinder pathFinder = new PathFinder();
            return pathFinder.findAvalibleHexes(_unitMap, _unitMap[(x, y)],_lastUnit);
        }
    }
}
