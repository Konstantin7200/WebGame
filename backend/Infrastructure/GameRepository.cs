using backend.GameConfig;

namespace backend.Infrastructure
{
    static public class GameRepository
    {
        public static List<GameState> Games { get;private set; }
        static public void saveGame(GameState gameState){
            Games.Add(gameState);
            MyJsonSerializer.writeToJson(Games, MyJsonSerializer.GAMES_PATH);
        }
        static public void loadGames()
        {
            Games = MyJsonSerializer.readFromJson<List<GameState>>(MyJsonSerializer.GAMES_PATH);
        }
        static public GameState loadGame(int index)
        {
            return Games[index];
        }
    }
}
