using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffTower : Tower
{
    [Header("Draw Circle Settings")]
    [SerializeField] private int steps = 20;
    [SerializeField] private float range = 5f;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private SphereCollider targetCollider;
    [SerializeField] private float drawHeight = 0.1f;

    [Header("Attack Settings")]
    [SerializeField] private float damage = 0f;
    [SerializeField] private float attackSpeed = 1f;
    [SerializeField] private bool canAttack = true;

    [Header("Upgrade Settings")]
    [SerializeField] private GameObject nextUpgrade;
    [SerializeField] private int cost = 10;
    [SerializeField] private int sellValue = 5;

    #region protected variables
    protected override int Steps { get => steps; set => steps = value; }
    protected override float Range { get => range; set => range = value; }
    protected override LineRenderer LineRenderer { get => lineRenderer; }

    protected override SphereCollider TargetCollider { get => targetCollider; }
    protected override float DrawHeight { get => drawHeight; set => drawHeight = value; }

    protected override float Damage { get => damage; set => damage = value; }
    protected override float AttackSpeed { get => attackSpeed; set => attackSpeed = value; }
    protected override bool CanAttack { get => canAttack; set => canAttack = value; }

    protected override GameObject NextUpgrade { get => nextUpgrade; set => nextUpgrade = value; }
    protected override int Cost { get => cost; set => cost = value; }
    protected override int SellValue { get => sellValue; set => sellValue = value; }
    #endregion

    //variables for the AOI tower
    private List<IDamagable> m_List = new List<IDamagable>();
    public static event System.Action<IDamagable> hurtEnemy;

    protected override void OnTowerPlaced()
    {
        drawCircle(Steps, Range, LineRenderer, DrawHeight);
        TargetCollider.radius = Range;
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
    }

    private void Update()
    {
        //drawCircle(Steps, targetCollider.radius, LineRenderer, DrawHeight);
    }

    protected override IEnumerator Attack()
    {
        while (canAttack)
        {
            if (m_List.Count > 0)
            {
                for (int i = 0; i < m_List.Count; i++)
                {
                    IDamagable enemy = m_List[i];
                    enemy.TakeDamage(damage, IDamagable.DamageType.Debuff_Slow);
                    if (enemy.health <= 0)
                    {
                        m_List.Remove(enemy);
                    }
                }
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
                m_List.Add(enemy);
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
                m_List.Remove(enemy);
            }
        }
    }
}
