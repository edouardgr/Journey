using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Print : Node
{
    public T_String msg; 

    public Print()
    {
        type_count = GetType().GetProperties().Length;
        size = new Vector2(size.x, 20 + (18 * type_count));
    }

    public override void Start()
    {

    }

    public override bool Update()
    {
        Debug.Log(msg.value);
        return true;
    }

    public override void Draw()
    {
        msg.Draw();
    }
}
