using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyToSpawn;
    public GameObject goal;
    public float SpawnTime = 1.2f;

    private void Start()
    {
        WaveStart();
    }

    private void WaveStart()
    {
        StartCoroutine(spawnEnemies());
    }

    IEnumerator spawnEnemies()
    {
        while (true)
        {
            BaseEnemy enemy = Instantiate(enemyToSpawn, transform).gameObject.GetComponent<BaseEnemy>();
            enemy.goal = goal.transform;
            yield return new WaitForSeconds(SpawnTime);
            Debug.Log("Timer up");
        }
    }
}
