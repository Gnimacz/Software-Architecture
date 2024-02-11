using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : MonoBehaviour, IDamagable
{
    [SerializeField] float hp = 6f;
    float maxHp = 6f;
    [SerializeField] float speed = 10f;
    [SerializeField] int damage = 1;
    [SerializeField] bool isAlive = true;
    [SerializeField] List<IDamagable.Debuff> debuffs = new List<IDamagable.Debuff>();
    [SerializeField] int money = 5;


    #region Nav Mesh
    private NavMeshAgent meshAgent;
    public Transform goal;
    #endregion

    #region Interface Implementations
    public float Health
    {
        get { return hp; }
        set { hp = value; }
    }

    public float MaxHealth
    {
        get { return maxHp; }
        set { maxHp = value; }
    }

    public int Damage
    {
        get { return damage; }
        set { damage = value; }
    }

    public bool IsAlive
    {
        get { return isAlive; }
        set { isAlive = value; }
    }

    public List<IDamagable.Debuff> Debuffs
    {
        get { return debuffs; }
        set { debuffs = value; }
    }

    public int Money
    {
        get { return money; }
        set { money = value; }
    }

    public Transform ParentTransform
    {
        get { return transform; }
    }


    public void OnDeath()
    {
        isAlive = false;
        EventBus<EnemyKilledEvent>.Raise(new EnemyKilledEvent(this));
    }

    public void OnHurt(float damageTaken)
    {
        EventBus<EnemyHurtEvent>.Raise(new EnemyHurtEvent(this, damageTaken));
    }

    public void TakeDamage(float damage, IDamagable.DamageType damageType)
    {
        switch (damageType)
        {
            case IDamagable.DamageType.Physical:
                Health -= damage;
                if (Health <= 0f)
                {
                    Health = 0f;
                    OnDeath();
                    return;
                }
                OnHurt(damage);
                break;
            default:
                break;
        }



    }

    public void AddDebuff(IDamagable.Debuff debuff)
    {
        if (Debuffs.Contains(debuff)) return;
        Debuffs.Add(debuff);
        switch (debuff.debuffType)
        {
            case IDamagable.DebuffType.Slow:
                speed *= debuff.level;
                break;

            default:
                break;
        }
    }

    public void RemoveDebuff(IDamagable.Debuff debuff)
    {
        if (!Debuffs.Contains(debuff)) return;
        Debuffs.Remove(debuff);
        switch (debuff.debuffType)
        {
            case IDamagable.DebuffType.Slow:
                speed /= debuff.level;
                break;

            default:
                break;
        }
    }
    #endregion

    void Start()
    {
        meshAgent = GetComponent<NavMeshAgent>();
        maxHp = hp;
    }

    void Update()
    {
        MoveEnemy();
    }

    void MoveEnemy()
    {
        meshAgent.SetDestination(goal.position);
        meshAgent.speed = speed;
    }
}
