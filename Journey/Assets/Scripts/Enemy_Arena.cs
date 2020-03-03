using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class Enemy_Arena : MonoBehaviour, Shootable
{
    [HideInInspector]
    public Arena_Master master; //Arena master reference, [HideInInspector] is to stop anyone from replacing it
    public Enemy_state state; //Current state of the Enemy
    protected Enemy_Info info; //Info related to the spawned enemy
    protected NavMeshAgent agent;
    public float stopping_distance = 2f;
    public Transform target;

    public float spawn_time = 1;
    public int time_dir = 0;
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
            mr.material.SetFloat("Vector1_2FBF5B54", (time / spawn_time));
        }
    }

    public void Damage(int amount, GameObject sender)
    {
        if(time_dir != 0) { return; }

        info.health -= amount; //Apply damage

        if(target == null) { //If player has not been detected yet
            state = Enemy_state.attacking;
            target = sender.transform; //Set target
        }

        if(sender != null && sender.tag == "Player") {
            master.Alert_nearby_enemies(transform, 40f, sender.transform);
        }

        if (info.health <= 0) { //Health depleted & despawn
            DeSpawn(); //Start despawn
            master.despawn_list.Add(this);
            master.current_enemies.Remove(this);//Move from active list to despawn list
        }
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
}

public enum Enemy_state
{
    idle, chasing, attacking
}