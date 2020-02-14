using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basic_Movement_Jump : MonoBehaviour
{
     public float speed = 12.0f;
     public float rotateSpeed = 12.0f;
     public float jumpSpeed = 5.0f;
     public float gravity = 10f;

     private Vector3 moveDirection = Vector3.zero;
     private CharacterController cc;


     void Start()
     {
          cc = GetComponent<CharacterController>();
     }


     void Update()
     {

          if (cc.isGrounded)
          {
               moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
               moveDirection = transform.TransformDirection(moveDirection);
               moveDirection *= speed;

               if (Input.GetKeyDown("space"))
               {
                    moveDirection.y = jumpSpeed;

               }
          }
          else
          {
               moveDirection = new Vector3(Input.GetAxis("Horizontal"), moveDirection.y, Input.GetAxis("Vertical"));
               moveDirection = transform.TransformDirection(moveDirection);
               moveDirection.x *= speed;
               moveDirection.z *= speed;
          }

          moveDirection.y -= gravity * Time.deltaTime;
          cc.Move(moveDirection * Time.deltaTime);
     }
}
