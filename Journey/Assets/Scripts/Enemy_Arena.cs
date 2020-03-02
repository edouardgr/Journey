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

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        info = GetComponentInChildren<Enemy_Info>();
        agent.speed = info.move_speed;
        agent.stoppingDistance = stopping_distance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(int amount, GameObject sender)
    {
        info.health -= amount; //Apply damage

        if(target == null) { //If player has not been detected yet
            state = Enemy_state.attacking;
            target = sender.transform; //Set target
        }

        if(sender.tag == "Player") {
            master.Alert_nearby_enemies(gameObject, 40f, sender.transform);
        }

        if (info.health <= 0) { //Health depleted
            master.current_enemies.Remove(this); //Remove from enemy list
            Destroy(gameObject); //Destroy enemy
        }
    }
}

public enum Enemy_state
{
    idle, chasing, attacking
}