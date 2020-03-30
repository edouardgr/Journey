using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wait : Node
{
    public Wait()
    {
        color = Color.blue;
    }

    public override void Start()
    {
        Debug.Log("Wait: Start()");
    }

    public override bool Update()
    {
        Debug.Log("Wait: Update()");
        return true;
    }
}
