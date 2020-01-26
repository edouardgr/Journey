using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawBounds : MonoBehaviour
{
    EdgeCollider2D pc;

    private void OnDrawGizmos()
    {
        pc = GetComponent<EdgeCollider2D>();
        Gizmos.color = Color.green;
        for(int i = 0; i < pc.pointCount - 1; i++) {
            Gizmos.DrawLine(transform.position + new Vector3(pc.points[i].x, pc.points[i].y), transform.position + new Vector3(pc.points[i + 1].x, pc.points[i + 1].y));
        }
    }
}
