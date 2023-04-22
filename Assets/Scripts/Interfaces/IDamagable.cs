using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    float Health { get; set; }
    int Damage { get; set; }
    bool IsAlive { get; set; }
    bool HasDebuff { get; set; }
    int Money { get; set; }

    public void OnDeath();
    public void OnHurt();
    public void TakeDamage(float damage, DamageType damageType);

    public enum DamageType
    {
        Physical,
        Debuff_Slow
    }
}
