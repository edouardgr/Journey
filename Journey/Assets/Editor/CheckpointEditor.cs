using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Checkpoint_master))]
public class CheckpointEditor : Editor
{
    GameObject prefab;
    public override void OnInspectorGUI()
    {
        prefab = Resources.Load<GameObject>("Checkpoint");
        Checkpoint_master obj = (Checkpoint_master)target;

        if (GUILayout.Button("Add Checkpoint"))
        {
            GameObject gameObj = Instantiate(prefab, obj.transform.position, obj.transform.rotation, obj.transform);
            gameObj.name = "Checkpoint " + obj.transform.childCount;
        }

        DrawDefaultInspector();
    }
}
