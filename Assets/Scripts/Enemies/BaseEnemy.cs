using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : MonoBehaviour, IDamagable
{
    [SerializeField] float hp = 6f;
    [SerializeField] float speed = 10f;
    [SerializeField] int damage = 1;
    [SerializeField] bool isAlive = true;
    [SerializeField] bool hasDebuff = false;
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

    public bool HasDebuff
    {
        get { return hasDebuff; }
        set { hasDebuff = value; }
    }

    public int Money
    {
        get { return money; }
        set { money = value; }
    }


    public void OnDeath()
    {
        isAlive = false;
        EventBus<EnemyKilledEvent>.Raise(new EnemyKilledEvent(this));
        //Destroy(gameObject);
    }

    public void OnHurt()
    {
    }

    public void TakeDamage(float damage, IDamagable.DamageType damageType)
    {
        switch (damageType)
        {
            case IDamagable.DamageType.Physical:
                Health -= damage;
                if (Health <= 0f)
                {
                    OnDeath();
                    return;
                }
                break;
            case IDamagable.DamageType.Debuff_Slow:
                if (!hasDebuff)
                {
                    hasDebuff = true;
                    speed *= 0.5f;
                }
                break;
            default:
                break;
        }

        

    }
    #endregion

    void Start()
    {
        meshAgent = GetComponent<NavMeshAgent>();
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
