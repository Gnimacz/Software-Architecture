using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] public bool hasTower { get; set; } = false;

    private Vector3 originalScale = Vector3.one;
    private void Start()
    {
        originalScale = transform.localScale;
    }

    private void Update()
    {
        if (transform.localScale != originalScale)
        {
            transform.localScale = originalScale;
        }
    }
}
