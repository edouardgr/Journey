using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement1: MonoBehaviour
{
 
      float speed = 12.0f;
      float jump_speed = 9.0f;
      float gravity = 20.0f;
     
      float swim_speed = 12f;
      float swim_gravity = 3f;
      bool is_in_water = false;

      Vector3 move_direction = Vector3.zero;
      Vector3 move_direction_water = Vector3.zero;
      CharacterController CC;


     void Start()
     {
          CC = GetComponent<CharacterController>();
          Cursor.lockState = CursorLockMode.Locked;
     }


     void Update()
     {
          if (is_in_water) {
               move_direction_water = Vector3.zero;
               move_direction_water += Input.GetAxis("Vertical") * transform.GetChild(0).forward * swim_speed;
               move_direction_water += Input.GetAxis("Horizontal") * transform.GetChild(0).right * swim_speed;

               move_direction_water.y -= swim_gravity;

               CC.Move(move_direction_water * Time.deltaTime);

          }
          else
          {

               if (CC.isGrounded)
               {

                    move_direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                    move_direction = transform.TransformDirection(move_direction);
                    move_direction *= speed;

                    if (Input.GetKeyDown("space"))
                    {
                         move_direction.y = jump_speed;

                    }
               }
               else
               {
                    move_direction = new Vector3(Input.GetAxis("Horizontal"), move_direction.y, Input.GetAxis("Vertical"));
                    move_direction = transform.TransformDirection(move_direction);
                    move_direction.x *= speed;
                    move_direction.z *= speed;
               }

               move_direction.y -= gravity * Time.deltaTime;
               CC.Move(move_direction * Time.deltaTime);
          }
   
     }

     //When user falls into some WHATAH
     private void OnTriggerStay(Collider other)
     {
          if (other.tag == "Water")
               is_in_water = true;
          
     }
     //When user jumps out of some WHATAH
     private void OnTriggerExit(Collider other)
     {
          if(other.tag == "Water")
               is_in_water = false;
          
     }
}
