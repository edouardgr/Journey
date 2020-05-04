using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

public class Enemy_Rusher_Behav : Enemy_Arena
{
    [Header("Guarding")]
    public float attack_radius; //Distance the target should be inorder to attack
    public float attack_time; //Time it takes to charge up and attack
    float att_time;

    // Update is called once per frame
    private new void Update()
    {
        base.Update();
        if(time_dir != 0) {
            return;
        }
        
        //------------------Idle-------------------
        if (state == Enemy_state.idle) {
            if (agent.remainingDistance < agent.stoppingDistance) {
                Vector3 point;
                if (master.RandomPoint(out point)) {
                    agent.destination = point;
                }
            }

            List<Transform> list = FOV_Detection(info);
            foreach (Transform item in list) {
                if (item.tag == "Player") {
                    target = item;
                    state = Enemy_state.chase;
                    master.Toggle_Gates(true);
                    break;
                }
            }

        } else if(state == Enemy_state.chase) {
            GetComponent<AudiioQueue>().acitve = true;
            agent.destination = target.position;
            if(Vector3.Distance(transform.position, target.position) <= stopping_distance) { //Get ready to attack target as it in attack range
                if(Within_angle(transform, target, info)) { //If we are facing the target
                    state = Enemy_state.attack;
                    att_time = attack_time;
                } else { //Face the target
                    RotateTowards(target);
                }

            }
        } else if(state == Enemy_state.attack) {
            att_time -= Time.deltaTime;
            if(att_time <= 0) {
                if(Vector3.Distance(transform.position, target.position) <= stopping_distance && Within_angle(transform, target, info)) { //Check if target is still in range
                    target.GetComponent<Shootable>().Damage(info.projectile_damage, gameObject); //CHANGE PROJECTILE_DAMAGE to CQC_DAMAGE
                }
                state = Enemy_state.chase;
            }
        }
    }

    private void RotateTowards(Transform target) //Obtained from https://answers.unity.com/questions/540120/how-do-you-update-navmesh-rotation-after-stopping.html
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 2);
    }

    private new void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        if (agent != null) {
            //Draw Field of vision
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + ((transform.forward + AngleDirection(info.field_of_view_angle, false)) * (info.field_of_view_distance / 2)));
            Gizmos.DrawLine(transform.position, transform.position + ((transform.forward + AngleDirection(-info.field_of_view_angle, false)) * (info.field_of_view_distance / 2)));



            //Draw Destination
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(agent.destination, 1f); //Draw Destination
            //Handles.color = Color.blue;
            //Handles.DrawWireDisc(agent.destination, Vector3.up, stopping_distance); //Draw stopping radius
            //Handles.color = Color.red;
            //Handles.DrawWireDisc(transform.position, transform.up, attack_radius);
        }
    }

    Vector3 AngleDirection(float angle, bool global)
    {
        angle += (!global ? transform.eulerAngles.y : 0);
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }
}
