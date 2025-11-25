namespace backend.DTOes
{
    public class MovesDTO
    {
        public List<HexDTO> hexes { get; }
        public List<EnemiesHex> enemiesHexes { get; }

        public MovesDTO(List<HexDTO> hexes, List<EnemiesHex> enemiesHexes)
        {
            this.hexes = hexes;
            this.enemiesHexes = enemiesHexes;
        }
        public MovesDTO() { }
    }
}
