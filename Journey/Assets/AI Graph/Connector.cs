using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public abstract class Connector
{
    public Rect rect;
    public bool active;
    public Direction dir;

    public abstract void Draw(Texture tex);
}