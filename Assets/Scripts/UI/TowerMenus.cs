using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is responsible for the tower menus.
/// It is used to display the tower buy and upgrade menus.
/// It also controls when the menus should be displayed during gameplay.
/// </summary>
public class TowerMenus : MonoBehaviour
{
    [SerializeField] private GameObject towerBuyMenu;
    [SerializeField] private GameObject towerUpgradeMenu;

    void Awake()
    {
        EventBus<OpenTowerUIEvent>.Subscribe(OnTowerClicked);
        EventBus<WavePauseUpdate>.Subscribe(OnWaveUpdate);
        towerUpgradeMenu.SetActive(false);
    }

    

    private void OnDisable()
    {
        EventBus<OpenTowerUIEvent>.Unsubscribe(OnTowerClicked);
        EventBus<WavePauseUpdate>.Unsubscribe(OnWaveUpdate);
    }

    private void OnDestroy()
    {
        EventBus<OpenTowerUIEvent>.Unsubscribe(OnTowerClicked);
        EventBus<WavePauseUpdate>.Unsubscribe(OnWaveUpdate);
    }

    private void OnWaveUpdate(WavePauseUpdate update)
    {
        if (!update.isPaused)
        {
            towerBuyMenu.SetActive(false);
            towerUpgradeMenu.SetActive(false);
        }
        else
        {
            if(!towerUpgradeMenu.activeSelf) towerBuyMenu.SetActive(true);
        }
    }

    private void OnTowerClicked(OpenTowerUIEvent @event)
    {
        if (!@event.shouldOpen)
        {
            towerBuyMenu.SetActive(true);
            towerUpgradeMenu.SetActive(false);
            return;
        }

        if (@event.tile.tower != null && @event.shouldOpen && @event.tile != null)
        {
            towerBuyMenu.SetActive(false);
            towerUpgradeMenu.SetActive(true);
            towerUpgradeMenu.GetComponent<TowerUpgradeMenu>().SetTowerVariables(@event.tile);
        }
        else
        {
            towerBuyMenu.SetActive(true);
            towerUpgradeMenu.SetActive(false);
        }
    }
}
