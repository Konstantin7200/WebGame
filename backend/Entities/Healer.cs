using backend.Services;
using backend.Templates;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace backend.Entities
{
    public class Healer:Unit
    {
        [JsonIgnore]
        public Dictionary<(int, int), Unit> UnitMap { get; }
        public HealerTemplate BaseUnit { get; protected set; }
        public Healer(HealerTemplate unit, int x, int y, UnitSide side, Dictionary<(int, int), Unit> unitMap) :base(unit,x,y,side)
        {
            UnitMap = unitMap;
            BaseUnit = unit;
        }
        public Healer()
        {

        }
        public override void OnTurnStart()
        {
            base.OnTurnStart();
            int[,]neighbours=PathFinder.getNeighbourHexes(Y);
            int neighbourHexesAmount = 6;
            for(int i=0;i<neighbourHexesAmount;i++)
            {
                if (UnitMap.ContainsKey((X + neighbours[i, 0], Y + neighbours[i, 1])))
                    if (UnitMap[(X + neighbours[i, 0], Y + neighbours[i, 1])].Side == Side)
                    {
                        UnitMap[(X + neighbours[i, 0], Y + neighbours[i, 1])].getHealed(BaseUnit.HealingPower);
                    }
            }
        }
    }
}
