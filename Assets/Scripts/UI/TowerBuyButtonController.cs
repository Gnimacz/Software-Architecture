using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TowerBuyButtonController : MonoBehaviour
{
    [SerializeField] private Tower tower;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private Button buyButton;

    private void Awake()
    {
        costText.text = tower.Cost.ToString();

        EventBus<MoneyChangeEvent>.Subscribe(OnMoneyChange);
        EventBus<WavePauseUpdate>.Subscribe(OnWaveStateUpdate);
        CheckPrice();
    }

    private void OnDestroy()
    {
        EventBus<MoneyChangeEvent>.Unsubscribe(OnMoneyChange);
        EventBus<WavePauseUpdate>.Unsubscribe(OnWaveStateUpdate);
    }

    void OnMoneyChange(Event e)
    {
        MoneyChangeEvent moneyChangeEvent = e as MoneyChangeEvent;
        CheckPrice();

    }

    void OnWaveStateUpdate(Event e)
    {
        SetActive((e as WavePauseUpdate).isPaused);
    }

    void CheckPrice()
    {
        if (tower.Cost > GameManager.Instance.Money)
        {
            SetActive(false);
            buyButton.image.color = Color.red;
            costText.color = Color.red;
        }
        else
        {
            SetActive(true);
            buyButton.image.color = Color.white;
            costText.color = Color.black;
        }
    }

    void SetActive(bool active)
    {
        buyButton.interactable = active;
    }
}
