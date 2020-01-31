using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockBehav : MonoBehaviour
{
    public bool is_active = false;
    SpriteRenderer sr;
    GameObject door;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        door = transform.parent.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        sr.color = (is_active ? Color.green : Color.red);
        door.SetActive(!is_active);
        if(is_active) {
            GameObject.Find("PlayerRespawnPoint").transform.position = transform.position;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && Input.GetKeyDown(KeyCode.E)) {
            is_active = !is_active;
        }
    }
}
