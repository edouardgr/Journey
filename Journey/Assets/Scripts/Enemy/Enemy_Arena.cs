using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

[System.Serializable]
public abstract class Enemy_Arena : MonoBehaviour, Shootable
{
    [Header("Parent")]
    [HideInInspector]
    public Arena_Master master; //Arena master reference, [HideInInspector] is to stop anyone from replacing it
    public Enemy_state state; //Current state of the Enemy
    protected Enemy_Info info; //Info related to the spawned enemy
    protected NavMeshAgent agent;
    public float stopping_distance = 3f;
    public float alert_when_shot = 40f;
    public float alert_chase_radius = 10f;
    public Transform target;
    public float Wrath_To_Add = 0.2f;

    public float spawn_time = 1f; //Time it takes to spawn and despawn
    [HideInInspector]
    public int time_dir = 0; //Counting up or down
    [HideInInspector]
    public float time;
    MeshRenderer mr;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        info = GetComponentInChildren<Enemy_Info>();
        mr = GetComponent<MeshRenderer>();
        agent.speed = info.move_speed;
        agent.stoppingDistance = stopping_distance;
    }

    // Update is called once per frame
    public void Update()
    {
        //Spawn lock
        if (time_dir != 0) {
            time += Time.deltaTime * time_dir;
            if (time_dir < 0 && time < 0) { time_dir = 0; time = 0; } //Count down
            if (time_dir > 0 && time > spawn_time) { time_dir = 0; time = spawn_time; } //Count up
           // mr.material.SetFloat("Vector1_2FBF5B54", (time / spawn_time));
        }
    }

    public void Damage(int amount, GameObject sender)
    {
        if(time_dir != 0) { return; }

        info.health -= amount; //Apply damage

        if(target == null) { //If player has not been detected yet
            state = Enemy_state.chase;
            target = sender.transform; //Set target
        }

        if(sender != null && sender.tag == "Player") {
            master.Alert_nearby_enemies(transform, alert_when_shot, sender.transform);
        }

        if (info.health <= 0) { //Health depleted & despawn
            if (Wrath_Bar.Can_Be_Modified)
                Wrath_Bar.Add_Wrath_Bar(Wrath_To_Add);
            DeSpawn(); //Start despawn
            agent.isStopped = true;
            master.despawn_list.Add(this);
            master.current_enemies.Remove(this);//Move from active list to despawn list
        }
    }

    public List<Transform> FOV_Detection(Enemy_Info info)
    {
        //Detecting Player
        List<Transform> list = new List<Transform>();
        Collider[] view_objs = Physics.OverlapSphere(transform.position, info.field_of_view_distance);
        foreach (Collider obj in view_objs) {
            Transform detected_target = obj.transform;
            if (Within_angle(transform, detected_target, info)) {
                RaycastHit[] hits = Physics.RaycastAll(transform.position, (detected_target.position - transform.position).normalized, Vector3.Distance(transform.position, detected_target.position));
                foreach (RaycastHit hit in hits) {
                    if (hit.transform.tag != "Untagged") {
                        Debug.DrawLine(transform.position, detected_target.transform.position, Color.red);
                        list.Add(hit.transform);
                    }
                }
            }
        }
        return list;
    }

    public bool Within_angle(Transform obj, Transform target, Enemy_Info info)
    {
        Vector3 dirToTarget = (target.position - transform.position).normalized;
        return Vector3.Angle(obj.forward, dirToTarget) < info.field_of_view_angle / 2;
    }

    public void Spawn()
    {
        time_dir = -1;
        time = spawn_time;
    }

    public void DeSpawn()
    {
        time_dir = 1;
        time = 0;
    }

    public void OnDrawGizmos()
    {
        //Alert when shot radius
        Handles.color = Color.cyan - new Color(0, 0, 0, 0.75f);
        Handles.DrawWireDisc(transform.position, transform.up, alert_when_shot);
    }
}

public enum Enemy_state
{
    idle, chase, attack
}