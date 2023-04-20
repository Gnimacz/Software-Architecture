using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    List<WaveScriptableObject> waves = new List<WaveScriptableObject>();
    [SerializeField][Header("Levels"), Tooltip("A list containing Level ScriptableObjects")] List<LevelScriptableObject> levels = new List<LevelScriptableObject>();
    [SerializeField] int currentWave = 0;
    int currentLevel = 0;
    [SerializeField] float timeBetweenWaves = 5f;
    float timeBetweenSpawns;
    [SerializeField] GameObject goal;

    bool isWaveGoingOn = false;
    private void Start()
    {
        currentLevel = 0;
        LoadWaves();
        WaveStart();
    }

    private void WaveStart()
    {
        StartCoroutine(spawnEnemies());
    }

    void LoadWaves()
    {
        waves.Clear();
        foreach (WaveScriptableObject wave in levels[currentLevel].waves)
        {
            waves.Add(wave);
        }
    }

    void NextLevel()
    {
        currentLevel++;
        LoadWaves();
    }

    IEnumerator spawnEnemies()
    {
        for (int i = 0; i < waves[currentWave].EnemiesToSpawn.Count; i++)
        {
            GameObject enemy = Instantiate(waves[currentWave].EnemiesToSpawn[i], transform.position, Quaternion.identity);
            enemy.GetComponent<BaseEnemy>().goal = goal.transform;
            yield return new WaitForSeconds(waves[currentWave].TimeBetweenSpawns);
        }
    }
}
