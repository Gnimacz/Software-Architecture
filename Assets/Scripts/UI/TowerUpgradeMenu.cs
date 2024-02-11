using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerUpgradeMenu : MonoBehaviour
{
    public Tile tile;
    private Tower tower;
    [SerializeField] private TextMeshProUGUI towerName;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private TextMeshProUGUI UpgradeButtonText;
    [SerializeField] private Button SellButton;
    [SerializeField] private TextMeshProUGUI SellButtonText;

    [SerializeField] private TextMeshProUGUI rangeText;
    [SerializeField] private TextMeshProUGUI typeText;
    [SerializeField] private TextMeshProUGUI fireRateText;

    private void Awake()
    {
        upgradeButton.onClick.AddListener(OnUpgradeClicked);
        SellButton.onClick.AddListener(OnSellClicked);

        EventBus<MoneyChangeEvent>.Subscribe(OnMoneyChange);
    }

    private void OnMoneyChange(MoneyChangeEvent @event)
    {
        if(tower.NextUpgrade != null && @event.money < tower.NextUpgrade.Cost)
        {
            upgradeButton.interactable = false;
        }
        else
        {
            upgradeButton.interactable = true;
        }
    }

    public void SetTowerVariables(Tile tile)
    {
        this.tile = tile;
        tower = tile.tower.GetComponent<Tower>();
        towerName.text = tower.name.Split('(')[0];
        rangeText.text = "Range: " + tower.Range;
        typeText.text = "Type: " + tower.TowerType;
        fireRateText.text = "Fire Rate: " + tower.AttackSpeed;

        if (tower.NextUpgrade == null)
        {
            upgradeButton.interactable = false;
            UpgradeButtonText.text = "Max Level";
        }
        else
        {
            upgradeButton.interactable = true;
            UpgradeButtonText.text = "Upgrade(" + tower.NextUpgrade.Cost + ")";
        }
    }

    private void OnDestroy()
    {
        EventBus<MoneyChangeEvent>.Unsubscribe(OnMoneyChange);
    }

    private void OnDisable()
    {
        EventBus<MoneyChangeEvent>.Unsubscribe(OnMoneyChange);
    }


    private void OnUpgradeClicked()
    {
        EventBus<TowerUpGradeEvent>.Raise(new TowerUpGradeEvent(tower));
        EventBus<PurchaseEvent>.Raise(new PurchaseEvent(tower.NextUpgrade.Cost));
        EventBus<OpenTowerUIEvent>.Raise(new OpenTowerUIEvent(tile, false));
    }

    private void OnSellClicked()
    {
        EventBus<TileUpdateEvent>.Raise(new TileUpdateEvent(tile, null));
        tile = null;
        tower = null;
        EventBus<OpenTowerUIEvent>.Raise(new OpenTowerUIEvent(null, false));
    }
}
