using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuyMenuGenerator : MonoBehaviour
{
    [SerializeField] private GameObject towerButtonPrefab;
    private TowerBuyButtonController towerBuyButtonController;
    void Awake()
    {
        foreach (GameObject tower in WaveManager.Instance.waves[WaveManager.Instance.currentWave].availableTowers)
        {
            GameObject towerButton = Instantiate(towerButtonPrefab);
            TowerBuyButtonController towerBuyButtonController = towerButton.GetComponent<TowerBuyButtonController>();
            towerBuyButtonController.towerObject = tower;
            towerButton.transform.SetParent(this.transform);
            Debug.Log(tower);
            Debug.Log(towerBuyButtonController.towerObject);
            towerButton.GetComponent<TowerBuyButtonController>().Setup();
        }
    }
}
