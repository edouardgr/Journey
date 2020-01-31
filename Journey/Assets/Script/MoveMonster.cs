using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MoveMonster : MonoBehaviour
{
    Rigidbody2D rb;
    BoxCollider2D bc;
    
    public bool facing_right = true;
    public float hor, vert;
    public Collider2D last_col;
    [Header("Controller")]
    public float deadzone = 0.5f;
    [Header("X Velocity")]
    public float curr_vel_x = 0f;
    public float max_vel_x = 7f;
    public float acc_vel_x = 2f;
    public float dec_vel_x = 3f;
    [Header("Y Velocity")]
    public float fall_gravity = 3f;
    public float jump_gravity = 2f;
    public float drop_down_time = 0.2f;
    public float drop_down_timer = 0f;
    Collider2D prev_oneWay;
    [Header("Jumping")]
    public bool groundCheck = false;
    public float jump_speed = 11f;
    public float fall_speed = 2.5f;
    public float extra_jump_time = 0.1f;
    public float extra_jump_timer = 0f;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
    }

    void FixedUpdate()
    {
        if (extra_jump_timer > 0f) {
            extra_jump_timer -= Time.deltaTime;
        }
        if(drop_down_timer > 0f) {
            drop_down_timer -= Time.deltaTime;
            if(drop_down_timer <= 0f) {
                Physics2D.IgnoreCollision(bc, prev_oneWay, false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Determine user input
        float input_x = Input.GetAxisRaw("Horizontal");
        float input_y = Input.GetAxisRaw("Vertical");
        hor = (Mathf.Abs(input_x) > deadzone ? (input_x > 0f ? 1f : -1f) : 0f);
        vert = (Mathf.Abs(input_y) > deadzone ? (input_y > 0f ? 1f : -1f) : 0f);

        //Player Movement
        if (hor != 0f) {
            curr_vel_x += acc_vel_x * hor;
            facing_right = curr_vel_x > 0;
        } else {
            if (Mathf.Abs(curr_vel_x) > dec_vel_x) {
                curr_vel_x -= dec_vel_x * (curr_vel_x > 0f ? 1f : -1f);
            } else {
                curr_vel_x = 0f;
            }
        }
        curr_vel_x = Mathf.Clamp(curr_vel_x, -max_vel_x, max_vel_x); //Limit speed

        //Jumping
        //Start cayote timer
        if (vert >= 0 && Input.GetButtonDown("Jump")) {
            extra_jump_timer = extra_jump_time;
        }

        //Drop down ledge
        if (vert < 0 && Input.GetButtonDown("Jump") && last_col != null) {
            Physics2D.IgnoreCollision(bc, last_col, true);
            prev_oneWay = last_col;
            drop_down_timer = drop_down_time;
            groundCheck = false;
        }

        //Active jump on button
        if (extra_jump_timer > 0f && groundCheck) {
            rb.velocity = new Vector2(rb.velocity.x, jump_speed);
            groundCheck = false;
        }
        //Change gravity for fall
        if (rb.velocity.y < 0) {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fall_speed - 1) * Time.deltaTime;
        } else if (rb.velocity.y > 0 && !Input.GetButton("Jump")) {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fall_speed - 1) * Time.deltaTime;
        }

        //Apply velocity
        rb.velocity = new Vector2(curr_vel_x, rb.velocity.y);

        //Set gravity
        rb.gravityScale = (rb.velocity.y > 0f ? jump_gravity : fall_gravity);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        for(int i = 0; i < collision.contactCount; i++) {
            if(collision.GetContact(i).collider.tag == "OneWay") {
                last_col = collision.GetContact(i).collider;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //GroundCheck
        for (int i = 0; i < collision.contactCount; i++) {
            if (collision.GetContact(i).normal.y > 0f) {
                if (!groundCheck && rb.velocity.y <= 0f) {
                    groundCheck = true;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        last_col = null;
    }
}
