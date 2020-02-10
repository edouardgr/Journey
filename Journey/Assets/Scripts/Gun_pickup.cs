﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Gun_pickup : MonoBehaviour
{
    public Object weapon;
    public float rotation_speed = 2f; //Speed at which the weapon will rotate
    Vector3 origin;
    public float frequency = 4f; //DISCUSS
    public float amplitude = 0.25f;

    // Start is called before the first frame update
    void Awake()
    {
        Instantiate(weapon, transform); //Create the gun object
        origin = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.GetChild(0).Rotate(new Vector3(0, rotation_speed, 0)); //Possibly add to fixedUpdate //Rotate the weapon to get players attention
        transform.GetChild(0).position = origin + new Vector3(0, Mathf.Sin(Time.time * frequency) * amplitude, 0);
    }

    private void OnDrawGizmos() //Used for visual reference
    {
        Handles.color = Color.green;
        Handles.DrawWireDisc(transform.position, Vector3.up, GetComponent<SphereCollider>().radius);
        Handles.DrawWireDisc(transform.position, Vector3.right, GetComponent<SphereCollider>().radius);
        Handles.DrawWireDisc(transform.position, Vector3.forward, GetComponent<SphereCollider>().radius);
    }

    private void OnTriggerEnter(Collider other) //When the player touches weapon
    {
        if(other.tag == "Player") {
            string[] split_name = weapon.name.Split('_');
            int weapon_index = int.Parse(split_name[split_name.Length - 1]); //Get the index of the weapon
            Weapon_info info = other.GetComponent<Weapon_Manager>().gun_pivot.transform.GetChild(weapon_index).GetComponent<Weapon_info>(); //Get data for pick up weapon slot
            if (!info.unlocked) { //Check if weapon has been unlocked yet, prevents weapon switching when already obtained
                info.unlocked = true; //Enable the currently picked up weapon
                other.GetComponent<Weapon_Manager>().switch_weapon(weapon_index); //Switch to the pick up weapon
            }
            Destroy(gameObject); //Destroy this object to free up memory
        }
    }
}