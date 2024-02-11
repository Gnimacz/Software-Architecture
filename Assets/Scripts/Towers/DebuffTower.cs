using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// This class is responsible for the debuff tower.
/// It is used to apply a debuff to the enemies within its range.
/// You can easily change change the debuff type(if there were more than one) and add new ones.
/// </summary>
public class DebuffTower : Tower
{
    [Header("Draw Circle Settings")]
    [SerializeField] private int steps = 20;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private SphereCollider targetCollider;
    [SerializeField] private float drawHeight = 0.1f;

    [Header("Attack Settings")]
    [SerializeField] private IDamagable.Debuff debuff;
    [SerializeField] public float attackSpeed = 1f;
    [SerializeField] public float range = 5f;
    [SerializeField] private bool canAttack = true;
    [SerializeField] private GameObject attackEffect;
    [SerializeField] private GameObject enemyDebuffEffect;
    [SerializeField] private Transform attackEffectTransform;

    [Header("Upgrade Settings")]
    [SerializeField] private string towerType;
    [SerializeField] private Tower nextUpgrade;
    [SerializeField] private int cost = 10;
    [SerializeField] private int sellValue = 5;

    private List<IDamagable> targetEnemies = new List<IDamagable>();
    #region protected variables
    protected override int Steps { get => steps; set => steps = value; }
    public override float Range { get => range; set => range = value; }
    protected override LineRenderer LineRenderer { get => lineRenderer; }

    protected override SphereCollider TargetCollider { get => targetCollider; }
    protected override float DrawHeight { get => drawHeight; set => drawHeight = value; }

    public override float Damage { get => debuff.level; set => debuff.level = value; }
    public override float AttackSpeed { get => attackSpeed; set => attackSpeed = value; }
    protected override bool CanAttack { get => canAttack; set => canAttack = value; }

    public override string TowerType { get => towerType; }
    public override Tower NextUpgrade { get => nextUpgrade; set => nextUpgrade = value; }
    public override int Cost { get => cost; set => cost = value; }
    public override int SellValue { get => sellValue; set => sellValue = value; }
    protected override List<IDamagable> TargetEnemies { get => targetEnemies; set => targetEnemies = value; }
    protected override GameObject AttackEffect { get => attackEffect; }
    protected override Transform AttackEffectTransform { get => attackEffectTransform; }
    #endregion

    #region public getter methods
    public int GetCost()
    {
        return Cost;
    }
    public int GetSellValue()
    {
        return SellValue;
    }
    public Tower GetNextUpgrade()
    {
        return NextUpgrade;
    }
    #endregion

    protected override void OnTowerPlaced()
    {
        drawCircle(Steps, Range, LineRenderer, DrawHeight);
        TargetCollider.radius = Range;
    }

    protected override void OnTowerUpgrade(Event e)
    {
        if ((e as TowerUpGradeEvent).tower != this) return;
        if (NextUpgrade != null)
        {
            GameObject newTower = Instantiate(NextUpgrade.gameObject, transform.position, transform.rotation);
            newTower.transform.parent = transform.parent;
            newTower.transform.parent.GetComponent<Tile>().tower = newTower;
            Destroy(gameObject);
        }

    }

    private void Awake()
    {
        if (LineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }
        if (TargetCollider == null)
        {
            targetCollider = GetComponent<SphereCollider>();
        }

        StartCoroutine(Attack());
        OnTowerPlaced();

        EventBus<EnemyKilledEvent>.Subscribe(onEnemyKilled);
        EventBus<EnemyReachedGoalEvent>.Subscribe(OnEnemyReachedGoal);
        EventBus<TowerUpGradeEvent>.Subscribe(OnTowerUpgrade);
        EventBus<WavePauseUpdate>.Subscribe(OnWaveUpdate);
    }

    private void OnDestroy()
    {
        EventBus<EnemyKilledEvent>.Unsubscribe(onEnemyKilled);
        EventBus<EnemyReachedGoalEvent>.Unsubscribe(OnEnemyReachedGoal);
        EventBus<TowerUpGradeEvent>.Unsubscribe(OnTowerUpgrade);
        EventBus<WavePauseUpdate>.Unsubscribe(OnWaveUpdate);
    }

    private void OnWaveUpdate(WavePauseUpdate update)
    {
        if (!update.isPaused)
        {
            lineRenderer.enabled = false;
        }
        else
        {
            lineRenderer.enabled = true;
        }
    }


    protected override IEnumerator Attack()
    {
        while (canAttack && !GameManager.Instance.isGameOver)
        {
            foreach (IDamagable enemy in targetEnemies.ToList())
            {
                if (!enemy.IsAlive || enemy == null)
                {
                    targetEnemies.Remove(enemy);
                    continue;
                }
                enemy.AddDebuff(debuff);
                Instantiate(enemyDebuffEffect, enemy.ParentTransform.position, Quaternion.identity);
            }
            if(targetEnemies.Count > 0) Instantiate(AttackEffect, AttackEffectTransform.position, Quaternion.identity);
            yield return new WaitForSeconds(attackSpeed);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            IDamagable enemy = other.GetComponent<IDamagable>();
            if (enemy != null)
            {
                targetEnemies.Add(enemy);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            IDamagable enemy = other.GetComponent<IDamagable>();
            if (enemy != null)
            {
                targetEnemies.Remove(enemy);
                enemy.RemoveDebuff(debuff);
            }
        }
    }

    void onEnemyKilled(Event e)
    {
        EnemyKilledEvent enemyKilledEvent = (e as EnemyKilledEvent);
        if (!TargetEnemies.Contains(enemyKilledEvent.enemy)) return;
        targetEnemies.Remove(enemyKilledEvent.enemy);

    }
    void OnEnemyReachedGoal(Event e)
    {
        EnemyReachedGoalEvent enemyReachedGoalEvent = (e as EnemyReachedGoalEvent);
        if (!TargetEnemies.Contains(enemyReachedGoalEvent.enemy)) return;
        targetEnemies.Remove(enemyReachedGoalEvent.enemy);
    }

}
