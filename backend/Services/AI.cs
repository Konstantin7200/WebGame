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
            /*foreach(var unit in _unitMap.Values)
            {
                if(unit.Side==currentTurn.currentTurn)
                {
                    Console.WriteLine(unit.X + " " + unit.Y+" "+unit.MovesLeft+" "+unit.attacked);
                }
            }
            Console.WriteLine("______________________________________");*/
            foreach(var unit in _unitMap.Values)
            {
                if (unit.Side == currentTurn.currentTurn && unit.MovesLeft != 0&&!unit.attacked)
                {
                    makeAMove(unit);
                    return true;
                }
            }
            return false;

        }
        public void makeAMove(Unit myUnit)
        {
            MoveEngine moveEngine = new MoveEngine();
            MovesDTO moves=moveEngine.getAllMoves(_unitMap,myUnit,myUnit.Side);
            HexDTO hexToStepOn;
            Unit pickedUnit;
            (pickedUnit,hexToStepOn)=findTheBestOpponent(moves.enemiesHexes);
            if(pickedUnit!=null&&hexToStepOn!=null)
            {
                moveEngine.moveToHex(myUnit, _unitMap, hexToStepOn);
                (Attack, Attack) attackPair = pickAttack(myUnit, pickedUnit);
                BattleEngine.fight(myUnit, pickedUnit, attackPair.Item1, attackPair.Item2,_unitMap);
            }
            else
            {
                if (!moveEngine.moveToClosestEnemy(myUnit, _unitMap))
                    moveRandomly(moves,myUnit);
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
                (newAttackValue,newDamageDiff)=calculateAttackValueAndDiff(attack, othersAttack,ourUnit,pickedUnit);
                if(attackValue<newAttackValue)
                {
                    attackValue = newAttackValue;
                    damageDiff=newDamageDiff;
                    attackPair = (attack, othersAttack);
                }
                else if(attackValue==newAttackValue&&damageDiff<newDamageDiff)
                {
                    attackValue = newAttackValue;
                    damageDiff = newDamageDiff;
                    attackPair = (attack, othersAttack);
                }
            }

            return attackPair;
        }
        public (double,int) calculateAttackValueAndDiff(Attack attackersAttack,Attack defendersAttack,Unit attacker,Unit defender)
        {
            double value = 10000;
            if (defendersAttack.Damage!=0)
                value = 1.0 *Math.Floor(attackersAttack.Damage * attackersAttack.AttacksAmount * (1 - defender.BaseUnit.Resistances[attackersAttack.DamageType])) / Math.Floor(defendersAttack.Damage * defendersAttack.AttacksAmount * (1 - attacker.BaseUnit.Resistances[defendersAttack.DamageType]));
            int damageDiff = (int)(attackersAttack.Damage * attackersAttack.AttacksAmount* (1 - defender.BaseUnit.Resistances[attackersAttack.DamageType])) - (int)(defendersAttack.Damage * defendersAttack.AttacksAmount* (1 - attacker.BaseUnit.Resistances[defendersAttack.DamageType]));
            return (value, damageDiff);
        }
        /*public void moveToTheEnemyLeader()
        {

        }*/
        public void moveRandomly(MovesDTO moves,Unit myUnit)
        {
            MoveEngine pathFinder = new MoveEngine();
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
