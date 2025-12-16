namespace backend.DTOes
{
    public class AIResponse
    {
        public bool HasTurnHappened { get; set; }
        public bool HasFighted { get; set; }
        public AIResponse((bool,bool) item)
        {
            HasTurnHappened = item.Item1;
            HasFighted = item.Item2;
        }
    }
}
