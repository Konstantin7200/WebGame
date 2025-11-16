namespace backend.Entities
{
    public class Units
    {
        public List<Leader> Leaders { get; set; }
        public List<Unit> OtherUnits { get; set; }

        public Units(List<Unit> otherUnits,List<Leader> leaders)
        {
            OtherUnits = otherUnits;
            Leaders = leaders;
        }
    }
}
