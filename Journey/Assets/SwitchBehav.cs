using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBehav : MonoBehaviour
{
    public float switch_timer = 5f;
    public float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        timer = switch_timer;
    }

    void FixedUpdate()
    {
        if(timer > 0) {
            timer -= Time.deltaTime;
            if(timer <= 0) {
                timer = switch_timer;
                Switch();
            }
        }
    }

    void Switch()
    {
        for(int i = 0; i < transform.childCount; i++) {
            transform.GetChild(i).gameObject.SetActive(!transform.GetChild(i).gameObject.activeSelf);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
