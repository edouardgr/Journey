using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

public class Enemy_Rusher_Behav : Enemy_Arena
{
    [Header("Guarding")]
    public float detection_time;

    // Update is called once per frame
    void Update()
    {
        //------------------Idle-------------------
        if (state == Enemy_state.idle) {
            if (agent.remainingDistance < agent.stoppingDistance) {
                Vector3 point;
                if (master.RandomPoint(out point)) {
                    agent.destination = point;
                }
            }
            //Detecting Player
            Collider[] view_objs = Physics.OverlapSphere(transform.position, info.field_of_view_distance);
            for (int i = 0; i < view_objs.Length; i++) {
                Transform detected_target = view_objs[i].transform;
                Vector3 dirToTarget = (detected_target.position - transform.position).normalized;
                if (Vector3.Angle(transform.forward, dirToTarget) < info.field_of_view_angle / 2) {
                    RaycastHit[] a = Physics.RaycastAll(transform.position, dirToTarget, Vector3.Distance(transform.position, detected_target.position));
                    for(int j = 0; j < a.Length; j++) {
                        if(a[j].transform.tag == "Player") {
                            Debug.DrawLine(transform.position, detected_target.transform.position, Color.red);
                            base.target = detected_target;
                            state = Enemy_state.attacking;
                        }
                    }
                }
            }
        } else if(state == Enemy_state.attacking) {
            agent.destination = target.position;
        }
    }

    private void OnDrawGizmos()
    {
        if (agent != null) {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + ((transform.forward + AngleDirection(info.field_of_view_angle, false)) * (info.field_of_view_distance / 2)));
            Gizmos.DrawLine(transform.position, transform.position + ((transform.forward + AngleDirection(-info.field_of_view_angle, false)) * (info.field_of_view_distance / 2)));

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(agent.destination, 1f); //Draw Destination
            Handles.DrawWireDisc(agent.destination, Vector3.up, stopping_distance); //Draw stopping radius
        }
    }

    Vector3 AngleDirection(float angle, bool global)
    {
        angle += (!global ? transform.eulerAngles.y : 0);
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }
}
