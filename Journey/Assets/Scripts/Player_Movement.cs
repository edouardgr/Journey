using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Edouard & Brain*/
public class Player_Movement : MonoBehaviour
{
    CharacterController cc;
    //Maximum movement speed
    public float max_speed = 5f;
    //Vertical and Horizontal movement
    public float acc_input_x = 0.1f, acc_input_z = 0.1f;
    public float curr_input_x = 0f, curr_input_z = 0f;
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
        if (in_water) {
            Vector3 dir = (transform.GetChild(0).forward * curr_input_z * 0.1f) + (transform.GetChild(0).right * curr_input_x * 0.1f);
            if(Input.GetKey(KeyCode.Space)) {
                dir += transform.GetChild(0).up * 0.1f;
            } else {
                dir -= transform.up * 0.01f;
            }
            cc.Move(dir);
        } else {
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

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Water") { in_water = true; }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Water") { in_water = false; }
    }
}
