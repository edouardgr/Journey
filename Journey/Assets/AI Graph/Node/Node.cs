using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public abstract class Node : ScriptableObject
{
    public bool is_protected = false;
    public int type_count = 0;
    public Vector2 position, size;
    public Color color;
    public Connector has_input, has_output;
    public Node next_node;

    public abstract void Start();
    public abstract bool Update();
    public virtual void Draw() { }

    public Node() : this(new Vector2(100, 100), new Vector2(100, 100)) { }
    public Node(Vector2 _position, Vector2 _size) : this(_position, _size, Color.blue) { }
    public Node(Vector2 _position, Vector2 _size, Color _color)
    {
        position = _position;
        size = _size;
        color = _color;
        has_input = new Connector(new Vector2(3, 23), new Vector2(12, 12), Direction.input, Dir_Type.t_control);
        has_output = new Connector(new Vector2(size.x - 3 - 12, 23), new Vector2(12, 12), Direction.output, Dir_Type.t_control);
    }

    public static T Create<T>(string name) where T : Node
    {
        T node = CreateInstance<T>();
        node.name = name;
        return node;
    }
}