using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement1 : MonoBehaviour
{
     [Header("Movement")]
     public float speed = 12.0f;
     public float jump_speed = 9.0f;
     public float gravity = 22.5f;
     public float acc_input_x = 0.2f, acc_input_z = 0.2f;
     public float curr_input_x = 0f, curr_input_z = 0f;
     
     
     [Header("Swim")]
     public float swim_speed = 12f;
     public float swim_gravity = 3f;

     bool is_in_water = false;

     

     [Header("Ignore for now")]
     public bool launch = false;
     public Vector3 move_direction = Vector3.zero;

     Vector3 move_direction_water = Vector3.zero;
     CharacterController CC;


     void Start()
     {
          CC = GetComponent<CharacterController>();
          Cursor.lockState = CursorLockMode.Locked;
     }

     void FixedUpdate()
     {
          // Get Horizontal and Vertical Input
          float horizontalInput = (Input.GetAxisRaw("Horizontal"));
          float verticalInput = (Input.GetAxisRaw("Vertical"));

          //Forward movement
          if (verticalInput != 0f)
          {
               curr_input_z += acc_input_z * verticalInput;
          }
          else
          {
               if (Mathf.Abs(curr_input_z) > acc_input_z)
               {
                    curr_input_z -= acc_input_z * (curr_input_z > 0f ? 1f : -1f);
               }
               else
               {
                    curr_input_z = 0f;
               }
          }
          curr_input_z = Mathf.Clamp(curr_input_z, -1, 1);

          //Sideways movement
          if (horizontalInput != 0f)
          {
               curr_input_x += acc_input_x * horizontalInput;
          }
          else
          {
               if (Mathf.Abs(curr_input_x) > acc_input_x)
               {
                    curr_input_x -= acc_input_x * (curr_input_x > 0f ? 1f : -1f);
               }
               else
               {
                    curr_input_x = 0f;
               }
          }
          curr_input_x = Mathf.Clamp(curr_input_x, -1, 1);
     }
     void Update()
     {
     
          if (is_in_water)
          {
               move_direction_water = Vector3.zero;
               move_direction_water +=  curr_input_z * transform.GetChild(0).forward * swim_speed;
               move_direction_water += curr_input_x * transform.GetChild(0).right * swim_speed;
               
               //if space is held, let player swim to surface vertically
               if (Input.GetKey("space"))
               {
                    move_direction_water += transform.GetChild(0).up *(swim_speed/2);

               } else move_direction_water.y -= swim_gravity;


               CC.Move(move_direction_water * Time.deltaTime);

          }
          else
          {

               if (CC.isGrounded && !launch)
               {

                    move_direction = new Vector3(curr_input_x, 0, curr_input_z);
                    move_direction = transform.TransformDirection(move_direction);
                    move_direction *= speed;

                    if (Input.GetKeyDown("space"))
                    {
                         move_direction.y = jump_speed;

                    }

               }
               else
               {
                    //player is in the air, so change launched to false, as they cant be launched mid-air
                    launch = false;
                    move_direction = new Vector3(curr_input_x, move_direction.y, curr_input_z);
                    move_direction = transform.TransformDirection(move_direction);
                    move_direction.x *= speed;
                    move_direction.z *= speed;
               }

               move_direction.y -= gravity * Time.deltaTime;
               CC.Move(move_direction * Time.deltaTime);
          }

     }

     //When user falls into some water
     private void OnTriggerStay(Collider other)
     {
          if (other.tag == "Water")
               is_in_water = true;

     }
     private void OnTriggerEnter(Collider other)
     {
        if(other.tag == "CheckPoint")
        { //GROSS
            GameObject.FindGameObjectWithTag("CheckPoint").transform.root.GetComponent<Checkpoint_master>().update_checkpoint();
        }

        if (other.tag == "Death") {
            transform.position = GameObject.FindGameObjectWithTag("CheckPoint").transform.root.GetComponent<Checkpoint_master>().respawn_point.position;
        } //TOO LONG
            

     }
    //When user jumps out of some water
    private void OnTriggerExit(Collider other)
     {
          if (other.tag == "Water") {
               is_in_water = false;

               //Makes jumping out of the water much smoother, less jagged
               if (Input.GetKey("space"))
               {
                    move_direction.y = jump_speed/2;

               }
          }
        
     }
}

