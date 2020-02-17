using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Behaviour : MonoBehaviour
{
    public float speed;
    public int damage;
    public GameObject parent;

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;

        if(Mathf.Abs(transform.position.x) > 50 || Mathf.Abs(transform.position.y) > 50 || Mathf.Abs(transform.position.z) > 50) {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject == parent) { return; }

        if(collision.collider.GetComponent<Shootable>() != null) {
            collision.collider.GetComponent<Shootable>().Damage(damage, gameObject);
        }
        Destroy(gameObject);
    }
}
