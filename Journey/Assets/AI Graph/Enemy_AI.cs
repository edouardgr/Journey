using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AI : MonoBehaviour
{
    public AI_Graph graph;
    Node current_node;

    // Start is called before the first frame update
    void Start()
    {
        if(graph == null) {
            return;
        }

        current_node = graph.nodes[0];
        current_node.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if(graph == null) {
            return; 
        }

        if (current_node != null && current_node.Update()) {
            current_node = current_node.next_node;
            if (current_node != null) {
                current_node.Start();
            }
        }
    }
}
