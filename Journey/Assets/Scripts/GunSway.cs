using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSway : MonoBehaviour
{
    Player_Movement pm;
    Vector3 origin_pos;
    Vector3 origin_rot;
    float amount = 0.01f;
    float rot_amount = 5f;


    void Awake()
    {
        pm = transform.GetComponentInParent<Player_Movement>();
        origin_pos = transform.localPosition;
        origin_rot = transform.localEulerAngles;
    }

    void Update()
    {
        Vector3 offset_pos = new Vector3(pm.curr_input_x, 0, pm.curr_input_z) * amount;
        Vector3 offset_rot = new Vector3(0, 0, pm.curr_input_x) * rot_amount;
        transform.localPosition = origin_pos + offset_pos;
        transform.localEulerAngles = origin_rot + offset_rot;
    }
}
