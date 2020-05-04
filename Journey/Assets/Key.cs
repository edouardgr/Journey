using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{

    public GameObject key; //key to spawn
    public bool spawned = false; //key should be spawned
    public Transform Key_pos;

    void Awake()
    {
        if (GameObject.Find("Basic_Enemy") == null && GameObject.Find("Basic_Enemy (1)") == null && GameObject.Find("Basic_Enemy (3)") == null && GameObject.Find("Basic_Enemy (4)") == null)
        {
            GameObject k = Instantiate(key, Key_pos.position, Key_pos.rotation); //Create the key
            spawned = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        // GameObject.Find("Basic_Enemy") == null
        if (spawned == false)
        {
            Awake();
            //GameObject k = Instantiate(key, transform); //Create the key
        }
    }
}
