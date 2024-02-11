using System;
using UnityEngine;
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

public class TileHoverEvent : Event
{
    public Tile tile;

    public TileHoverEvent(Tile tile)
    {
        this.tile = tile;
    }
}

public class TileUpdateEvent : Event
{
    public GameObject tower;
    public Tile tile;

    public TileUpdateEvent(Tile tile, GameObject tower)
    {
        this.tile = tile;
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

public class EnemyHurtEvent : Event
{
    public BaseEnemy enemy;
    public float damage;

    /// <summary>
    /// Called when an enemy is hurt. Could be used to play certain effects or sounds for example
    /// </summary>
    /// <param name="enemy">The enemy that was hurt</param>
    /// <param name="damage">The amount of damage the enemy took</param>
    public EnemyHurtEvent(BaseEnemy enemy, float damage)
    {
        this.enemy = enemy;
        this.damage = damage;
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

public class WavePauseUpdate : Event
{
    public bool isPaused;
    public int pauseTime = 0;

    /// <summary>
    /// Called when a wave is paused or unpaused. Gives the time until the next wave
    /// </summary>
    /// <param name="isPaused">Whether a wave is paused or not</param>
    /// <param name="pauseTime">How long until the next wave</param>
    public WavePauseUpdate(bool isPaused, int pauseTime)
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

public class OpenTowerUIEvent : Event
{
    public Tile tile;
    public bool shouldOpen;
    public OpenTowerUIEvent(Tile tile, bool shouldOpen = true)
    {
        this.tile = tile;
        this.shouldOpen = shouldOpen;
    }
}

public class TowerSelectedEvent : Event
{
    public GameObject tower;
    public TowerSelectedEvent(GameObject tower)
    {
        this.tower = tower;
    }
}