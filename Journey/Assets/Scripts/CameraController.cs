using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

     public float HorizontalSpeed = 4.0f; // horizontal mouse camera speed, can be changed to be variable
     public float VerticleSpeed = 4.0f; // verticle mouse camera speed, can be changed to be variable

     private float yaw = 0.0f;
     private float pitch = 0.0f;


    // Update is called once per frame
    void Update()
    {
          yaw += HorizontalSpeed * Input.GetAxis("Mouse X");
          pitch -= VerticleSpeed * Input.GetAxis("Mouse Y");

          transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
    }
}
