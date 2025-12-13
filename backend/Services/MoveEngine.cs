using backend.DTOes;
using backend.Entities;
using backend.GameConfig;
using Microsoft.AspNetCore.Mvc;

namespace backend.Services
{
    public class MoveEngine
    {
        public const int MAPSIZE = 7;

        public MovesDTO getAllMoves(Dictionary<(int, int), Unit> UnitMap, Unit pickedUnit,Unit.UnitSide turn)
        {
            if (turn != pickedUnit.Side || pickedUnit.Attacked)
            {
                return new MovesDTO(new List<Hex>(), new List<EnemiesHex>());
            }
            int[,] hexMas = new int[MAPSIZE, MAPSIZE];
            List<Hex> hexes = PathFinder.findFreeHexes(UnitMap, pickedUnit, hexMas);
            List<EnemiesHex> enemiesHexes = PathFinder.findEnemiesHexes(hexes, pickedUnit, hexMas);
            return new MovesDTO(hexes, enemiesHexes);
        }
        public void moveToHex(Unit lastUnit, Dictionary<(int, int), Unit> UnitMap, int x, int y, int movesToReach)
        {
            lastUnit.MovesLeft -= movesToReach;
            UnitMap.Remove((lastUnit.X, lastUnit.Y));
            lastUnit.X = x;
            lastUnit.Y = y;
            UnitMap.Add((x, y), lastUnit);
        }
        public void moveToHex(Unit lastUnit, Dictionary<(int, int), Unit> unitMap, Hex hex)
        {
            moveToHex(lastUnit, unitMap, hex.X, hex.Y, hex.Moves);
        }
        public bool moveToClosestEnemy(Unit myUnit,Dictionary<(int,int),Unit> unitMap)
        {
            int[,] movesToReachMas = new int[MAPSIZE,MAPSIZE]; 
            Unit closestEnemy = PathFinder.findClosestEnemy(myUnit, unitMap,movesToReachMas);
            List<Hex> path = PathFinder.findPathToClosestEnemy(myUnit, unitMap,closestEnemy,movesToReachMas);
            foreach (Hex hex in path)
            {
                if(hex.Moves==myUnit.MovesLeft)
                {
                    moveToHex(myUnit, unitMap, hex);
                    return true;
                }
            }
            return false;
        }
        
    }
}
