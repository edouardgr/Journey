using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glock : MonoBehaviour
{
  
    //Private Variables 
    private float _canfire = -1f;//used as comparasion for when the bullet can fire 
    private float _firerate = 0.05f;//used for delay until the player can fire again
    public GameObject _bulletPrefab;

    // Start is called before the first frame update
    void Start()
    {

    }

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

    }
}
