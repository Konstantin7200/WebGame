using backend.Entities;

namespace backend.DTOes
{
    public class FightRequestDTO
    {
        public Attack Attack1 { get; set; }
        public Attack Attack2 { get; set; }
        public int X1 { get; set; }
        public int Y1 { get; set; }
        public int X2 { get; set; }
        public int Y2 { get; set; }
    }

}
