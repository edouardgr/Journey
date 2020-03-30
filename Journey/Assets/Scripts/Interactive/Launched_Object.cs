using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launched_Object : MonoBehaviour
{    //Brian
     //basically, apply this script to anything that has a rigidbody and that you want to launch up into the air

     Rigidbody Rb;
     bool launched = false;
     public float Launch_Force = 100f;
     void Start()
     {
          Rb = GetComponent<Rigidbody>();
     }

     // Update is called once per frame
     void Update()
     {
          if (launched)
          {
               Rb.AddForce(new Vector3(0, Launch_Force, 0));
          }
     }
     //if the object collides with the launch pad tag, change the boolean to true and 
     //launch the object. If not, set the boolean to false
     private void OnTriggerExit(Collider other)
     {

          if (other.tag == "Launch Pad")
               launched = false;

     }
     private void OnTriggerEnter(Collider other)
     {
          if (other.tag == "Launch Pad")
               launched = true;
     }
}
