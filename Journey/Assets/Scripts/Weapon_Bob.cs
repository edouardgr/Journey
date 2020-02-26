using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Bob : MonoBehaviour
{
     public float Amount;
     public float Max_Amount;

     private float Horizontal;
     private float Verticle;
     private float Wavesplice;


     //Initial position of weapon
     private Vector3 Initial_Position;


     void Start()
     { 
          Initial_Position = transform.localPosition;

     }

    // Update is called once per frame
    void Update()
    {
          Horizontal = Input.GetAxis("Horizontal");
          Verticle = Input.GetAxis("Verticle");


     }
}
