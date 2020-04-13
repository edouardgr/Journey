using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Connector
{
    public Rect rect;
    public bool active;
    public Direction dir;
    public Dir_Type dataType;

    public Connector(Vector2 _pos, Vector2 _size, Direction _dir, Dir_Type _type)
    {
        rect = new Rect(_pos, _size);
        dir = _dir;
        dataType = _type;
    }

    public void Draw(Texture2D tex)
    {
        Color col = Node_Util.Get_Type_Color(dataType);
        if (!active) {
            GUI.DrawTexture(rect, Node_Util.Get_Colored_Texture(tex, dataType));
        } else {
            EditorGUI.DrawRect(rect, col);
        }
    }
}