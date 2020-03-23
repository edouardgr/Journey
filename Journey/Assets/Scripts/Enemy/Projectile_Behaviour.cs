using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Edouard*/
public class Projectile_Behaviour : MonoBehaviour
{
    public float speed; //Projectile speed
    public int damage; //Projectile damage
    public GameObject parent; //Reference to the sender object

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime; //Translate forward

        if(Mathf.Abs(transform.position.x) > 50 || Mathf.Abs(transform.position.y) > 50 || Mathf.Abs(transform.position.z) > 50) { //Destroy object if beyond 
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject == parent) { return; } //Don't react if projectile collides with sender

        if(collision.collider.GetComponent<Shootable>() != null) { //Check if collider has shootable interface
            collision.collider.GetComponent<Shootable>().Damage(damage, gameObject); //Damage the collided object
        }
        Destroy(gameObject); //Destroy object
    }
}
