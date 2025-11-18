using backend.DTOes;
using backend.Entities;
using backend.Infrastructure;
using Microsoft.AspNetCore.Mvc;
namespace backend.Services
{
    public class UnitGenerator
    {
        Dictionary<(int, int), UnitDTO> unitMap;
        public UnitGenerator(Dictionary<(int, int),UnitDTO> UnitMap)
        {
            unitMap = UnitMap;
        }
        public List<UnitDTO> initialGeneration()
        {
            var random = new Random();
            const int UNITS_NUMBER= 3;
            Units units=MyJsonSerializer.readFromJson<Units>(MyJsonSerializer.UNITS_PATH);
            Healer healer = (Healer)units.OtherUnits[1];
            List<UnitDTO> generatedUnits = new();
            //Leader leader = units.Leaders[random.Next(0, units.Leaders.Count-1)];generatedUnits.Add(leader);
            for(int i=0;i<UNITS_NUMBER;i++)
            {
                (int X, int Y) unitHex = (1, i);
                Unit pickedUnit = units.OtherUnits[random.Next(0, units.OtherUnits.Count - 1)];
                UnitDTO unitDTO = new UnitDTO(unit:pickedUnit,x: unitHex.X,y: unitHex.Y);
                generatedUnits.Add(unitDTO);
                unitMap.Add((unitHex.X, unitHex.Y),unitDTO);
            }

            return generatedUnits;
        } 
    }
}
