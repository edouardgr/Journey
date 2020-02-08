using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_movement : MonoBehaviour
{
    CharacterController cc;
    public float speed = 5f;

    // Start is called before the first frame update
    void Awake()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get Horizontal and Vertical Input
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        // Calculate the Direction to Move based on the tranform of the Player
        Vector3 moveDirectionForward = transform.forward * verticalInput;
        Vector3 moveDirectionSide = transform.right * horizontalInput;

        //find the direction
        Vector3 direction = (moveDirectionForward + moveDirectionSide).normalized;

        //find the distance
        Vector3 distance = direction * speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.LeftShift)) {
            distance *= speed;
        }

        //Gravity
        distance.y += Physics.gravity.y * Time.deltaTime;

        // Apply Movement to Player
        cc.Move(distance);
    }
}
