using backend.Templates;

namespace backend.Entities
{
    public class Leader:Unit
    {
        public Leader(UnitTemplate unit, int x, int y, UnitSide side) : base(unit, x, y, side) { }
        public Leader() { }
        
    }
}
