using backend.DTOes;
using backend.Entities;
using backend.GameConfig;
using backend.Infrastructure;
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
        public AIResponse makeAITurn()
        {
            AI ai = new AI(_gameState.UnitMap);
            return new AIResponse(ai.start(_gameState.CurrentSide));
        }
        [HttpGet("isNextPlayerAI")]
        public bool isNextPlayerAI()
        {
            return _playerConfig.isAI(Unit.UnitSide.Yours == _gameState.CurrentSide);
        }
        [HttpPost("StartGame")]
        public void startGame([FromBody] PlayerTypes playerTypes,PlayerConfig playerConfig)
        {
            _gameState.startNewGame(playerTypes,playerConfig);
        }
        [HttpGet("LoadGame")]
        public GameDTO loadGame([FromQuery] int index)
        {
            _gameState.copy(GameRepository.loadGame(index).Item1);
            _playerConfig.copy(GameRepository.loadGame(index).Item2);
            return GameRepository.getGame(index);
        }
        [HttpPost("SaveGame")]
        public void saveGame()
        {
            GameRepository.saveGame(_gameState,_playerConfig);
        }
        [HttpGet("GetGames")]
        public List<GameDTO> getGames()
        {
            return GameRepository.Games; 
        }
        [HttpGet("GetTurn")]
        public int getTurn()
        {
            return _gameState.TurnNumber;
        }
    }
}
