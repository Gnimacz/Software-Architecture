using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is responsible for the goal.
/// It is used to detect when an enemy reaches the goal and then raise an event to let the game manager know that the enemy reached the goal.
/// </summary>
public class Goal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;
        BaseEnemy enemyThatEntered = other.GetComponent<BaseEnemy>();
        EventBus<EnemyReachedGoalEvent>.Raise(new EnemyReachedGoalEvent(enemyThatEntered));
    }
}
