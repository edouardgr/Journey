using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Aim : MonoBehaviour
{
  
    //Private Variables 
    private float _canfire = -1f;//used as comparasion for when the bullet can fire 
    private float _firerate = 0.05f;//used for delay until the player can fire again
    public GameObject _bulletPrefab;
    public GameObject point;

    // Update is called once per frame
    void Update()
    {
        //0 for left click, 1 for right click, 2 for middle button
        if (Input.GetMouseButtonDown(0))
        {
           //Debug.Log("Pressed primary button.");//Debug purpose so that it works
           //Time.time used for how long the game has been running
            _canfire = Time.time + _firerate; //Cool down system
            //Debug.Log("Space Key Pressed"); //Debug log to verify 
            Instantiate(_bulletPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity); //Clones the original and returns the clone 

            

        }

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            
            point.transform.position = hit.point;//makes ball go where ur looking at. visual aid
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);//Draws laaser from player to direction pointing
            Debug.Log("Did Hit");
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Did not Hit");
        }

    }
}
