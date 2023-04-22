using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor.SceneTemplate;

public class EnemyMoneyDropped : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private RectTransform moneyTextPosition;
    [SerializeField] private Color textColor;


    private void Awake()
    {
        EventBus<EnemyKilledEvent>.Subscribe(OnEnemyDied);
    }

    private void OnDestroy()
    {
        EventBus<EnemyKilledEvent>.Unsubscribe(OnEnemyDied);
    }

    void OnEnemyDied(Event e)
    {
        EnemyKilledEvent enemy = e as EnemyKilledEvent;
        moneyText.text = enemy.enemy.Money.ToString();
        moneyText.color = textColor;

        moneyTextPosition.position = Camera.main.WorldToScreenPoint(enemy.enemy.transform.position);

        StopCoroutine(MakeInvisible(0.0f));
        StartCoroutine(MakeInvisible(0.01f));

    }

    IEnumerator MakeInvisible(float TimeStep)
    {
        while(moneyText.color.a > 0)
        {
            moneyText.color = new Color(moneyText.color.r, moneyText.color.g, moneyText.color.b, moneyText.color.a - TimeStep);
            yield return null;
        }
        //float i = 0;
        //while (i < time)
        //{
        //    i += Time.deltaTime;
        //    //moneyTextPosition.position = new Vector2(transform.position.x, transform.position.y - 0.1f);
        //    moneyText.color = new Color(moneyText.color.r, moneyText.color.g, moneyText.color.b, Mathf.Lerp(1, 0, Mathf.Clamp(i, 0, 1)));
        //    yield return null;
        //}

    }
}
