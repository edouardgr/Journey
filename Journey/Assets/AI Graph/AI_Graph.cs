using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AI_Graph : ScriptableObject
{
    [SerializeField]
    public List<Node> nodes;
    private List<Node> Nodes {
        get {
            if (nodes == null) {
                nodes = new List<Node>();
            }
            return nodes;
        }
    }

    public static AI_Graph Create(string name)
    {
        AI_Graph graph = CreateInstance<AI_Graph>();

        string path = string.Format("Assets/{0}.asset", name);
        AssetDatabase.CreateAsset(graph, path);

        return graph;
    }

    public void AddNode(Node node)
    {
        Nodes.Add(node);
        AssetDatabase.AddObjectToAsset(node, this);
        AssetDatabase.SaveAssets();
    }
}
