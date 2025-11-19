using backend.DTOes;
using backend.Entities;
using backend.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using static backend.DTOes.Unit;
namespace backend.Services
{
    public class UnitGenerator
    {
        Dictionary<(int, int), Unit> unitMap;
        public UnitGenerator(Dictionary<(int, int),Unit> UnitMap)
        {
            unitMap = UnitMap;
        }
        public List<Unit> initialGeneration()
        {
            var random = new Random();
            const int UNITS_NUMBER= 3;
            Units units=MyJsonSerializer.readFromJson<Units>(MyJsonSerializer.UNITS_PATH);
            HealerTemplate healer = (HealerTemplate)units.OtherUnits[1];
            List<Unit> generatedUnits = new();
            //Leader leader = units.Leaders[random.Next(0, units.Leaders.Count-1)];generatedUnits.Add(leader);
            for(int i=0;i<UNITS_NUMBER;i++)
            {
                (int X, int Y) unitHex = (1, i);
                UnitTemplate pickedUnit = units.OtherUnits[random.Next(0, units.OtherUnits.Count - 1)];
                Unit unitDTO = new Unit(unit:pickedUnit,x: unitHex.X,y: unitHex.Y, UnitSide.Yours);
                generatedUnits.Add(unitDTO);
                unitMap.Add((unitHex.X, unitHex.Y),unitDTO);
            }
            for(int i=0;i<UNITS_NUMBER;i++)
            {
                (int X, int Y) unitHex = (5, i);
                UnitTemplate pickedUnit = units.OtherUnits[random.Next(0, units.OtherUnits.Count - 1)];
                Unit unitDTO = new Unit(unit: pickedUnit, x: unitHex.X, y: unitHex.Y, UnitSide.Enemies);
                generatedUnits.Add(unitDTO);
                unitMap.Add((unitHex.X, unitHex.Y), unitDTO);
            }

            return generatedUnits;
        } 
    }
}
