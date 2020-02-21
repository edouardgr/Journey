using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Sway : MonoBehaviour
{
     //How much sway to put in
     public float Amount = 0.2f;

     //How far gun can sway based on mouse movement
     public float Max_Amount = 0.5f;

     //How much smoothing to be applied
     public float Smooth_Amount = 6;

     //Initial position of weapon
     private Vector3 Initial_Position;

     Animator anim;

     private void Start()
     {
          anim = GetComponent<Animator>();
          //Getting first position of weapon
          Initial_Position = transform.localPosition;
     }

     // Update is called once per frame
     void Update()
    {
          if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
          {
               anim.enabled = false;
          }
          else anim.enabled = true;

          float Movement_X = -Input.GetAxis("Mouse X") * Amount;
          float Movement_Y = -Input.GetAxis("Mouse Y") * Amount;
          Movement_X = Mathf.Clamp(Movement_X, -Max_Amount, Max_Amount);
          Movement_Y = Mathf.Clamp(Movement_Y, -Max_Amount, Max_Amount);

          Vector3 Final_Position = new Vector3(Movement_X, Movement_Y, 0);


          transform.localPosition = Vector3.Lerp(transform.localPosition, Final_Position+Initial_Position, Time.deltaTime * Smooth_Amount);

    }
}
