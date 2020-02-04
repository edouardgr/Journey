using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Properties : MonoBehaviour
{
    //public variables 
    public float speed = 5f;

    // Update is called once per frame
    void Update()
    {
        //makes the bullet go forward, on the z axis
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        //Allows the hierachy to be cleared up, 50 so it doesnt go on forever
        if (transform.position.z >= 50)
        {
            //Should delete off the hierarchy
            Destroy(gameObject);
        }

    }
}