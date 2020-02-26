using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Trail_Behav : MonoBehaviour
{
    LineRenderer lr;
    public float fade_time = 1f;
    float time = 0;

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        time = fade_time;
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        //lr.startColor = new Color(lr.startColor.r, lr.startColor.g, lr.startColor.b, time / fade_time);
        lr.endColor = new Color(lr.endColor.r, lr.endColor.g, lr.endColor.b, time / fade_time);
        if(time <= 0) { Destroy(gameObject); }
    }
}
