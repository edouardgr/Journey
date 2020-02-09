using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_movement : MonoBehaviour
{
    CharacterController cc;
    //Maximum movement speed
    public float max_speed = 5f;
    //Vertical and Horizontal movement
    public float acc_input_x = 0.1f, acc_input_z = 0.1f;
    public float curr_input_x = 0f, curr_input_z = 0f;

    // Start is called before the first frame update
    void Awake()
    {
        cc = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
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

        // Calculate the Direction to Move based on the tranform of the Player
        Vector3 moveDirectionForward = transform.forward * curr_input_z * max_speed;
        Vector3 moveDirectionSide = transform.right * curr_input_x * max_speed;

        //find the direction
        Vector3 direction = (moveDirectionForward + moveDirectionSide);

        //find the distance
        Vector3 distance = direction * Time.deltaTime;

        //Gravity
        distance.y += Physics.gravity.y * Time.deltaTime;

        // Apply Movement to Player
        cc.Move(distance);
    }
}
