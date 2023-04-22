using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    List<WaveScriptableObject> waves = new List<WaveScriptableObject>();
    [SerializeField][Header("Levels"), Tooltip("A list containing Level ScriptableObjects")] List<LevelScriptableObject> levels = new List<LevelScriptableObject>();
    [SerializeField] int currentWave = 0;
    int currentLevel = 0;
    [SerializeField] int timeBetweenWaves = 5;
    [SerializeField] GameObject start;
    [SerializeField] GameObject goal;

    bool isWaveGoingOn = false;

    private List<BaseEnemy> enemies = new List<BaseEnemy>();

    private static EnemyManager instance;

    public static EnemyManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<EnemyManager>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        //singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        currentLevel = 0;
        LoadWaves();
        //WaveStart();
        WavesWithDelay();

        EventBus<EnemyKilledEvent>.Subscribe(OnEnemyDeath);
        EventBus<EnemyReachedGoalEvent>.Subscribe(OnEnemyReachedGoal);
    }

    private void WaveStart()
    {
        StartCoroutine(spawnEnemies());
    }

    void LoadWaves()
    {
        currentWave = 0;
        waves.Clear();
        foreach (WaveScriptableObject wave in levels[currentLevel].waves)
        {
            waves.Add(wave);
        }
        //GameManager.Instance.Health = levels[currentLevel].health;
        EventBus<HealthUpdateEvent>.Raise(new HealthUpdateEvent(levels[currentLevel].health));
        timeBetweenWaves = levels[currentLevel].timeBetweenWaves;
    }

    void LoadNextLevel()
    {
        currentLevel++;
        if (currentLevel >= levels.Count) currentLevel = 0;
        LoadWaves();
    }

    IEnumerator spawnEnemies()
    {
        isWaveGoingOn = true;
        for (int i = 0; i < waves[currentWave].EnemiesToSpawn.Count; i++)
        {
            GameObject enemy = Instantiate(waves[currentWave].EnemiesToSpawn[i], start.transform.position, Quaternion.identity);
            enemy.GetComponent<BaseEnemy>().goal = goal.transform;
            enemies.Add(enemy.GetComponent<BaseEnemy>());
            yield return new WaitForSeconds(waves[currentWave].TimeBetweenSpawns);
        }
    }

    IEnumerator WaitForWave()
    {
        for (int i = 0; i < timeBetweenWaves; i++)
        {
            EventBus<WaveStatusUpdate>.Raise(new WaveStatusUpdate(true, timeBetweenWaves - i));
            Debug.LogWarning("Wave in cooldown!\n" + (timeBetweenWaves - i) + " remaining.");
            yield return new WaitForSeconds(1);
        }
        EventBus<WaveStatusUpdate>.Raise(new WaveStatusUpdate(false, 0));
        WaveStart();
    }

    void OnEnemyDeath(Event e)
    {
        EnemyKilledEvent enemyKilledEvent = (EnemyKilledEvent)e;
        RemoveEnemy(enemyKilledEvent.enemy);

    }

    void WavesWithDelay()
    {
        if (currentWave >= waves.Count)
        {
            LoadNextLevel();
        }
        StartCoroutine(WaitForWave());
    }

    void OnEnemyReachedGoal(Event e)
    {
        EnemyReachedGoalEvent enemyReachedGoalEvent = (e as EnemyReachedGoalEvent);
        RemoveEnemy(enemyReachedGoalEvent.enemy);
    }
    
    void RemoveEnemy(BaseEnemy enemy)
    {
        enemies.Remove(enemy);
        Destroy(enemy.gameObject);
        Debug.Log(enemies.Count);
        if (enemies.Count <= 0)
        {
            currentWave++;
            WavesWithDelay();
        }
    }

}
