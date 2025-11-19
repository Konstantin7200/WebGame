namespace backend.Entities
{
    public class Units
    {
        public List<LeaderTemplate> Leaders { get; set; }
        public List<UnitTemplate> OtherUnits { get; set; }

        public Units(List<UnitTemplate> otherUnits,List<LeaderTemplate> leaders)
        {
            OtherUnits = otherUnits;
            Leaders = leaders;
        }
    }
}
