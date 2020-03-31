using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Node_Connectors : Connector
{
    public Node_Connectors(Vector2 _pos, Vector2 _size, Direction _dir)
    {
        rect = new Rect(_pos, _size);
        dir = _dir;
    }

    public override void Draw(Texture tex)
    {
        if (!active) {
            GUI.DrawTexture(rect, tex);
        } else {
            EditorGUI.DrawRect(rect, Color.white);
        }
    }
}
