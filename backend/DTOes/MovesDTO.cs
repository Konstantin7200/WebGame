using backend.Entities;

namespace backend.DTOes
{
    public class MovesDTO
    {
        public List<Hex> hexes { get; }
        public List<EnemiesHex> enemiesHexes { get; }

        public MovesDTO(List<Hex> hexes, List<EnemiesHex> enemiesHexes)
        {
            this.hexes = hexes;
            this.enemiesHexes = enemiesHexes;
        }
        public MovesDTO() { }
    }
}
