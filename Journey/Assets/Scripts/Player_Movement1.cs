using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement1: MonoBehaviour
{
 
     public float speed = 12.0f;
     public float jump_speed = 5.0f;
     public float gravity = 10.0f;
     
     public float swim_speed = 6.0f;
     public float swim_gravity = 5.0f;
     private bool is_in_water = false;

     private Vector3 move_direction = Vector3.zero;
     private Vector3 move_direction_y = Vector3.zero;
     private CharacterController CC;


     void Start()
     {
          CC = GetComponent<CharacterController>();
          Cursor.lockState = CursorLockMode.Locked;
     }


     void Update()
     {
          if (is_in_water) {
               if (Input.GetKeyDown("w"))
               {
                    move_direction = transform.GetChild(0).forward * swim_speed;

               }
               else move_direction = Vector3.zero;
              

               CC.Move(move_direction * Time.deltaTime);
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
     private void OnTriggerEnter(Collider other)
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
