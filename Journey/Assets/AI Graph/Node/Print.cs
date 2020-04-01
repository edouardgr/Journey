using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Print : Node
{
    string msg;

    public Print()
    {
        color = Color.blue;
    }

    public override void Start()
    {
        Debug.Log("Print: Start()");
    }

    public override bool Update()
    {
        Debug.Log("Print: Update()");
        return true;
    }
}
