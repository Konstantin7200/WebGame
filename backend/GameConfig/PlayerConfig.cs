using backend.Entities;
using backend.Services;

namespace backend.GameConfig
{
    public class PlayerConfig
    {
        public enum PlayerType
        {
            Player,
            AI
        }
        public PlayerType FirstSide { get; private set; }
        public PlayerType SecondSide { get; private set; }
        public void createNewGame(bool side1,bool side2)
        {
            FirstSide = side1 ? PlayerType.Player : PlayerType.AI;
            SecondSide = side2 ? PlayerType.Player: PlayerType.AI;
        }
        public bool isAI(bool turn)
        {
            
            if(turn)
            return FirstSide == PlayerType.AI;
            return SecondSide == PlayerType.AI;
        }
    }
}
