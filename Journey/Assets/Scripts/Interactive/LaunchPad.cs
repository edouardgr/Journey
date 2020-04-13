using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchPad : MonoBehaviour
{
      public float Object_Launch_Force = 75f;
      public float Player_Launch_Force = 25f;
	  public AudioClip triggerSound;
	  private bool entered = true;
	  AudioSource audioSource;
	 void Start() {
		 audioSource = GetComponent<AudioSource>();
	 }
	 void Update() {
		 if (triggerSound != null) {
			audioSource.PlayOneShot(triggerSound, 0.7F);
		 } 
	 }
     private void OnTriggerEnter(Collider other)
     {
		  if (triggerSound != null && entered == true) {
				audioSource.PlayOneShot(triggerSound, 0.7F);
		  }
          if (other.tag == "Player") {

               other.GetComponent<Player_Movement1>().move_direction.y = Player_Launch_Force;
               other.GetComponent<Player_Movement1>().launch = true;

          }
		  
     }
     private void OnTriggerStay(Collider other)
     {
          if (other.tag == "Movable")
          {
               other.GetComponent<Rigidbody>().AddForce(new Vector3(0, Object_Launch_Force, 0));
          }
     }

}