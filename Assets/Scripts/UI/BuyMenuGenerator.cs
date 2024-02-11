using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// This class is responsible for generating the buy menu.
/// It is used to display the available towers for the player to buy.
/// It looks at the available towers in the current wave and creates a button for each tower.
/// </summary>
public class BuyMenuGenerator : MonoBehaviour
{
    [SerializeField] private GameObject towerButtonPrefab;
    void Awake()
    {
        foreach (GameObject tower in WaveManager.Instance.waves[WaveManager.Instance.currentWave].availableTowers)
        {
            GameObject towerButton = Instantiate(towerButtonPrefab);
            TowerBuyButtonController towerBuyButtonController = towerButton.GetComponent<TowerBuyButtonController>();
            towerBuyButtonController.towerObject = tower;
            towerButton.transform.SetParent(this.transform);
            towerButton.GetComponent<TowerBuyButtonController>().Setup();
        }
    }
}
