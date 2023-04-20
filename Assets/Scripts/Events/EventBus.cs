using System;

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

    public TowerPlacedEvent(Tower tower)
    {
        this.tower = tower;
    }
}

public class EnemyKilledEvent : Event
{
    public BaseEnemy enemy;

    public EnemyKilledEvent(BaseEnemy enemy)
    {
        this.enemy = enemy;
    }
}