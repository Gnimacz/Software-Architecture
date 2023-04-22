using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] Vector3 offset = Vector3.zero;
    // Update is called once per frame
    void Update()
    {
        //Rotate to face the main camera
        transform.LookAt(Camera.main.transform.position + offset);
    }
}
