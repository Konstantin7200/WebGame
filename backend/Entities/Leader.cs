using backend.Templates;

namespace backend.Entities
{
    public class Leader:Unit
    {
        public Leader(UnitTemplate unit, int x, int y, UnitSide side) : base(unit, x, y, side) { }
        public Leader() { }
        public override void copyFrom(Unit other)
        {
            base.copyFrom(other);
        }
        public override Unit createNew()
        {
            return new Leader();
        }
    }
}
