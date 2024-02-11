using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TowerBuyButtonController : MonoBehaviour
{
    public GameObject towerObject;
    private Tower tower;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI towerName;
    [SerializeField] private Button buyButton;

    public void Setup(){
        EventBus<MoneyChangeEvent>.Subscribe(OnMoneyChange);
        EventBus<WavePauseUpdate>.Subscribe(OnWaveStateUpdate);

        Debug.Log(towerObject);
        tower = towerObject.GetComponent<Tower>();
        costText.text = tower.Cost.ToString();
        towerName.text = towerObject.name;
        buyButton.onClick.AddListener(OnClick);
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
            buyButton.interactable = false;
        }
        else
        {
            buyButton.interactable = true;
        }
    }

    void SetActive(bool active)
    {
        buyButton.interactable = active;
    }

    void OnClick()
    {
        EventBus<TowerSelectedEvent>.Raise(new TowerSelectedEvent(towerObject));
    }
}
