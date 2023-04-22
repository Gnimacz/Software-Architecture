using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerHandler : MonoBehaviour
{
    //TODO(check if you hit a tile, then check if it's occupied, then put down the currently selected tower)
    [SerializeField] private Camera mainCamera;

    bool isHovering = false;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private Tile GetTileAtMousePosition()
    {

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray);

        foreach (RaycastHit hit in hits)
        {
            if (!hit.collider.CompareTag("Tile")) continue;
            Tile tile = hit.collider.gameObject.GetComponent<Tile>();
            return tile;
        }


        return null;
    }
}
