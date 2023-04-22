using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;
        BaseEnemy enemyThatEntered = other.GetComponent<BaseEnemy>();
        EventBus<EnemyReachedGoalEvent>.Raise(new EnemyReachedGoalEvent(enemyThatEntered));
    }
}
