using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerHandler : MonoBehaviour
{
    //TODO(check if you hit a tile, then check if it's occupied, then put down the currently selected tower)
    [SerializeField] private Camera mainCamera;

    [SerializeField] private GameObject selectedTower = null;
    [SerializeField] private Tower selectedTowerScript = null;

    [SerializeField] bool canPlace = true;
    [SerializeField] private LayerMask tileLayerMask;
    private bool isHovering = false;

    private void Awake()
    {
        mainCamera = Camera.main;
        EventBus<WaveStatusUpdate>.Subscribe(OnWaveUpdate);
    }


    private void Update()
    {
        if (!canPlace) return;
        isHovering = false;
        TileHover();
        if (isHovering && selectedTower == null && Input.GetMouseButtonDown(0))
        {
            RemoveTowerAtTile(GetTileAtMousePosition());
        }
        if (selectedTower != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Tile tile = GetTileAtMousePosition();
                if (tile == null) return;
                PlaceTower(tile);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                selectedTower = null;
                selectedTowerScript = null;
            }
        }
    }


    private void OnWaveUpdate(Event e)
    {
        canPlace = (e as WaveStatusUpdate).isPaused;
    }

    private Tile GetTileAtMousePosition()
    {

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, tileLayerMask);

        foreach (RaycastHit hit in hits)
        {
            if (!hit.collider.CompareTag("Tile")) continue;
            Tile tile = hit.collider.gameObject.GetComponent<Tile>();
            Debug.Log("Tile found at mouse position");
            return tile;
        }

        Debug.Log("No Tile found at mouse position");
        return null;
    }

    void TileHover()
    {
        Tile hoveredTile = GetTileAtMousePosition();
        if (hoveredTile == null) return;
        //if (hoveredTile.hasTower) return;
        isHovering = true;
        EventBus<TileHoverEvent>.Raise(new TileHoverEvent(hoveredTile));
    }

    private void PlaceTower(Tile tile)
    {
        if (tile.hasTower) return;
        if (selectedTower == null) return;
        if (selectedTowerScript.Cost > GameManager.Instance.money) return;

        EventBus<TileUpdateEvent>.Raise(new TileUpdateEvent(tile, selectedTower));
        selectedTower = null;
        selectedTowerScript = null;
    }

    private void RemoveTowerAtTile(Tile tile)
    {
        if (tile.hasTower)
        {
            EventBus<TileUpdateEvent>.Raise(new TileUpdateEvent(tile, null));
        }
    }

    public void SelectTower(GameObject tower)
    {
        selectedTower = tower;
        selectedTowerScript = tower.GetComponent<Tower>();
    }
}
