using backend.Entities;
using backend.Services;
using System.Text.Json.Serialization;

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
        [JsonConstructor]
        public PlayerConfig(PlayerType firstSide, PlayerType secondSide)
        {
            FirstSide = firstSide;
            SecondSide = secondSide;
        }
        public void copy(PlayerConfig other)
        {
            FirstSide = other.FirstSide;
            SecondSide = other.SecondSide;
        }

        public PlayerConfig(PlayerConfig other)
        {
            FirstSide = other.FirstSide;
            SecondSide = other.SecondSide;
        }
        public PlayerConfig()
        {

        }
        public bool isAI(bool turn)
        {
            
            if(turn)
            return FirstSide == PlayerType.AI;
            return SecondSide == PlayerType.AI;
        }
    }
}
