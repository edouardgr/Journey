using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
     //grab the transform of where you want to teleport player, and the player themselves
     public Transform teleportTarget;
     public GameObject thePlayer;
     
     private void OnTriggerEnter(Collider other)
     {
          //when they enter the box collider, teleport them, then disable the box collider so they can get back
          thePlayer.transform.position = teleportTarget.transform.position;
          GetComponent<BoxCollider>().enabled = false;
     }
}