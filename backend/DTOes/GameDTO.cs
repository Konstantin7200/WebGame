using backend.Entities;
using backend.GameConfig;

namespace backend.DTOes
{
    public class GameDTO
    {
        public List<Unit> units { get; private set; }
        public int currentTurn { get; private set; }
        public GameDTO(GameState gameState)
        {
            units = gameState.UnitMap.Values.ToList();
            currentTurn = gameState.TurnNumber;
        }
        static public List<GameDTO> getGames(List<GameState> gameStates)
        {
            List <GameDTO> result= new();

            for(int i=0;i<gameStates.Count;i++)
            {
                result.Add(new GameDTO(gameStates[i]));
            }

            return result;
        }
    }
}
