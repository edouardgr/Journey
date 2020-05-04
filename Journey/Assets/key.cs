
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class key : MonoBehaviour
{

    public Transform player; // position of player
    public GameObject key_prefab; // key to be spawned
    private GameObject k; // spawned key
    public GameObject door; // door to be opened
    private bool spawned = false;
    private bool destroyed = false;

    void Awake()
    {
        // if no enemies found, spawn key
        if(GameObject.Find("Basic_Enemy") == null && GameObject.Find("Basic_Enemy (1)") == null && GameObject.Find("Basic_Enemy (2)") == null && GameObject.Find("Basic_Enemy (3)") == null)
        {
            k = Instantiate(key_prefab, new Vector3(player.position.x - 5, player.position.y, player.position.z), default(Quaternion));
            spawned = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!spawned) // must spawn key
        {
            Awake();
        }
        else if (!destroyed) // must collect key and open door
        {
            if(Mathf.Abs(k.transform.position.x - player.position.x) <= 2 && Mathf.Abs(k.transform.position.z - player.position.z) <= 2) // manual key collider
            {
                Destroy(k);
                destroyed = true;
                door.transform.position = new Vector3(door.transform.position.x, door.transform.position.y, door.transform.position.z - 1.5f);
            }
        }
    }

}
