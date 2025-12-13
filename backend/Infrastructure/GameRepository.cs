using backend.DTOes;
using backend.GameConfig;

namespace backend.Infrastructure
{
    static public class GameRepository
    {
        public static List<GameDTO> Games{ get;private set; }
        static public void saveGame(GameState gameState,PlayerConfig playerConfig){
            Games.Add(new GameDTO(gameState, playerConfig,DateTime.Now));
            Console.WriteLine(Games.Count);
            MyJsonSerializer.rewriteFile(Games, MyJsonSerializer.GAMES_PATH);
        }
        static public void loadGamesFromFile()
        {
            Games = MyJsonSerializer.readFromJson<List<GameDTO>>(MyJsonSerializer.GAMES_PATH);
        }
        static public (GameState,PlayerConfig) loadGame(int index)
        {
            return (new GameState(Games[index].Units, Games[index].CurrentTurn), new PlayerConfig(Games[index].PlayerConfig));
        }
        static public GameDTO getGame(int index)
        {
            return Games[index];
        }

    }
}
