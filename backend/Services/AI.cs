using backend.DTOes;
using backend.Entities;

namespace backend.Services
{
    public class AI
    {
        Dictionary<(int, int), Unit> _unitMap;
        public AI(Dictionary<(int, int), Unit> unitMap)
        {
            _unitMap = unitMap;
        }

        public bool start(Turn currentTurn)
        {
            List<Unit> units = new();
            foreach(var unit in _unitMap.Values)
            {
                if (unit.Side == currentTurn.currentTurn && unit.MovesLeft != 0&&!unit.attacked)
                {
                    Console.WriteLine(unit.X + " " + unit.Y +" "+ unit.MovesLeft);
                    makeAMove(unit);
                    return true;
                }
            }
            
            Console.WriteLine(false);
            return false;

        }
        public void makeAMove(Unit myUnit)
        {
            PathFinder pathfinder = new PathFinder();
            MovesDTO moves=pathfinder.getAllMoves(_unitMap,myUnit);
            HexDTO hexToStepOn;
            Unit pickedUnit;
            Console.WriteLine("MyHex" + myUnit.X +" "+myUnit.Y);
            foreach(EnemiesHex hex in moves.enemiesHexes)
            {
                Console.WriteLine(hex.X + " " + hex.Y);
            }
            (pickedUnit,hexToStepOn)=findTheBestOpponent(moves.enemiesHexes);
            if(pickedUnit!=null&&hexToStepOn!=null)
            {
                Console.WriteLine("Picked opponent "+pickedUnit.X + " " + pickedUnit.Y);
                (Attack,Attack) attackPair=pickAttack(myUnit, pickedUnit);
                BattleEngine battleEngine = new BattleEngine();
                battleEngine.fight(myUnit, pickedUnit, attackPair.Item1, attackPair.Item2,_unitMap);
                pathfinder.moveToHex(myUnit, _unitMap, hexToStepOn); 
            }
            else
            {
                moveRandomly(moves, myUnit);
            }
        }
        public (Unit?,HexDTO?) findTheBestOpponent(List<EnemiesHex> enemiesHexes)
        {
            int minHp = int.MaxValue;
            Unit pickedUnit=null;
            HexDTO hexToStayOn = null;
            foreach(EnemiesHex hex in enemiesHexes)
            {
                if (_unitMap[(hex.X,hex.Y)].Health<minHp)
                {
                    minHp = _unitMap[(hex.X, hex.Y)].Health;
                    pickedUnit = _unitMap[(hex.X, hex.Y)];
                    hexToStayOn = hex.Previous;
                }
            }
            return (pickedUnit,hexToStayOn);
        }
        public (Attack,Attack) pickAttack(Unit ourUnit,Unit pickedUnit)
        {
            Attack fakeAttack = new Attack("Nothing", Attack.AttackTypes.Melee, Attack.DamageTypes.Slash, 0, 0);
            
            double attackValue=-1;
            int damageDiff = -1;
            double newAttackValue;
            int newDamageDiff;
            (Attack, Attack) attackPair = (fakeAttack, fakeAttack);
            foreach(Attack attack in ourUnit.BaseUnit.Attacks)
            {
                Attack othersAttack = fakeAttack;
                foreach(Attack othersPotentialAttack in pickedUnit.BaseUnit.Attacks)
                {
                    if (attack.AttackType == othersPotentialAttack.AttackType)
                        othersAttack = othersPotentialAttack;
                }
                (newAttackValue,newDamageDiff)=calculateAttackValueAndDiff(attack, othersAttack);
                if(attackValue<newAttackValue)
                {
                    attackPair = (attack, othersAttack);
                }
                else if(attackValue==newAttackValue&&damageDiff<newDamageDiff)
                {
                    attackPair = (attack, othersAttack);
                }
            }

            return attackPair;
        }
        public (double,int) calculateAttackValueAndDiff(Attack yourAttack,Attack othersAttack)
        {
            double value=int.MaxValue;
            if (othersAttack.Damage!=0)
                value = 1.0 * yourAttack.Damage * yourAttack.AttacksAmount / othersAttack.Damage / othersAttack.AttacksAmount;
            int damageDiff = yourAttack.Damage * yourAttack.AttacksAmount - othersAttack.Damage * othersAttack.AttacksAmount;
            return (value, damageDiff);
        }
        /*public void moveToTheEnemyLeader()
        {

        }*/
        public void moveRandomly(MovesDTO moves,Unit myUnit)
        {
            PathFinder pathFinder = new PathFinder();
            List<HexDTO> hexes=new();
            foreach(HexDTO hex in moves.hexes)
            {
                if (myUnit.MovesLeft == hex.Moves)
                    hexes.Add(hex);
            }
            pathFinder.moveToHex(myUnit, _unitMap, hexes[Random.Shared.Next(0, hexes.Count)]);
            
        }

    }
}
