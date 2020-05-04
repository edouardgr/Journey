using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Init : Node
{
    public Init()
    {
        is_protected = true;
        has_input = null;
        color = Color.red;
    }

    public override void Start()
    {

    }

    public override bool Update()
    {
        return true;
    }
}
