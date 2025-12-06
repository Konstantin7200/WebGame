using backend.DTOes;
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
            _gameState.endTurn();
        }
        [HttpPost("AIMove")]
        public bool makeAITurn()
        {
            AI ai = new AI(_gameState.UnitMap);
            return ai.start(_gameState.CurrentTurn);
        }
        [HttpGet("isNextPlayerAI")]
        public bool isNextPlayerAI()
        {
            return _playerConfig.isAI(Unit.UnitSide.Yours == _gameState.CurrentTurn.currentTurn);
        }
        [HttpPost("StartGame")]
        public void startGame([FromBody] PlayerTypes playerTypes,PlayerConfig playerConfig)
        {
            _gameState.startNewGame(playerTypes,playerConfig);
        }
    }
}
