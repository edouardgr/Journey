using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiralActive : MonoBehaviour
{
    public List<target> targets = new List<target>();
    public SpilarStairs stair;

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < targets.Count; i++) {
            if(!targets[i].Value) {
                return;
            }
        }
        stair.play_ani = true;
        Destroy(this);
    }
}
