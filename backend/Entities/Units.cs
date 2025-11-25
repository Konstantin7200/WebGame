using backend.Templates;

namespace backend.Entities
{
    public class Units
    {
        public List<UnitTemplate> Leaders { get; set; }
        public List<UnitTemplate> OtherUnits { get; set; }

        public Units(List<UnitTemplate> otherUnits,List<UnitTemplate> leaders)
        {
            OtherUnits = otherUnits;
            Leaders = leaders;
        }
    }
}
