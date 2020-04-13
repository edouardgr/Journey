using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : Node
{
    public Exit() : base(new Vector2(500, 100), new Vector2(100, 100))
    {
        is_protected = true;
        has_output = null;
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
