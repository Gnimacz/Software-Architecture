using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// This class is responsible for managing the waves of enemies.
/// It is used to spawn enemies, handle enemy deaths and enemy reaching the goal.
/// </summary>
public class WaveManager : MonoBehaviour
{
    public List<WaveScriptableObject> waves = new List<WaveScriptableObject>();
    [SerializeField][Header("Levels"), Tooltip("A list containing Level ScriptableObjects")] List<LevelScriptableObject> levels = new List<LevelScriptableObject>();
    public int currentWave = 0;
    public int currentLevel = 0;
    public int timeBetweenWaves;
    [SerializeField] GameObject start;
    [SerializeField] GameObject goal;


    private List<BaseEnemy> enemies = new List<BaseEnemy>();
    public List<BaseEnemy> Enemies { get { return enemies; } }

    private Stack<GameObject> enemiesInWaveObject;

    private static WaveManager instance;

    public static WaveManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<WaveManager>();
            }
            return instance;
        }
    }

    private void Awake()
    {

        currentLevel = 0;
        LoadWaves();
        //WaveStart();
        WavesWithDelay();

        EventBus<EnemyKilledEvent>.Subscribe(OnEnemyDeath);
        EventBus<EnemyReachedGoalEvent>.Subscribe(OnEnemyReachedGoal);
        EventBus<GameOverEvent>.Subscribe(OnGameOver);
    }

    private void OnDisable()
    {
        EventBus<EnemyKilledEvent>.Unsubscribe(OnEnemyDeath);
        EventBus<EnemyReachedGoalEvent>.Unsubscribe(OnEnemyReachedGoal);
        EventBus<GameOverEvent>.Unsubscribe(OnGameOver);
    }

    private void WaveStart()
    {
        StartCoroutine(SpawnEnemies());
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
        if(GameManager.Instance.isGameOver) return;
        currentLevel++;
        if (currentLevel >= levels.Count)
        {
            EventBus<GameOverEvent>.Raise(new GameOverEvent(true));
            return;
        }
        LoadWaves();
    }


    /// <summary>
    /// Coroutine that spawns enemies based on the current wave configuration.
    /// </summary>
    IEnumerator SpawnEnemies()
    {
        if(GameManager.Instance.isGameOver) yield break;
        enemiesInWaveObject = new Stack<GameObject>(waves[currentWave].EnemiesToSpawn);

        while (enemiesInWaveObject.Count > 0)
        {
            GameObject enemy = Instantiate(enemiesInWaveObject.Pop(), start.transform.position, Quaternion.identity);
            enemy.GetComponent<BaseEnemy>().goal = goal.transform;
            enemies.Add(enemy.GetComponent<BaseEnemy>());
            yield return new WaitForSeconds(waves[currentWave].TimeBetweenSpawns);
        }
    }

    /// <summary>
    /// Coroutine that waits for a certain amount of time between waves.
    /// </summary>
    IEnumerator WaitForWave()
    {
        if(GameManager.Instance.isGameOver) yield break;
        for (int i = 0; i < timeBetweenWaves; i++)
        {
            EventBus<WavePauseUpdate>.Raise(new WavePauseUpdate(true, timeBetweenWaves - i));
            Debug.LogWarning("Wave in cooldown!\n" + (timeBetweenWaves - i) + " remaining.");
            yield return new WaitForSeconds(1);
        }
        EventBus<WavePauseUpdate>.Raise(new WavePauseUpdate(false, 0));
        WaveStart();
    }

    /// <summary>
    /// Handles the event when an enemy dies.
    /// </summary>
    /// <param name="e">The event object.</param>
    void OnEnemyDeath(Event e)
    {
        EnemyKilledEvent enemyKilledEvent = (EnemyKilledEvent)e;
        RemoveEnemy(enemyKilledEvent.enemy);

    }

    /// <summary>
    /// Executes the waves of enemies with a delay between each wave.
    /// </summary>
    void WavesWithDelay()
    {
        if (currentWave >= waves.Count)
        {
            LoadNextLevel();
        }
        StartCoroutine(WaitForWave());
    }

    /// <summary>
    /// Event handler for when an enemy reaches the goal.
    /// </summary>
    /// <param name="e">The event containing information about the enemy.</param>
    void OnEnemyReachedGoal(Event e)
    {
        EnemyReachedGoalEvent enemyReachedGoalEvent = (e as EnemyReachedGoalEvent);
        RemoveEnemy(enemyReachedGoalEvent.enemy);
    }
    
    /// <summary>
    /// Removes an enemy from the list of enemies and destroys its game object.
    /// If there are no more enemies in the current wave and no more enemies overall, and the game is not over, it proceeds to the next wave.
    /// </summary>
    /// <param name="enemy">The enemy to be removed.</param>
    void RemoveEnemy(BaseEnemy enemy)
    {
        if (enemy == null || !enemies.Contains(enemy)) return;
        enemies.Remove(enemy);
        Destroy(enemy.gameObject);
        if (enemiesInWaveObject?.Count() <= 0 && enemies?.Count <= 0 && !GameManager.Instance.isGameOver)
        {
            currentWave++;
            WavesWithDelay();
        }
    }

    void OnGameOver(Event e)
    {
        foreach (BaseEnemy residue in enemies.ToList())
        {
            RemoveEnemy(residue);
            enemiesInWaveObject = null;
            StopCoroutine(SpawnEnemies());
        }
    }

}
