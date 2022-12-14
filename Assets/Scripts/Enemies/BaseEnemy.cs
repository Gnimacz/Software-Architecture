using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using PathCreation;

public class BaseEnemy : MonoBehaviour, IDamagable
{
    float hp = 2f;
    public float speed = 10f;
    public float damage = 2f;

    #region Path Following Variables
    public bool doesPathExist = false;
    public PathCreator pathCreator;
    public EndOfPathInstruction end;
    private float distanceTravelled = 0f;
    #endregion

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
    }

    public void OnHurt()
    {
    }
    #endregion

    void Start()
    {
        if (pathCreator != null) doesPathExist = true;

        meshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (doesPathExist)
        {
            distanceTravelled += speed * Time.deltaTime;
            transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, end);
            //transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, end);
        }
        else
        {
            meshAgent.SetDestination(goal.position);
        }
    }
}
