using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerBehav : MonoBehaviour
{
    public float spin_rate = 2f;
    LineRenderer lr_up, lr_down;

    // Start is called before the first frame update
    void Start()
    {
        lr_up = transform.GetChild(0).GetComponent<LineRenderer>();
        lr_up.positionCount = 2;
        lr_down = transform.GetChild(1).GetComponent<LineRenderer>();
        lr_down.positionCount = 2;
    }

    private void FixedUpdate()
    {
        transform.Rotate(transform.forward, spin_rate);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit_up = Physics2D.Raycast(transform.position, transform.up);
        if(hit_up) {
            lr_up.SetPosition(0, transform.position);
            lr_up.SetPosition(1, hit_up.point);

            if(hit_up.collider.tag == "Player") {
                hit_up.collider.GetComponent<MoveMonster>().Respawn();
            }
        }

        RaycastHit2D hit_down = Physics2D.Raycast(transform.position, -transform.up);
        if (hit_down) {
            lr_down.SetPosition(0, transform.position);
            lr_down.SetPosition(1, hit_down.point);

            if (hit_down.collider.tag == "Player") {
                hit_down.collider.GetComponent<MoveMonster>().Respawn();
            }
        }
    }
}
