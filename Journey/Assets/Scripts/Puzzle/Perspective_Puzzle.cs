using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perspective_Puzzle : MonoBehaviour
{
    public Connection[] points;
    public GameObject complete, puzzle;
    public float point_dist;
    public Transform puzzle_spot;

    bool start_fade = false;
    public float fade_time = 2f;
    float time = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Reveal completed puzzle
        if(start_fade) {
            //Tinker with this maybe
            Camera.main.transform.root.position = /*Vector3.Lerp(Camera.main.transform.root.position,*/ puzzle_spot.position;//, Time.deltaTime * 5);
            time -= Time.deltaTime;

            MeshRenderer[] meshes = complete.GetComponentsInChildren<MeshRenderer>();
            foreach(MeshRenderer mesh in meshes) {
                mesh.material.SetFloat("Vector1_2FBF5B54", (time / (fade_time * 1.5f)));
            }

            meshes = puzzle.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer mesh in meshes) {
                mesh.material.SetFloat("Vector1_2FBF5B54", ((fade_time - time) / fade_time));
            }

            if (time <= 0) {
                puzzle.SetActive(false);
                Camera.main.transform.root.GetComponent<Player_Movement1>().is_enabled = true;
                enabled = false; //Disable the script as it is
            }
            return;
        }

        //HAVE THE PLAYER STOP BEFORE CHECKING IF RIGHT ANGLE
        //Detect spot
        if (Vector3.Distance(Camera.main.transform.parent.position, points[0].a.position) > 30) { return; }
        bool match = true;
        for(int i = 0; i < points.Length; i++) {
            //Debug.Log(i + ": " + Vector3.Distance(Camera.main.WorldToScreenPoint(points[i].a.position), Camera.main.WorldToScreenPoint(points[i].b.position)));
            Vector3 point_a = Camera.main.WorldToViewportPoint(points[i].a.position);
            Vector3 point_b = Camera.main.WorldToViewportPoint(points[i].b.position);
            Debug.Log(point_a);
            if (point_a.x < 0 || point_a.x > 1 || point_a.y < 0 || point_a.y > 1 || point_a.z <= 0 || point_b.x < 0 || point_b.x > 1 || point_b.y < 0 || point_b.y > 1 || point_b.z <= 0 ||
                Vector3.Distance(Camera.main.WorldToScreenPoint(points[i].a.position), Camera.main.WorldToScreenPoint(points[i].b.position)) >= point_dist) {
                match = false;
                break;
            }
        }
        if(match) {
            Camera.main.transform.root.GetComponent<Player_Movement1>().is_enabled = false;
            start_fade = true;
            time = fade_time;
            //Disable shadows from appearing
            complete.SetActive(true);
            MeshRenderer[] meshes = complete.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer mesh in meshes) {
                mesh.material.SetFloat("Vector1_2FBF5B54", 1f);
            }
        }
    }

    private void OnDrawGizmos()
    {
        for(int i = 0; i < points.Length; i++) {
            Gizmos.color = new Color(1, 0, 0, 0.25f);
            Gizmos.DrawSphere(points[i].a.position, 0.1f);
            Gizmos.DrawSphere(points[i].b.position, 0.1f);
            Gizmos.DrawLine(points[i].a.position, points[i].b.position);
        }
    }

    [System.Serializable]
    public class Connection
    {
        public Transform a, b;
    }
}
