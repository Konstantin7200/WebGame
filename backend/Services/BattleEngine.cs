using backend.Entities;
using System.Reflection.Metadata.Ecma335;

namespace backend.Services
{
    public class BattleEngine
    {
        public int fight(Unit attacker,Unit defender,Attack attackersAttack,Attack defendersAttack)
        {
            attacker.attacked = true;
            int aAttacksLeft = attackersAttack.AttacksAmount;
            int dAttacksLeft = defendersAttack.AttacksAmount;
            while(aAttacksLeft!=0&&dAttacksLeft!=0)
            {
                if(aAttacksLeft != 0)
                {
                    defender.takeDamage(attackersAttack.Damage * 1);
                    aAttacksLeft -= 1;
                    if (defender.isDead())
                    {
                        return 1;
                    }
                }
                if (dAttacksLeft != 0)
                {
                    attacker.takeDamage(defendersAttack.Damage * 1);
                    dAttacksLeft -= 1;
                    if (attacker.isDead())
                    {
                        return -1;
                    }
                }
            }
            return 0;
        }
    }
}
