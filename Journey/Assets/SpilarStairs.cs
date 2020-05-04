using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpilarStairs : MonoBehaviour
{
    public bool play_ani = false;
    float time = 0f;
    public float ani_time = 10f;

    //Step settings
    public float origin_y;
    public float stair_step_offset; //Height of each step

    private void Start() {
        origin_y = transform.position.y + 0.2f;    
    }

    void Update()
    {
        if(!play_ani) {
            return;
        }

        if (time < ani_time) {
            time += Time.deltaTime;
        } else {
            time = ani_time;
        }

        for (int i = 0; i < transform.childCount; i++) {
            transform.GetChild(i).position = new Vector3(transform.GetChild(i).position.x, i * ((stair_step_offset * (time / ani_time)) / transform.childCount) + origin_y, transform.GetChild(i).position.z);
        }
    }
}
