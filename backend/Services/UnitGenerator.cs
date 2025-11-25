using backend.Entities;
using backend.Infrastructure;
using backend.Templates;
using Microsoft.AspNetCore.Mvc;
using System;
using static backend.Entities.Unit;
namespace backend.Services
{
    public class UnitGenerator
    {
        Dictionary<(int, int), Unit> unitMap;
        const int UNITS_NUMBER = 3;
        public UnitGenerator(Dictionary<(int, int),Unit> UnitMap)
        {
            unitMap = UnitMap;
        }
        public void generateOneSide(Units units,int column)
        {
            UnitSide unitSide = column == 1 ? UnitSide.Yours : UnitSide.Enemies;
            int leadersY = column == 1 ? 0 : column + 1;
            (int X, int Y) unitHex = (leadersY, 3);
            UnitTemplate pickedUnitTemplate = units.Leaders[Random.Shared.Next(0, units.Leaders.Count)];
            Leader leader = new Leader(new UnitTemplate(pickedUnitTemplate), unitHex.X, unitHex.Y, unitSide);
            unitMap.Add((unitHex.X, unitHex.Y), leader);
            for (int i = 0; i < UNITS_NUMBER; i++)
            {
                unitHex = (column, i+2);
                pickedUnitTemplate = units.OtherUnits[Random.Shared.Next(0, units.OtherUnits.Count)];
                Unit unit = new Unit(unit: new UnitTemplate(pickedUnitTemplate), x: unitHex.X, y: unitHex.Y, unitSide);
                unitMap.Add((unitHex.X, unitHex.Y), unit);
            }
        }
        public void initialGeneration()
        {
            unitMap.Clear();
            var random = new Random();
            
            Units units=MyJsonSerializer.readFromJson<Units>(MyJsonSerializer.UNITS_PATH);

            generateOneSide(units, 1);
            generateOneSide(units, 5);
        } 
    }
}
