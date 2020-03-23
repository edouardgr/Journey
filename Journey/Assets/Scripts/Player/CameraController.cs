using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Brain*/
public class CameraController : MonoBehaviour
{
     public float HorizontalSpeed = 4.0f;
     public float VerticalSpeed = 4.0f;

     public float pitch_limit = 60f;

     private float yaw = 0.0f;
     private float pitch = 0.0f;
    // Update is called once per frame
    void Update()
    {
          yaw += HorizontalSpeed * Input.GetAxis("Mouse X");
          pitch -= VerticalSpeed * Input.GetAxis("Mouse Y");
          
          //Limit pitch from going upside down
          pitch = Mathf.Clamp(pitch, -pitch_limit, pitch_limit);

          //Change the y rotation of the whole player entity
          transform.eulerAngles = new Vector3(0.0f, yaw, 0.0f);
          //Allow player to look up and down
          transform.GetChild(0).localEulerAngles = new Vector3(pitch, 0.0f, 0.0f);
    }
}
