using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_move : MonoBehaviour
{
    CharacterController cc;
    Rigidbody rb;

    public float move_speed = 5f;

    // Start is called before the first frame update
    void Awake()
    {
        cc = this.GetComponent<CharacterController>();
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float hor = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(hor, 0, vert);
        cc.Move(move * Time.deltaTime * move_speed);
    }

    void FixedUpdate()
    {
        
    }
}
