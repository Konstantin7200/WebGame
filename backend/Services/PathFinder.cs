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
            if(X>=0&&X<=MapSize&&Y>=0&&Y<=MapSize)
            {
                return true;
            }
            return false;
        }
        public List<HexDTO> findAvalibleHexes(Dictionary<(int,int),UnitDTO> UnitMap, UnitDTO pickedUnit,UnitDTO lastUnit)
        {
            lastUnit.copy(pickedUnit);
            int[,] hexMas = new int[MapSize + 1, MapSize + 1];
            (int X, int Y) hex = new();
            foreach (var unit in UnitMap)
            {
                hex = (unit.Value.X,unit.Value.Y);
                if (unit.Value.BaseUnit.Side == Entities.Unit.UnitSide.Yours)
                {
                    hexMas[hex.X, hex.Y] = 1;
                }
                else {
                    hexMas[hex.X, hex.Y] = -1;
                    int[,] moves = hex.Y % 2 == 0 ? movesEven : movesOdd;
                    for(int i = 0; i < moves.GetLength(0); i++)
                    {
                        if (checkIfHexExists(hex.X + moves[i, 0], hex.Y + moves[i,1]))
                            hexMas[hex.X + moves[i,0], hex.Y + moves[i,1]] = -1;
                    }
                }
            }
            int unitMoves = pickedUnit.BaseUnit.MovesAmount;
            hex = (pickedUnit.X, pickedUnit.Y);
            Queue<(int, int)> queue = new();
            queue.Enqueue(hex);
            
            int[,] movesToReachMas = new int[MapSize + 1, MapSize + 1];
            movesToReachMas[hex.X, hex.Y] = 0;
            while (queue.Count != 0)
            {
                hex = queue.Dequeue();
                if (hexMas[hex.X, hex.Y] == -1 || movesToReachMas[hex.X,hex.Y]==unitMoves)
                    continue;
                int[,] moves = hex.Y % 2 == 0 ? movesEven : movesOdd;
                for (int i=0; i<moves.GetLength(0); i++)
                {
                    if (checkIfHexExists(hex.X + moves[i, 0], hex.Y + moves[i, 1]) && movesToReachMas[hex.X + moves[i, 0], hex.Y + moves[i, 1]]==0)
                    {
                        movesToReachMas[hex.X + moves[i, 0], hex.Y + moves[i, 1]] = movesToReachMas[hex.X, hex.Y] + 1;
                        queue.Enqueue((hex.X + moves[i, 0], hex.Y + moves[i, 1]));
                    }
                }
            }

            for(int i=0;i< MapSize + 1; i++)
                for(int j=0;j< MapSize + 1; j++)
                {
                    if (hexMas[i, j] == 1)
                        movesToReachMas[i, j] = 0;
                }
            movesToReachMas[pickedUnit.X, pickedUnit.Y] = 0;

            List<HexDTO> result = new();

            for(int i=0;i< MapSize + 1; i++)
                for(int j=0;j<MapSize+1;j++)
                {
                    if (movesToReachMas[i, j] != 0)
                    {
                        result.Add(new HexDTO(i, j, movesToReachMas[i, j]));
                    }
                }

            return result;
        }
        public void moveToHex(UnitDTO lastUnit,Dictionary<(int, int), UnitDTO> UnitMap,int x,int  y)
        {
            UnitDTO movedUnit = new UnitDTO();
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
