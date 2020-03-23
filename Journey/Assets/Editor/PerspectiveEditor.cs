using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Perspective_Puzzle))]
public class PerspectiveEditor : Editor
{
    Vector3 position_offset = new Vector3(5, 0, 5);
    float spacing = 0.1f;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.Label("----------Editor----------");

        Perspective_Puzzle pp = (Perspective_Puzzle)target;

        position_offset = EditorGUILayout.Vector3Field("Offset", position_offset);
        spacing = EditorGUILayout.FloatField("Spacing", spacing);

        if (GUILayout.Button("Detect position")) {
            //Detect the best position to face the illusion
            Vector3 player_origin = Camera.main.transform.root.position; //Get the origin of the player
            Vector3 lowest_pos = new Vector3(1000, 1000, 1000); //Best possible place, set to high number
            float lowest_val = 1000000; //Lowest point calculated
            float[] dist = new float[pp.points.Length]; //Distances of each point to check the threshold
            for (float x = -position_offset.x; x < position_offset.x; x += spacing) {
                for (float z = -position_offset.z; z < position_offset.z; z += spacing) {
                    Camera.main.transform.root.position = player_origin + new Vector3(x, 0, z); //Offset the player
                    float acc = 0;
                    for (int i = 0; i < pp.points.Length; i++) {
                        acc += Vector3.Distance(Camera.main.WorldToScreenPoint(pp.points[i].a.position), Camera.main.WorldToScreenPoint(pp.points[i].b.position));
                    }
                    if(acc < lowest_val) {
                        lowest_val = acc;
                        lowest_pos = player_origin + new Vector3(x, 0, z);
                        for (int i = 0; i < pp.points.Length; i++) {
                            dist[i] = Vector3.Distance(Camera.main.WorldToScreenPoint(pp.points[i].a.position), Camera.main.WorldToScreenPoint(pp.points[i].b.position));
                        }
                    }
                }
            }
            Camera.main.transform.root.position = lowest_pos;
            for (int i = 0; i < dist.Length; i++) {
                Debug.Log(i + ": " + dist[i]);
            }
            if (pp.puzzle_spot == null) { //Check if we already have created a point
                GameObject obj = new GameObject();
                obj.transform.position = lowest_pos;
                obj.transform.parent = pp.transform;
                obj.transform.name = pp.name + " spot";
                pp.puzzle_spot = obj.transform;
            } else {
                pp.puzzle_spot.position = lowest_pos;
            }
        }
    }
}
