using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class TowerUpgradeButton : MonoBehaviour
{
    [SerializeField] Canvas UICanvas;
    [SerializeField] Tower attachedTower;
    [SerializeField] TextMeshProUGUI upgradeCostText;
    [SerializeField] TextMeshProUGUI towerUpgradeCostText;
    [SerializeField] Button upgradeButton;

    private void Awake()
    {
        UICanvas.worldCamera = Camera.main;
        attachedTower = transform.parent.GetComponent<Tower>();
        if (attachedTower.NextUpgrade == null)
        {
            SetActive(false);
            return;
        }
    }

    private void OnEnable()
    {
        EventBus<MoneyChangeEvent>.Subscribe(OnMoneyChangeEvent);
        EventBus<WaveStatusUpdate>.Subscribe(OnWaveStatusChanged);
        UpdateUI();
    }

    private void OnDisable()
    {
        EventBus<MoneyChangeEvent>.Unsubscribe(OnMoneyChangeEvent);
        EventBus<WaveStatusUpdate>.Unsubscribe(OnWaveStatusChanged);
    }

    public void SetAttachedTower(Tower tower)
    {
        attachedTower = tower;
        if (attachedTower.NextUpgrade == null)
        {
            SetActive(false);
            return;
        }
        UpdateUI();
    }

    public void UpdateUI()
    {
        upgradeCostText.text = attachedTower.NextUpgrade.Cost.ToString();
        if (attachedTower.NextUpgrade.Cost > GameManager.Instance.money)
        {
            upgradeButton.interactable = false;
            upgradeButton.image.color = Color.red;
        }
        else
        {
            upgradeButton.interactable = true;
            upgradeButton.image.color = Color.white;
        }
    }

    void OnMoneyChangeEvent(Event e)
    {
        UpdateUI();
    }

    public void UpgradeTower()
    {
        if (attachedTower.NextUpgrade == null)
        {
            SetActive(false);
            return;
        }
        EventBus<TowerUpGradeEvent>.Raise(new TowerUpGradeEvent(attachedTower));
        EventBus<PurchaseEvent>.Raise(new PurchaseEvent(attachedTower.NextUpgrade.Cost));
        UpdateUI();
    }

    void OnWaveStatusChanged(Event e)
    {
        SetActive((e as WaveStatusUpdate).isPaused);
    }

    public void SetActive(bool active)
    {
        UICanvas.enabled = active;
    }

    public Tower GetAttachedTower()
    {
        return attachedTower;
    }



}
