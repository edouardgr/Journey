using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class T_String : T_Type
{
    public Connector connect = new Connector(new Vector2(3, 3), new Vector2(12, 12), Direction.input, Dir_Type.t_string);
    public string value;

    public override void Draw()
    {
        value = EditorGUILayout.TextField(value);
    }
}
