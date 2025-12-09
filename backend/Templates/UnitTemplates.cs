namespace backend.Templates
{
    public class UnitTemplates
    {
        public List<UnitTemplate> LeaderTemplates { get; set; }
        public List<UnitTemplate> OtherUnitsTemplates { get; set; }

        public UnitTemplates(List<UnitTemplate> otherUnitsTemplates,List<UnitTemplate> leaderTemplates)
        {
            OtherUnitsTemplates = otherUnitsTemplates;
            LeaderTemplates = leaderTemplates;
        }
    }
}
