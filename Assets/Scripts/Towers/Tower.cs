using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Tower : MonoBehaviour
{
    protected abstract int Steps { get; set; }
    public abstract float Range { get; set; }
    protected abstract LineRenderer LineRenderer { get; }
    protected abstract SphereCollider TargetCollider { get; }
    protected abstract float DrawHeight { get; set; }

    public abstract float Damage { get; set; }
    public abstract float AttackSpeed { get; set; }
    protected abstract bool CanAttack { get; set; }
    protected abstract List<IDamagable> TargetEnemies { get; set; }

    public abstract Tower NextUpgrade { get; set; }
    public abstract int Cost { get; set; }
    public abstract int SellValue { get; set; }
    public abstract string TowerType { get; }

    protected abstract GameObject AttackEffect { get; }
    protected abstract Transform AttackEffectTransform { get; }

    protected abstract IEnumerator Attack();
    protected abstract void OnTowerPlaced();
    protected abstract void OnTowerUpgrade(Event e);

    protected void drawCircle(int steps, float radius, LineRenderer lineRenderer, float drawHeight)
    {
        lineRenderer.positionCount = steps + 1;
        lineRenderer.useWorldSpace = false;
        float x;
        float y = drawHeight;
        float z;

        float angle = 20f;

        for (int i = 0; i < (steps + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            z = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;

            lineRenderer.SetPosition(i, new Vector3(x, y, z));

            angle += 360f / steps;
        }
    }
}
