using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This interface is responsible for the damagable objects.
/// It is used to define the properties and methods that a damagable object should have.
/// It also holds the data structures for the debuffs and the damage types.
/// </summary>
public interface IDamagable
{
    float Health { get; set; }
    float MaxHealth { get; set; }
    int Damage { get; set; }
    bool IsAlive { get; set; }
    List<Debuff> Debuffs { get; set; }
    int Money { get; set; }
    Transform ParentTransform { get; }

    public void OnDeath();
    public void OnHurt(float damageTaken);
    public void TakeDamage(float damage, DamageType damageType);
    public void AddDebuff(Debuff debuff);
    public void RemoveDebuff(Debuff debuff);

    public enum DamageType
    {
        Physical
    }

    public enum DebuffType
    {
        Slow
    }
    [System.Serializable]
    public struct Debuff
    {
        public Debuff(DebuffType debuffType, float level)
        {
            this.debuffType = debuffType;
            this.level = level;
        }
        public DebuffType debuffType;
        public float level;
    }
}
