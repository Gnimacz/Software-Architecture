using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] public bool hasTower { get; set; } = false;
    [SerializeField] public GameObject tower { get; set; } = null;
    private Tower towerScript;
    private bool isHovered = false;

    private Vector3 originalScale = Vector3.one;

    public Vector3 GetOriginalScale() { return originalScale; }
    private void Awake()
    {
        originalScale = transform.localScale;
        EventBus<TileHoverEvent>.Subscribe(OnTileHover);
        EventBus<TileUpdateEvent>.Subscribe(OnTileUpdate);
    }
    private void OnDestroy()
    {
        EventBus<TileHoverEvent>.Unsubscribe(OnTileHover);
        EventBus<TileUpdateEvent>.Unsubscribe(OnTileUpdate);
    }

    private void Update()
    {
        if (transform.localScale != originalScale && !isHovered)
        {
            transform.localScale = originalScale;
        }
        else if (isHovered)
        {
            transform.localScale = originalScale * 1.2f;
        }
        isHovered = false;
    }

    private void OnTileHover(Event e)
    {
        TileHoverEvent tileHoverEvent = e as TileHoverEvent;
        if (tileHoverEvent.tile == this)
        {
            isHovered = true;
        }
    }
    private void OnTileUpdate(Event e)
    {
        TileUpdateEvent tileUpdateEvent = e as TileUpdateEvent;
        if (tileUpdateEvent.tile == this && tileUpdateEvent.tower == null)
        {
            hasTower = false;
            //EventBus<MoneyChangeEvent>.Raise(new MoneyChangeEvent(GameManager.Instance.money + towerScript.SellValue));
            GameManager.Instance.AddMoney(towerScript.SellValue);
            Destroy(tower);
            tower = null;
            towerScript = null;
            return;
        }
        else if(tileUpdateEvent.tile == this)
        {
            tower = Instantiate(tileUpdateEvent.tower, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
            towerScript = tower.GetComponent<Tower>();
            hasTower = true;
            EventBus<PurchaseEvent>.Raise(new PurchaseEvent(towerScript.Cost));
        }

    }
}
