using backend.Entities;
using backend.GameConfig;
using backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        GameState _gameState;
        PlayerConfig _playerConfig;
        public GameController([FromServices] GameState gameState, PlayerConfig playerConfig)
        {
            _gameState = gameState;
            _playerConfig = playerConfig;
        }
        [HttpPatch("EndTurn")]
        public void EndTurn()
        {
            Console.WriteLine("TurnEnded");
            TurnEnder turnEnder = new TurnEnder();
            turnEnder.endTurn(_gameState.UnitMap, _gameState.CurrentTurn);
        }
        [HttpGet("GetTurn")]
        public Turn GetTurn()
        {
            return _gameState.CurrentTurn;
        }
        [HttpPost("AITurn")]
        public bool makeAITurn()
        {
            AI ai = new AI(_gameState.UnitMap);
            return ai.start(_gameState.CurrentTurn);
        }
        [HttpGet("GetNextTurn")]
        public bool getNextTurn()
        {
            bool res = _playerConfig.isAI(Unit.UnitSide.Yours == _gameState.CurrentTurn.currentTurn);
            return res;
        }
        public class Sides
        {
            public bool Side1 { get; set; }
            public bool Side2 { get; set; }
            public Sides() { }
        }
        [HttpPost("CreateConfig")]
        public void createNewConfig([FromBody] Sides sides)
        {
            _playerConfig.createNewGame(sides.Side1, sides.Side2);
        }
    }
}
