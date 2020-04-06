using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Node_Util
{
    public static Color Get_Type_Color(Dir_Type type)
    {
        switch(type) {
            case Dir_Type.t_control:
            return Color.white;
            case Dir_Type.t_int:
            return Color.blue;
            case Dir_Type.t_string:
            return Color.green;
            case Dir_Type.t_boolean:
            return Color.red;
            case Dir_Type.t_double:
            return Color.yellow;
            case Dir_Type.t_float:
            return Color.magenta;
        }
        return Color.black;
    }

    public static Texture2D Get_Colored_Texture(Texture2D tex, Dir_Type type)
    {
        Texture2D tex2 = new Texture2D(tex.width, tex.height);
        Color col = Get_Type_Color(type);
        for (int x = 0; x < tex2.width; x++) {
            for (int y = 0; y < tex2.height; y++) {
                if (tex.GetPixel(x, y).a >= 0.8f) {
                    tex2.SetPixel(x, y, col);
                } else {
                    tex2.SetPixel(x, y, Color.clear);
                }
            }
        }
        tex2.Apply();
        return tex2;
    }
}


public enum Direction
{
    input, output
}

public enum Dir_Type
{
    t_control, t_int, t_string, t_double, t_float, t_boolean
}
