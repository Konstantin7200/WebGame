using backend.Entities;

namespace backend.Services
{
    public static class PathFinder
    {
        public const int MAPSIZE = 7;
        static public int[,] getNeighbourHexes(int y)
        {
            int[,] movesEven = new int[,] { { 0, -1 }, { 1, -1 }, { -1, 0 }, { 1, 0 }, { 0, 1 }, { 1, 1 } };
            int[,] movesOdd = new int[,] { { -1, -1 }, { 0, -1 }, { -1, 0 }, { 1, 0 }, { -1, 1 }, { 0, 1 } };
            if (y % 2 == 0)
                return movesEven;
            return movesOdd;
        }
        static public List<EnemiesHex> findEnemiesHexes(List<Hex> hexes, Unit pickedUnit, int[,] hexMas)
        {
            Dictionary<(int, int), EnemiesHex> enemiesHexes = new Dictionary<(int, int), EnemiesHex>();
            hexes.Insert(0, new Hex(pickedUnit.X, pickedUnit.Y, 0));

            foreach (Hex hex in hexes)
            {
                int[,] moves = getNeighbourHexes(hex.Y);
                for (int i = 0; i < moves.GetLength(0); i++)
                {
                    if (checkIfHexExists(hex.X + moves[i, 0], hex.Y + moves[i, 1]))
                        if (hexMas[hex.X + moves[i, 0], hex.Y + moves[i, 1]] == -2)
                        {
                            enemiesHexes.TryAdd((hex.X + moves[i, 0], hex.Y + moves[i, 1]), new EnemiesHex(hex.X + moves[i, 0], hex.Y + moves[i, 1], hex));
                        }
                }
            }
            hexes.RemoveAt(0);

            return enemiesHexes.Values.ToList();
        }
        static public void markControlZones(Dictionary<(int, int), Unit> unitMap, Unit pickedUnit, int[,] hexMas)
        {
            (int X, int Y) hex = new();
            foreach (var unit in unitMap)
            {
                hex = (unit.Value.X, unit.Value.Y);
                if (unit.Value.Side != pickedUnit.Side)
                {
                    int[,] moves = getNeighbourHexes(hex.Y);
                    for (int i = 0; i < moves.GetLength(0); i++)
                    {
                        hexMas[hex.X, hex.Y] = -1;
                        if (checkIfHexExists(hex.X + moves[i, 0], hex.Y + moves[i, 1]))
                            hexMas[hex.X + moves[i, 0], hex.Y + moves[i, 1]] = -1;
                    }
                }
            }
        }
        static public List<Hex> findFreeHexes(Dictionary<(int, int), Unit> unitMap, Unit pickedUnit, int[,] hexMas)
        {
            (int X, int Y) hex = new();
            markControlZones(unitMap,pickedUnit, hexMas);

            hex = (pickedUnit.X, pickedUnit.Y);
            int[,] movesToReachMas = new int[MAPSIZE, MAPSIZE];
            movesToReachMas[hex.X, hex.Y] = 0;
            hexMas[hex.X, hex.Y] = 0;
            Queue<(int, int)> queue = new();
            queue.Enqueue(hex);
            while (queue.Count != 0)
            {
                hex = queue.Dequeue();
                if (hexMas[hex.X, hex.Y] == -1 || movesToReachMas[hex.X, hex.Y] == pickedUnit.MovesLeft)
                {
                    movesToReachMas[hex.X, hex.Y] = pickedUnit.MovesLeft;
                    continue;
                }
                int[,] moves = getNeighbourHexes(hex.Y);   
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
            foreach (var unit in unitMap.Values)
            {
                movesToReachMas[unit.X, unit.Y] = 0;
                if (unit.Side != pickedUnit.Side)
                {
                    hexMas[unit.X, unit.Y] = -2;
                }
            }
            List<Hex> result = new();

            for (int i = 0; i < MAPSIZE; i++)
                for (int j = 0; j < MAPSIZE; j++)
                {
                    if (movesToReachMas[i, j] != 0)
                    {
                        result.Add(new Hex(i, j, movesToReachMas[i, j]));
                    }
                }
            return result;
        }
        static public List<Hex> findPathToClosestEnemy(Unit myUnit, Dictionary<(int, int), Unit> unitMap, Unit closestEnemy, int[,] movesToReachMas)
        {
            (int X, int Y) hex = new();
            Queue<(int, int)> queue = new();
            queue.Enqueue((closestEnemy.X, closestEnemy.Y));
            List<Hex> path = new();
            movesToReachMas[myUnit.X, myUnit.Y] = 0;
            while (queue.Count != 0)
            {
                hex = queue.Dequeue();
                if (hex.X == myUnit.X && hex.Y == myUnit.Y)
                {
                    break;
                }
                int[,] moves = getNeighbourHexes(hex.Y);
                for (int i = 0; i < moves.GetLength(0); i++)
                {
                    if (checkIfHexExists(hex.X + moves[i, 0], hex.Y + moves[i, 1]))
                    {
                        if (movesToReachMas[hex.X, hex.Y] - 1 == movesToReachMas[hex.X + moves[i, 0], hex.Y + moves[i, 1]])
                        {
                            path.Add(new Hex(hex.X + moves[i, 0], hex.Y + moves[i, 1], movesToReachMas[hex.X + moves[i, 0], hex.Y + moves[i, 1]]));
                            queue.Enqueue((hex.X + moves[i, 0], hex.Y + moves[i, 1]));
                            break;
                        }
                    }
                }
            }
            return path;
        }
        static public Unit findClosestEnemy(Unit myUnit, Dictionary<(int, int), Unit> unitMap, int[,] movesToReachMas)
        {
            
            int[,] hexMas = new int[MAPSIZE, MAPSIZE];
            foreach (var unit in unitMap.Values)
                if (unit.Side != myUnit.Side)
                {
                    hexMas[unit.X, unit.Y] = -2;
                }
                else
                {
                    movesToReachMas[unit.X, unit.Y] = int.MaxValue;
                }

            (int X, int Y) hex = new();
            Unit closestEnemy = new();
            Queue<(int, int)> queue = new();
            queue.Enqueue((myUnit.X, myUnit.Y));
            movesToReachMas[myUnit.X, myUnit.Y] = 0;
            while (queue.Count != 0)
            {
                hex = queue.Dequeue();
                if (hexMas[hex.X, hex.Y] == -2)
                {
                    closestEnemy = unitMap[(hex.X, hex.Y)];
                    break;
                }
                int[,] moves = getNeighbourHexes(hex.Y);
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
            return closestEnemy;
        }
        static bool checkIfHexExists(int X, int Y)
        {
            if (X >= 0 && X < MAPSIZE && Y >= 0 && Y < MAPSIZE)
            {
                return true;
            }
            return false;
        }
    }
}
