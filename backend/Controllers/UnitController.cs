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
        [HttpGet]
        public List<UnitDTO> GetInitialUnits([FromServices] UnitGenerator unitGenerator)
        {
            return unitGenerator.initialGeneration();
        }
    }
}
