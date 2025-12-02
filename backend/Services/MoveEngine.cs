using backend.DTOes;
using backend.Entities;
using Microsoft.AspNetCore.Mvc;

namespace backend.Services
{
    public class MoveEngine
    {
        int[,] movesEven = new int[,] { { 0, -1 }, { 1, -1 }, { -1, 0 }, { 1, 0 }, { 0, 1 }, { 1, 1 } };
        int[,] movesOdd = new int[,] { { -1, -1 }, { 0, -1 }, { -1, 0 }, { 1, 0 }, { -1, 1 }, { 0, 1 } };
        public const int MAPSIZE = 7;
        int[,] hexMas = new int[MAPSIZE, MAPSIZE];

        public List<EnemiesHex> findEnemiesHexes(List<HexDTO> hexes,Unit pickedUnit)
        {
            Dictionary<(int, int), EnemiesHex> enemiesHexes = new Dictionary<(int, int), EnemiesHex>();
            hexes.Insert(0,new HexDTO(pickedUnit.X, pickedUnit.Y, 0));

            foreach (HexDTO hex in hexes)
            {
                int[,] moves = hex.Y % 2 == 0 ? movesEven : movesOdd;
                for (int i = 0; i < moves.GetLength(0); i++)
                {
                    if (checkIfHexExists(hex.X + moves[i, 0], hex.Y + moves[i, 1]))
                        if(hexMas[hex.X + moves[i, 0], hex.Y + moves[i, 1]]==-2)
                        {
                            enemiesHexes.TryAdd((hex.X + moves[i, 0], hex.Y + moves[i, 1]), new EnemiesHex(hex.X + moves[i, 0], hex.Y + moves[i, 1], hex));
                        }
                }
            }
            hexes.RemoveAt(0);

            return enemiesHexes.Values.ToList();
        }

        bool checkIfHexExists(int X,int Y)
        {
            if(X>=0&&X< MAPSIZE && Y>=0&&Y< MAPSIZE)
            {
                return true;
            }
            return false;
        }
        public List<HexDTO> findAvalibleHexes(Dictionary<(int,int),Unit> UnitMap, Unit pickedUnit)
        {
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
            int[,] movesToReachMas = new int[MAPSIZE, MAPSIZE];
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
            foreach (var unit in UnitMap.Values)
            {
                movesToReachMas[unit.X, unit.Y] = 0;
                if(unit.Side!=pickedUnit.Side)
                {
                    hexMas[unit.X, unit.Y] = -2;
                }
            }
            List<HexDTO> result = new();

            for(int i=0;i< MAPSIZE; i++)
                for(int j=0;j<MAPSIZE;j++)
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
            Console.WriteLine("MoveStart "+lastUnit.X + " " + lastUnit.Y + " " + lastUnit.MovesLeft);
            lastUnit.MovesLeft -= movesToReach;
            UnitMap.Remove((lastUnit.X, lastUnit.Y));
            lastUnit.X = x;
            lastUnit.Y = y;
            /*Unit movedUnit = lastUnit.createNew();
            movedUnit.copyFrom(lastUnit);*/
            UnitMap.Add((x, y), lastUnit);
            Console.WriteLine("MoveEnd "+lastUnit.X + " " + lastUnit.Y + " " + lastUnit.MovesLeft);
        }
        public void moveToHex(Unit lastUnit,Dictionary<(int,int),Unit> unitMap,HexDTO hex)
        {
            moveToHex(lastUnit, unitMap, hex.X, hex.Y, hex.Moves);
        }
        public MovesDTO getAllMoves(Dictionary<(int, int), Unit> UnitMap, Unit pickedUnit)
        {
            List<HexDTO> hexes = findAvalibleHexes(UnitMap, pickedUnit);
            List<EnemiesHex> enemiesHexes = findEnemiesHexes(hexes,pickedUnit);
            return new MovesDTO(hexes, enemiesHexes);
        }
        public List<HexDTO> findPathToClosestEnemy(Unit myUnit, Dictionary<(int, int), Unit> unitMap)
        {
            int[,] hexMas = new int[MAPSIZE, MAPSIZE];
            int[,] movesToReachMas = new int[MAPSIZE, MAPSIZE];
            foreach (var unit in unitMap.Values)
                if (unit.Side!=myUnit.Side)
                {
                    hexMas[unit.X, unit.Y] = 1;
                }
                else
                {
                    movesToReachMas[unit.X, unit.Y] = int.MaxValue;
                }

                    (int X, int Y) hex = new();
            Unit closestEnemy = new();
            Queue<(int,int)> queue = new();
            queue.Enqueue((myUnit.X, myUnit.Y));
            movesToReachMas[myUnit.X, myUnit.Y] = 0;
            while (queue.Count != 0)
            {
                hex = queue.Dequeue();
                if (hexMas[hex.X, hex.Y] == 1)
                {
                    closestEnemy = unitMap[(hex.X, hex.Y)];
                    break;
                }
                int[,] moves = hex.Y % 2 == 0 ? movesEven : movesOdd;
                for (int i = 0; i < moves.GetLength(0); i++)
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
            queue.Clear();
            queue.Enqueue((closestEnemy.X, closestEnemy.Y));
            List<HexDTO> path = new();
            movesToReachMas[myUnit.X, myUnit.Y] = 0;
            while (queue.Count!=0)
            {
                hex = queue.Dequeue();
                if (hex.X==myUnit.X&&hex.Y==myUnit.Y)
                {
                    break;
                }
                int[,] moves = hex.Y % 2 == 0 ? movesEven : movesOdd;
                for (int i = 0; i < moves.GetLength(0); i++)
                {
                    if (checkIfHexExists(hex.X + moves[i, 0], hex.Y + moves[i, 1]))
                    {
                        if (movesToReachMas[hex.X, hex.Y]-1== movesToReachMas[hex.X + moves[i, 0], hex.Y + moves[i, 1]])
                        {
                            path.Add(new HexDTO(hex.X + moves[i, 0], hex.Y + moves[i, 1], movesToReachMas[hex.X + moves[i, 0], hex.Y + moves[i, 1]]));
                            queue.Enqueue((hex.X + moves[i, 0], hex.Y + moves[i, 1]));
                            break;
                        }    
                        queue.Enqueue((hex.X + moves[i, 0], hex.Y + moves[i, 1]));
                    
                    }
                }
            }
            return path;
        }
        public bool moveToClosestEnemy(Unit myUnit,Dictionary<(int,int),Unit> unitMap)
        {
            List<HexDTO> path = findPathToClosestEnemy(myUnit, unitMap);
            foreach (HexDTO hex in path)
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
