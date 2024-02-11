using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is responsible for the single target tower.
/// It is used to attack a single enemy at a time.
/// </summary>
public class SingleTargetTower : Tower
{
    [Header("Draw Circle Settings")]
    [SerializeField] private int steps = 20;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private SphereCollider targetCollider;
    [SerializeField] private float drawHeight = 0.1f;

    [Header("Attack Settings")]
    [SerializeField] public float damage = 1f;
    [SerializeField] public float range = 5f;
    [SerializeField] public float attackSpeed = 0.2f;
    [SerializeField] private bool canAttack = true;
    [SerializeField] private GameObject attackEffect;
    [SerializeField] private Transform attackEffectTransform;

    [Header("Upgrade Settings")]
    [SerializeField] private string towerThumbnail;
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

    public override float Damage { get => damage; set => damage = value; }
    public override float AttackSpeed { get => attackSpeed; set => attackSpeed = value; }
    protected override bool CanAttack { get => canAttack; set => canAttack = value; }

    public override string TowerType { get => towerThumbnail; }
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
        EventBus<EnemyReachedGoalEvent>.Subscribe(onEnemyReachedGoal);
        EventBus<TowerUpGradeEvent>.Subscribe(OnTowerUpgrade);
        EventBus<WavePauseUpdate>.Subscribe(OnWaveUpdate);
    }


    private void OnDestroy()
    {
        EventBus<EnemyKilledEvent>.Unsubscribe(onEnemyKilled);
        EventBus<EnemyReachedGoalEvent>.Unsubscribe(onEnemyReachedGoal);
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

            if (targetEnemies.Count > 0)
            {
                IDamagable enemy = targetEnemies[0];
                if (!enemy.IsAlive || enemy == null)
                {
                    targetEnemies.Remove(enemy);
                    continue;
                }
                enemy.TakeDamage(damage, IDamagable.DamageType.Physical);
                if(attackEffect && attackEffectTransform) Instantiate(attackEffect, attackEffectTransform.position, attackEffectTransform.rotation);
                if(attackEffect) Instantiate(attackEffect, enemy.ParentTransform.position, Quaternion.identity).transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);

            }
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
            }
        }
    }

    void onEnemyKilled(Event e)
    {
        EnemyKilledEvent enemyKilledEvent = (e as EnemyKilledEvent);
        if (!targetEnemies.Contains(enemyKilledEvent.enemy)) return;
        targetEnemies.Remove(enemyKilledEvent.enemy);
    }

    void onEnemyReachedGoal(Event e)
    {
        EnemyReachedGoalEvent enemyReachedGoalEvent = (e as EnemyReachedGoalEvent);
        if (!targetEnemies.Contains(enemyReachedGoalEvent.enemy)) return;
        targetEnemies.Remove(enemyReachedGoalEvent.enemy);
    }
}
