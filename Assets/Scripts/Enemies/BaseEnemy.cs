using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using PathCreation;

public class BaseEnemy : MonoBehaviour, IDamagable
{
    [SerializeField] float hp = 6f;
    [SerializeField] float speed = 10f;
    [SerializeField] float damage = 2f;


    #region Nav Mesh
    private NavMeshAgent meshAgent;
    public Transform goal;
    #endregion

    #region Interface Implementations
    public float health
    {
        get { return hp; }
        set { hp = value; }
    }

    public void OnDeath()
    {
        Destroy(this.gameObject);
    }

    public void OnHurt()
    {
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if(health <= 0f)
        {
            OnDeath();
            return;
        }
        OnHurt();
    }
    #endregion

    void Start()
    {
        meshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
            meshAgent.SetDestination(goal.position);
    }
}
