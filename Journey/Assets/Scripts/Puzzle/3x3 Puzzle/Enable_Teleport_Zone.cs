using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enable_Teleport_Zone : MonoBehaviour
{
     public BoxCollider Turn_Me_On;

     private void OnTriggerEnter(Collider other)
     {
          Turn_Me_On.enabled = true;
          GetComponent<BoxCollider>().enabled = false;
     }
}
