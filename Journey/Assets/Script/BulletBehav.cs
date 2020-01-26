using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehav : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger: " + collision.transform.name);
        Interactable inter = collision.gameObject.GetComponent<Interactable>();
        if (inter != null && collision.tag == "Interact") {
            inter.Toggle();
            Destroy(this.gameObject);
        }
    }
}
