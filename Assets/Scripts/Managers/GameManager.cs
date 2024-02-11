using UnityEngine;

/// <summary>
/// This class is responsible for the game manager.
/// It is used to keep track of the player's money and health.
/// It also listens to events and updates the money and health accordingly.
/// It also listens to the game over event and sets the game over state.
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] private int startMoney = 10;
    public int Money { get; private set; }
    [SerializeField] private int lives = 3;

    public bool isGameOver = false;
    [SerializeField] private bool playerWonGame = false;

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

    public bool PlayerWonGame { get => playerWonGame; set => playerWonGame = value; }

    private void OnEnable()
    {
        //subscribe to events here
        EventBus<HealthUpdateEvent>.Subscribe(OnHealthUpdate);
        EventBus<EnemyKilledEvent>.Subscribe(OnEnemyDeath);
        EventBus<EnemyReachedGoalEvent>.Subscribe(OnEnemyReachedGoal);
        EventBus<PurchaseEvent>.Subscribe(OnPurchase);
        EventBus<GameOverEvent>.Subscribe(OnGameOverEvent);

        AddMoney(startMoney);
    }

    private void OnDisable()
    {
        //unsubscribe to events here
        EventBus<HealthUpdateEvent>.Unsubscribe(OnHealthUpdate);
        EventBus<EnemyKilledEvent>.Unsubscribe(OnEnemyDeath);
        EventBus<EnemyReachedGoalEvent>.Unsubscribe(OnEnemyReachedGoal);
        EventBus<PurchaseEvent>.Unsubscribe(OnPurchase);
        EventBus<GameOverEvent>.Unsubscribe(OnGameOverEvent);
    }

    private void OnEnemyDeath(Event e)
    {
        AddMoney((e as EnemyKilledEvent).enemy.Money);
    }

    public void AddMoney(int amount)
    {
        Money += amount;
        EventBus<MoneyChangeEvent>.Raise(new MoneyChangeEvent(Money));
    }

    public void RemoveMoney(int amount)
    {
        Money -= amount;
    }

    public void SetMoney(int amount)
    {
        Money = amount;
        EventBus<MoneyChangeEvent>.Raise(new MoneyChangeEvent(Money));
    }

    private void OnEnemyReachedGoal(Event e)
    {
        EventBus<HealthUpdateEvent>.Raise(new HealthUpdateEvent(lives - (e as EnemyReachedGoalEvent).enemy.Damage));
    }

    private void OnPurchase(Event e)
    {
        RemoveMoney((e as PurchaseEvent).money);
        EventBus<MoneyChangeEvent>.Raise(new MoneyChangeEvent(Money));
    }

    private void OnHealthUpdate(Event e)
    {
        lives = (e as HealthUpdateEvent).health;
        if (lives < 1)
        {
            isGameOver = true;
            playerWonGame = false;
            EventBus<GameOverEvent>.Raise(new GameOverEvent(playerWonGame));
        }
    }

    private void OnGameOverEvent(Event e)
    {
        isGameOver = true;
        playerWonGame = (e as GameOverEvent).playerWon;
    }
}