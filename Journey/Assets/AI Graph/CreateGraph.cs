using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateGraph
{

    [MenuItem("Assets/Create/AI Graph", false, 10)]
    public static void Create_Graph()
    {
        // Create graph.
        AI_Graph graph = AI_Graph.Create("NewGraph " + (Random.value * 100));

        // Create nodes.
        Init nodeA = Node.Create<Init>("Init");
        Exit nodeB = Node.Create<Exit>("Exit");

        // Add nodes to graph.
        graph.AddNode(nodeA);
        graph.AddNode(nodeB);
    }
}
