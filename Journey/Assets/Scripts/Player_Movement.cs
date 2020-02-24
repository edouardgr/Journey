﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Edouard & Brain*/
public class Player_Movement : MonoBehaviour
{
    CharacterController cc;
    public float cayote_time = 0.2f; //Cayote time
    float time = 0f; //Timer
    bool fall_from_ledge = false, fall_charge = false; //Variables related to cayote time

    //Maximum movement speed
    public float max_speed = 12f;

    //Vertical and Horizontal movement
    public float acc_input_x = 0.2f, acc_input_z = 0.2f;
    public float curr_input_x = 0f, curr_input_z = 0f;
    float y_velocity = 0f;
    public float y_gravity = 0.8f;
    public float jump_velocity = 0.3f;
    public float groundCheck_dist = 1.2f;

    //Swimming
    public bool in_water = false;

    // Start is called before the first frame update
    void Awake()
    {
        cc = GetComponent<CharacterController>(); //Get character controller
        Cursor.lockState = CursorLockMode.Locked; //Removes mouse from the screen   
    }

    void FixedUpdate()
    {
        // Get Horizontal and Vertical Input
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        //Forward movement
        if (verticalInput != 0f) {
            curr_input_z += acc_input_z * verticalInput;
        } else {
            if (Mathf.Abs(curr_input_z) > acc_input_z) {
                curr_input_z -= acc_input_z * (curr_input_z > 0f ? 1f : -1f);
            } else {
                curr_input_z = 0f;
            }
        }
        curr_input_z = Mathf.Clamp(curr_input_z, -1, 1);

        //Sideways movement
        if (horizontalInput != 0f) {
            curr_input_x += acc_input_x * horizontalInput;
        } else {
            if (Mathf.Abs(curr_input_x) > acc_input_x) {
                curr_input_x -= acc_input_x * (curr_input_x > 0f ? 1f : -1f);
            } else {
                curr_input_x = 0f;
            }
        }
        curr_input_x = Mathf.Clamp(curr_input_x, -1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        //Cayote time
        if(time >= 0) { time -= Time.deltaTime; } //Countdown

        //GroundCheck
        bool groundCheck = false;
        RaycastHit ground;
        if(Physics.Raycast(transform.position, -transform.up, out ground)) { //Shoot ray down to ground
            if (ground.distance < groundCheck_dist) { //Distance requirement to enable jumps
                groundCheck = true;
                fall_from_ledge = true;
                fall_charge = false;
            }
        }

        //Vertical Velocity
        if (groundCheck) {
            y_velocity = 0;
        } else {
            y_velocity -= y_gravity * Time.deltaTime;
        }

        if (in_water) { //Water movement
            Vector3 dir = (transform.GetChild(0).forward * curr_input_z * 0.1f) + (transform.GetChild(0).right * curr_input_x * 0.1f);
            if(Input.GetKey(KeyCode.Space)) {
                dir += transform.GetChild(0).up * 0.1f;
            } else {
                dir -= transform.up * 0.01f;
            }
            cc.Move(dir);
        } else {
            //Cayote Time - Period of time that lets you jump early
            if(time > 0 && groundCheck) {
                y_velocity = jump_velocity;
                fall_from_ledge = false;
            } else if(!fall_charge && !groundCheck && fall_from_ledge && time <= 0) { //Cayote jump for falling
                time = cayote_time;
                fall_charge = true;
            }

            //Jumping
            if (Input.GetKeyDown(KeyCode.Space)) {
                Debug.Log(fall_from_ledge + ", " + (time > 0));
                if (groundCheck || (fall_from_ledge && time > 0)) {
                    y_velocity = jump_velocity; //Apply jump
                    fall_from_ledge = false;
                } else if (!groundCheck && time <= 0) {
                    time = cayote_time; //Set Cayote time
                }
            }

            // Calculate the Direction to Move based on the tranform of the Player
            Vector3 moveDirectionForward = transform.forward * curr_input_z * max_speed; //Forward
            Vector3 moveDirectionSide = transform.right * curr_input_x * max_speed; //Sideways
            Vector3 direction = (moveDirectionForward + moveDirectionSide); //Add directions together
            Vector3 distance = direction * Time.deltaTime; //Apply deltatime
            distance.y = y_velocity; //Apply Gravity
            cc.Move(distance); // Apply Movement to Player
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Water") { in_water = true; } //Enter water
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Water") { in_water = false; } //Exit water
    }
}
