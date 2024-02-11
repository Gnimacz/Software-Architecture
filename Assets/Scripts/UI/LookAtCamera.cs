using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is responsible for making the object look at the main camera.
/// Its main use is for the UI elements to always face the camera. Like the enemy health bar. However it can be used for other things as well.
/// </summary>
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
