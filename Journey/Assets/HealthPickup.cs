using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class HealthPickup : MonoBehaviour
{
    public Object weapon; //Weapon that will be displayed
    public float rotation_speed = 2f; //Speed at which the weapon will rotate
    Vector3 origin; //Origin of weapon
    public float frequency = 4f; //DISCUSS
    public float amplitude = 0.25f;
    public int health = 50;
    public AudioClip sound;

    // Start is called before the first frame update
    void Awake() {
        if(weapon == null) { return; }
        Instantiate(weapon, transform); //Create the weapon object
        origin = transform.position; //Set the origin of weapon
    }

    private void FixedUpdate() //Very dangerous
    {
        transform.GetChild(0).Rotate(new Vector3(0, rotation_speed, 0)); //Rotate the weapon to get players attention
        transform.GetChild(0).position = origin + new Vector3(0, Mathf.Sin(Time.time * frequency) * amplitude, 0);
    }

    private void OnDrawGizmos() //Used for visual reference
    {
        //Handles.color = Color.green;
        //Handles.DrawWireDisc(transform.position, Vector3.up, GetComponent<SphereCollider>().radius); //up
        //Handles.DrawWireDisc(transform.position, Vector3.right, GetComponent<SphereCollider>().radius); //right
        //Handles.DrawWireDisc(transform.position, Vector3.forward, GetComponent<SphereCollider>().radius); //forward
    }

    private void OnTriggerEnter(Collider other) //When the player touches weapon
    {
        if (other.tag == "Player") {
            other.GetComponent<AudiioQueue>().queue.Add(sound);
            other.GetComponent<Player_Info>().AddHealth(health);
            Destroy(gameObject); //Destroy this object to free up memory
        }
    }
}
