using backend.Entities;
using backend.GameConfig;
using System.Text.Json.Serialization;

namespace backend.DTOes
{
    public class GameDTO
    {
        public List<Unit> Units { get; private set; }
        public int CurrentTurn { get; private set; }
        public PlayerConfig PlayerConfig { get;private set; }
        public DateTime DateOfCreation { get; private set; }
        public GameDTO(GameState gameState, PlayerConfig playerConfig, DateTime dateOfCreation)
        {
            Units = gameState.UnitMap.Values.ToList();
            CurrentTurn = gameState.TurnNumber;
            PlayerConfig = playerConfig;
            DateOfCreation = dateOfCreation;
        }
        [JsonConstructor]
        public GameDTO(List<Unit> units,int currentTurn,PlayerConfig playerConfig,DateTime dateOfCreation)
        {
            DateOfCreation = dateOfCreation;
            Units = units;
            CurrentTurn = currentTurn;
            PlayerConfig = playerConfig;
        }
    }
}
