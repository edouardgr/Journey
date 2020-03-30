using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Init : Node
{
    public Init()
    {
        is_protected = true;
        has_input = null;
    }

    public override void Start()
    {
        Debug.Log("Init: Start()");
    }

    public override bool Update()
    {
        Debug.Log("Init: Update()");
        return true;
    }
}
