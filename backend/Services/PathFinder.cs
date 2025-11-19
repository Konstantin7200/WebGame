using backend.DTOes;
using Microsoft.AspNetCore.Mvc;

namespace backend.Services
{
    public class PathFinder
    {
        int[,] movesEven = new int[,] { { 0, -1 }, { 1, -1 }, { -1, 0 }, { 1, 0 }, { 0, 1 }, { 1, 1 } };
        int[,] movesOdd = new int[,] { { -1, -1 }, { 0, -1 }, { -1, 0 }, { 1, 0 }, { -1, 1 }, { 0, 1 } };
        public const int MapSize = 7;
        bool checkIfHexExists(int X,int Y)
        {
            if(X>=0&&X<MapSize&&Y>=0&&Y<MapSize)
            {
                return true;
            }
            return false;
        }
        public List<HexDTO> findAvalibleHexes(Dictionary<(int,int),Unit> UnitMap, Unit pickedUnit,Unit lastUnit)
        {
            lastUnit.copy(pickedUnit);
            int[,] hexMas = new int[MapSize, MapSize];
            (int X, int Y) hex = new();
            foreach (var unit in UnitMap)
            {
                hex = (unit.Value.X,unit.Value.Y);
                if (unit.Value.Side !=pickedUnit.Side)
                {
                    int[,] moves = hex.Y % 2 == 0 ? movesEven : movesOdd;
                    for(int i = 0; i < moves.GetLength(0); i++)
                    {
                        hexMas[hex.X , hex.Y] = -1;
                        if (checkIfHexExists(hex.X + moves[i, 0], hex.Y + moves[i,1]))
                            hexMas[hex.X + moves[i,0], hex.Y + moves[i,1]] = -1;
                    }
                }
            }
            int unitMoves = pickedUnit.MovesLeft;
            hex = (pickedUnit.X, pickedUnit.Y);
            Queue<(int, int)> queue = new();
            queue.Enqueue(hex);
            
            int[,] movesToReachMas = new int[MapSize, MapSize];
            movesToReachMas[hex.X, hex.Y] = 0;
            hexMas[hex.X, hex.Y] = 0;
            while (queue.Count != 0)
            {
                hex = queue.Dequeue();
                if (hexMas[hex.X, hex.Y] == -1 || movesToReachMas[hex.X, hex.Y] == unitMoves)
                {
                    movesToReachMas[hex.X , hex.Y ] =unitMoves;
                    continue;
                }
                int[,] moves = hex.Y % 2 == 0 ? movesEven : movesOdd;
                for (int i=0; i<moves.GetLength(0); i++)
                {
                    if (checkIfHexExists(hex.X + moves[i, 0], hex.Y + moves[i, 1]))
                    {
                        if (movesToReachMas[hex.X + moves[i, 0], hex.Y + moves[i, 1]] == 0)
                        {
                            movesToReachMas[hex.X + moves[i, 0], hex.Y + moves[i, 1]] = movesToReachMas[hex.X, hex.Y] + 1;
                            queue.Enqueue((hex.X + moves[i, 0], hex.Y + moves[i, 1]));
                        }
                    }
                }
            }

            foreach(var unit in UnitMap.Values)
            {
                movesToReachMas[unit.X, unit.Y] = 0;
            }

            List<HexDTO> result = new();

            for(int i=0;i< MapSize; i++)
                for(int j=0;j<MapSize;j++)
                {
                    if (movesToReachMas[i, j] != 0)
                    {
                        result.Add(new HexDTO(i, j, movesToReachMas[i, j]));
                    }
                }

            return result;
        }
        public void moveToHex(Unit lastUnit,Dictionary<(int, int), Unit> UnitMap,int x,int  y,int movesToReach)
        {
            Unit movedUnit = new Unit();
            lastUnit.MovesLeft -= movesToReach;
            movedUnit.copy(lastUnit);
            movedUnit.X = x;
            movedUnit.Y = y;
            foreach (var item in UnitMap)
            {
                if (item.Key == (lastUnit.X, lastUnit.Y))
                {
                    UnitMap.Remove(item.Key);
                    UnitMap.Add((x, y), movedUnit);
                    lastUnit.X = x;
                    lastUnit.Y = y;
                    return;
                }
            }
        }
    }
}
