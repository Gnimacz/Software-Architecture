using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] public bool hasTower { get; set; } = false;
    [SerializeField] public GameObject tower { get; set; } = null;
    [SerializeField] private GameObject tileMesh;
    private Tower towerScript;
    private bool isHovered = false;

    private Vector3 originalScale = Vector3.one;

    public Vector3 GetOriginalScale() { return originalScale; }
    private void Awake()
    {
        originalScale = tileMesh.transform.localScale;
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
        if (tileMesh.transform.localScale != originalScale && !isHovered)
        {
            tileMesh.transform.localScale = originalScale;
        }
        else if (isHovered)
        {
            tileMesh.transform.localScale = originalScale * 1.2f;
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
            tower = Instantiate(tileUpdateEvent.tower, tileMesh.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
            towerScript = tower.GetComponent<Tower>();
            hasTower = true;
            tower.transform.parent = transform;
            EventBus<PurchaseEvent>.Raise(new PurchaseEvent(towerScript.Cost));
        }

    }
}
