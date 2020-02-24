using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Edouard & Brain*/
public class Player_Movement_Rigid : MonoBehaviour
{
    Rigidbody rb;
    //Maximum movement speed
    public float max_speed = 12f;
    //Vertical and Horizontal movement
    public float acc_input_x = 0.2f, acc_input_z = 0.2f;
    public float curr_input_x = 0f, curr_input_z = 0f;
    public float y_velocity = 0f;
    public float y_gravity = 0.8f;
    public float jump_velocity = 0.3f;
    public float groundCheck_dist = 1.2f;
    public Transform parent_obj;
    public bool groundCheck;
    //Swimming
    public bool in_water = false;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>(); //Get character controller
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
        groundCheck = false;
        RaycastHit ground;
        if(Physics.Raycast(transform.position, -transform.up, out ground)) {
            if (ground.distance < groundCheck_dist) {
                groundCheck = true;
            }
        }

        if (groundCheck && y_velocity <= 0) {
            y_velocity = 0;
            //if (parent_obj == null) { parent_obj = ground.collider.transform; }
            //transform.position = parent_obj.position;
            //transform.rotation = parent_obj.rotation;
        } else {
            y_velocity -= y_gravity * Time.deltaTime;
            //if (parent_obj != null) { parent_obj = null; }
        }

        if (in_water) { //Water movement
            Vector3 dir = (transform.GetChild(0).forward * curr_input_z * 0.1f) + (transform.GetChild(0).right * curr_input_x * 0.1f);
            if(Input.GetKey(KeyCode.Space)) {
                dir += transform.GetChild(0).up * 0.1f;
            } else {
                dir -= transform.up * 0.01f;
            }
            //cc.Move(dir);
        } else {
            //Jumping
            if(Input.GetKeyDown(KeyCode.Space) && groundCheck) {
                y_velocity = jump_velocity;
                groundCheck = false;
            }
            // Calculate the Direction to Move based on the tranform of the Player
            Vector3 moveDirectionForward = transform.forward * curr_input_z * max_speed; //Forward
            Vector3 moveDirectionSide = transform.right * curr_input_x * max_speed; //Sideways
            Vector3 direction = (moveDirectionForward + moveDirectionSide); //Add directions together
            rb.velocity = new Vector3(direction.x, y_velocity, direction.z); // Apply Movement to Player
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        transform.parent = collision.transform;
    }

    private void OnCollisionExit(Collision collision)
    {
        transform.parent = null;
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
