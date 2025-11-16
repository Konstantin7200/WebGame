using System.Text.Json.Serialization;

namespace backend.Entities
{
    public class Attack
    {
        public enum AttackTypes{
            Ranged,
            Melee
        }
        public enum DamageTypes
        {
            Arcane,
            Fire,
            Pierce,
            Slash,
            Smash
        }
        public int Damage { get; }
        public int AttacksAmount { get; }
        public string AttackName { get; }
        public DamageTypes DamageType { get; }
        public AttackTypes AttackType { get; }
        [JsonConstructor]
        public Attack(string attackName,AttackTypes attackType,DamageTypes damageType,int damage,int attacksAmount)
        {
            DamageType = damageType;
            AttackType = attackType;
            AttackName = attackName;
            Damage = damage;
            AttacksAmount = attacksAmount;
        }
    }
}
