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
        Dictionary<(int, int), UnitDTO> _unitMap;
        UnitDTO _lastUnit;
        public UnitController([FromServices] UnitGenerator unitGenerator,[FromServices] Dictionary<(int, int), UnitDTO> UnitMap, [FromServices] UnitDTO lastUnit)
        {
            _unitGenerator = unitGenerator;
            _unitMap = UnitMap;
            _lastUnit = lastUnit;
        }

        [HttpGet("Generation")]
        public List<UnitDTO> GetInitialUnits()
        {
            return _unitGenerator.initialGeneration();
        }
        [HttpGet("GetUnits")]
        public List<UnitDTO> GetUnits()
        {
            foreach(var item in _unitMap)
            {
                Console.WriteLine(item.Value.X+" "+item.Value.Y+"|"+item.Key.Item1+" "+item.Key.Item2);
            }

            return _unitMap.Values.ToList();
        }
        [HttpPatch("MoveUnitTo")]
        public void MoveUnitTo([FromQuery] int x, [FromQuery] int y)
        {
            PathFinder pathFinder = new PathFinder();
            pathFinder.moveToHex(_lastUnit,_unitMap,x,y);
        }

        [HttpGet("GetHexesForUnit")]
        public List<HexDTO> GetReachableHexes([FromQuery] int x, [FromQuery] int y)
        {
            PathFinder pathFinder = new PathFinder();
            return pathFinder.findAvalibleHexes(_unitMap, _unitMap[(x, y)],_lastUnit);
        }
    }
}
