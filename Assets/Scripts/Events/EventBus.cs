using System;
using UnityEngine.Rendering;

public abstract class Event { }

public class EventBus<Event>
{
    private static event Action<Event> EventRaised;
    public static void Subscribe(Action<Event> action)
    {
        EventRaised += action;
    }
    public static void Unsubscribe(Action<Event> action)
    {
        EventRaised -= action;
    }
    public static void Raise(Event action)
    {
        EventRaised?.Invoke(action);
    }
}

public class TowerPlacedEvent : Event
{
    public Tower tower;

    /// <summary>
    /// Called when a tower is placed down.
    /// </summary>
    /// <param name="tower">The tower that was placed down</param>
    public TowerPlacedEvent(Tower tower)
    {
        this.tower = tower;
    }
}


public class EnemyKilledEvent : Event
{
    public BaseEnemy enemy;

    /// <summary>
    /// Called when an enemy is killed.
    /// </summary>
    /// <param name="enemy">The enemy that was killed</param>
    public EnemyKilledEvent(BaseEnemy enemy)
    {
        this.enemy = enemy;
    }
}

public class MoneyChangeEvent : Event
{
    public int money;

    /// <summary>
    /// Called when the amount of money a player has changes.
    /// </summary>
    /// <param name="money">The new amount of money.</param>
    public MoneyChangeEvent(int money)
    {
        this.money = money;
    }
}

public class PurchaseEvent : Event
{
    public int money;

    /// <summary>
    /// Called when a player makes a purchase or money decreases
    /// </summary>
    /// <param name="money">The amount of money to remove</param>
    public PurchaseEvent(int money)
    {
        this.money = money;
    }
}

public class EnemyReachedGoalEvent : Event
{
    public BaseEnemy enemy;

    /// <summary>
    /// Called when an enemy reaches and touches the goal position
    /// </summary>
    /// <param name="enemy">The enemy that touched the goal position</param>
    public EnemyReachedGoalEvent(BaseEnemy enemy)
    {
        this.enemy = enemy;
    }
}

public class HealthUpdateEvent : Event
{
    public int health;
    /// <summary>
    /// Called when the amount of health the player has changes
    /// </summary>
    /// <param name="health">The new health value of the player</param>
    public HealthUpdateEvent(int health)
    {
        this.health = health;
    }
}

public class WaveStatusUpdate : Event
{
    public bool isPaused;
    public int pauseTime = 0;

    /// <summary>
    /// Called when a wave needs to be paused or unpaused for a certain amount of time
    /// </summary>
    /// <param name="isPaused">Whether a wave is paused or not</param>
    /// <param name="pauseTime">How long until the next wave</param>
    public WaveStatusUpdate(bool isPaused, int pauseTime)
    {
        this.isPaused = isPaused;
        this.pauseTime = pauseTime;
    }
}
public class GameOverEvent : Event
{
    public bool playerWon;
    public GameOverEvent(bool playerWon)
    {
        this.playerWon = playerWon;
    }
}

public class TowerUpGradeEvent : Event
{

    public Tower tower;
    public TowerUpGradeEvent(Tower tower)
    {
        this.tower = tower;

    }
}