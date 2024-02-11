using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Experimental.GlobalIllumination;

/// <summary>
/// This class is responsible for the enemy health bar.
/// It is used to update the health bar when the enemy takes damage.
/// </summary>
public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private BaseEnemy enemy;
    [SerializeField] private Image healthBar;

    [SerializeField] private Color healthyColor;
    [SerializeField] private Color damagedColor;
    [SerializeField] private Color dyingColor;

    [SerializeField] private Vector2 colorThreshholds = new Vector2(0.75f, 0.5f);
    
    private void Awake()
    {
        EventBus<EnemyHurtEvent>.Subscribe(OnEnemyHurt);
    }

    private void OnDestroy()
    {
        EventBus<EnemyHurtEvent>.Unsubscribe(OnEnemyHurt);
    }

    void OnEnemyHurt(EnemyHurtEvent e)
    {
        EnemyHurtEvent enemyHurtEvent = e as EnemyHurtEvent;

        if (enemyHurtEvent.enemy == enemy)
        {
            enemy = enemyHurtEvent.enemy;
            float healthPercentage = enemy.Health / enemy.MaxHealth;
            //transform.localScale = new Vector3(healthPercentage, 1, 1);
            healthBar.rectTransform.sizeDelta = new Vector2(healthPercentage, healthBar.rectTransform.sizeDelta.y);
            if (healthPercentage > colorThreshholds.x)
            {
                healthBar.color = healthyColor;
            }
            else if (healthPercentage > colorThreshholds.y)
            {
                healthBar.color = damagedColor;
            }
            else
            {
                healthBar.color = dyingColor;
            }
        }
    }
}
