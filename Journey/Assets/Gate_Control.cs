using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Gate_Control : MonoBehaviour, Interactive
{
    Animator ani;
    public bool is_open = true;
    bool in_animation = false;
    float open_pos_y;
    public float close_pos_y;
    Transform gate;

    // Start is called before the first frame update
    void Start()
    {
        gate = transform.GetChild(0);
        open_pos_y = gate.localPosition.y;
        ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (in_animation && (is_open && gate.position.y >= close_pos_y) || (!is_open && gate.position.y <= open_pos_y)) {
            is_open = !is_open;
            in_animation = false;
        }
    }


    private void LateUpdate()
    {
        if (!in_animation) {
            gate.localPosition = new Vector3(0, is_open ? open_pos_y : close_pos_y, 0);
        }
    }

    public void Open_Gate()
    {
        if (in_animation || is_open) { return; }
        ani.Play("Open_Gate");
        in_animation = true;
    }

    public void Close_Gate()
    {
        if (in_animation || !is_open) { return; }
        ani.Play("Close_Gate");
        in_animation = true;
    }

    public void Toggle()
    {
        if (in_animation) { return; }
        if (is_open) {
            Close_Gate();
        } else {
            Open_Gate();
        }
    }

    public void enable()
    {
        Toggle();
    }
}
