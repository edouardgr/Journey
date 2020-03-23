using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable_Obj : MonoBehaviour
{
    Rigidbody rb;
    public Canvas hit_marker = null;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.GetComponentInParent<Shootable>() != null) {
            collision.collider.GetComponentInParent<Shootable>().Damage((int)rb.velocity.magnitude, null);
            hit_marker.GetComponent<Animator>().Play("Hit");
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (rb.velocity.magnitude < 2f) {
            Destroy(this);
        }
    }
}
