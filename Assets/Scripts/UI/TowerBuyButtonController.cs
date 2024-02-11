using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// This class is responsible for the tower buy button.
/// It is used to display the tower's cost and name and to check if the player can afford the tower.
/// </summary>
public class TowerBuyButtonController : MonoBehaviour
{
    public GameObject towerObject;
    private Tower tower;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI towerName;
    [SerializeField] private Button buyButton;

    public void Setup(){
        EventBus<MoneyChangeEvent>.Subscribe(OnMoneyChange);

        tower = towerObject.GetComponent<Tower>();
        costText.text = tower.Cost.ToString();
        towerName.text = towerObject.name;
        buyButton.onClick.AddListener(OnClick);
        CheckPrice();
    }

    private void OnDestroy()
    {
        EventBus<MoneyChangeEvent>.Unsubscribe(OnMoneyChange);
    }

    void OnMoneyChange(Event e)
    {
        MoneyChangeEvent moneyChangeEvent = e as MoneyChangeEvent;
        CheckPrice();

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
