using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerBehav : MonoBehaviour
{
    public float spin_rate = 2f;
    LineRenderer lr;

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 2;
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
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, hit_up.point);

            if(hit_up.collider.tag == "Player") {
                hit_up.collider.GetComponent<MoveMonster>().Respawn();
            }
        }
    }
}
