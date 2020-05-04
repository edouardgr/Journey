using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{

    public GameObject key; //key prefab to spawn
    private GameObject k; //spawned key
    public GameObject door; //door to be opened
    public bool spawned = false; //key should be spawned
    private bool destroyed = false; //key should be collected
    private bool door_opened = false; //door should be opened
    public Transform Key_pos;
    private Vector3 offset = new Vector3(5, 0, 0);


    void Awake()
    {
        if (GameObject.Find("Basic_Enemy") == null && GameObject.Find("Basic_Enemy (1)") == null && GameObject.Find("Basic_Enemy (3)") == null && GameObject.Find("Basic_Enemy (4)") == null)
        {
            k = Instantiate(key, Key_pos.position + offset, Key_pos.rotation); //Create the key
            spawned = true;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (!spawned) //instantiate key (must be in Awake())
        {
            Awake();
        }
        else if (!destroyed) //collect key
        {
            if (Mathf.Abs(Key_pos.position.x - k.transform.position.x) <= 2 && Mathf.Abs(Key_pos.position.z - k.transform.position.z) <= 2) //manual key collider
            {
                destroyed = true;
                Debug.Log("Entered collider");
                Destroy(k);
            }
        }
        else if(!door_opened) //open door
        {
            door_opened = true;
            door = GameObject.Find("DoorW");
            door.transform.position = new Vector3(door.transform.position.x, door.transform.position.y, door.transform.position.z - 1.5f);
        }
    }

}
