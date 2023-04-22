using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int startMoney = 10;
    [SerializeField] public int money { get; private set; }
    [SerializeField] private int lives = 3;
    [SerializeField] private int currentWave = 0;
    [SerializeField] private int currentLevel = 0;

    [SerializeField] public bool isGameOver = false;
    [SerializeField] private bool playerWonGame = false;
    [SerializeField] private bool buildingPhase = true;

    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }

    public int Health { get => lives; set => lives = value; }


    private void OnEnable()
    {
        //subscribe to events here
        EventBus<HealthUpdateEvent>.Subscribe(OnHealthUpdate);
        EventBus<EnemyKilledEvent>.Subscribe(OnEnemyDeath);
        EventBus<EnemyReachedGoalEvent>.Subscribe(OnEnemyReachedGoal);
        EventBus<PurchaseEvent>.Subscribe(OnPurchase);

        AddMoney(startMoney);
    }

    private void Update()
    {
        Debug.LogError(money);
    }

    void OnEnemyDeath(Event e)
    {
        AddMoney((e as EnemyKilledEvent).enemy.Money);
        
    }

    public void AddMoney(int amount)
    {
        money += amount;
        EventBus<MoneyChangeEvent>.Raise(new MoneyChangeEvent(money));
    }

    public void RemoveMoney(int amount)
    {
        money -= amount;
    }

    public void SetMoney(int amount)
    {
        money = amount;
        EventBus<MoneyChangeEvent>.Raise(new MoneyChangeEvent(money));
    }

    void OnEnemyReachedGoal(Event e)
    {
        EventBus<HealthUpdateEvent>.Raise(new HealthUpdateEvent(lives - 1));
    }

    void OnPurchase(Event e)
    {
        RemoveMoney((e as PurchaseEvent).money);
        EventBus<MoneyChangeEvent>.Raise(new MoneyChangeEvent(money));
    }

    void OnHealthUpdate(Event e)
    {
        lives = (e as HealthUpdateEvent).health;
        if (lives <= 0)
        {
            isGameOver = true;
            playerWonGame = false;
            EventBus<GameOverEvent>.Raise(new GameOverEvent(playerWonGame));
        }
    }
}
