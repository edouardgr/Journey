using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

[RequireComponent(typeof(SphereCollider))]
public class Arena_Master : MonoBehaviour
{
    public Transform arena_center;
    public float arena_distance;
    [Header("Enemy Spawning")]
    public int max_enemies; //Max amount of enemies allowed into the arena
    public List<Enemy_Arena> current_enemies; //Current amount of numbers in the scene
    public List<Enemy_Arena> despawn_list; //List of enemies that are despawning
    public int enemy_tickets; //Number of enemies that will be spawned over time
    public GameObject[] enemies; //List of enemies to spawn

    //IMPLEMENT ENEMIES ARE POINTED TO A POSITION AROUND THE PLAYER IN A CIRCULAR WAY TO AVOID CLUSTERING

    // Start is called before the first frame update
    void Start()
    {
        current_enemies = new List<Enemy_Arena>();
        SphereCollider col = GetComponent<SphereCollider>();
        col.center = arena_center.localPosition;
        col.radius = arena_distance;
        col.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(current_enemies.Count < max_enemies && enemy_tickets > 0) {
            Vector3 point;
            if(RandomPoint(out point)) {
                GameObject enemy = Instantiate(enemies[0], point, Quaternion.AngleAxis(Random.value * 360, transform.up), null);
                enemy.GetComponent<Enemy_Arena>().master = this;
                enemy.GetComponent<Enemy_Arena>().Spawn();
                current_enemies.Add(enemy.GetComponent<Enemy_Arena>());
                enemy_tickets--;
            }
        }

        if(despawn_list.Count > 0) {
            for(int i = 0; i < despawn_list.Count; i++) {
                if(despawn_list[i].time_dir == 0) {
                    Enemy_Arena ea = despawn_list[i]; //Get reference to (about to be) Destoyed object
                    despawn_list.RemoveAt(i); //Remove from enemy list
                    Destroy(ea.gameObject); //Destroy enemy
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Handles.color = Color.blue;
        Handles.DrawWireDisc(arena_center.position, arena_center.up, arena_distance);
    }

    public void Alert_nearby_enemies(Transform origin_point, float radius, Transform target)
    {
        for(int i = 0; i < current_enemies.Count; i++) {
            if(!current_enemies[i].target && Vector3.Distance(current_enemies[i].transform.position, origin_point.transform.position) < radius) {
                current_enemies[i].target = target;
                current_enemies[i].state = Enemy_state.chase;
            }
        }
    }

    public bool RandomPoint(out Vector3 result) //Shorter version of the RandomPoint Function
    {
        return RandomPoint(arena_center.position, arena_distance, out result);
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result) //From unity website
    {
        for (int i = 0; i < 30; i++) {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)) {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player") {
            other.GetComponent<Weapon_Shooter>().current_arena = this;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player") {
            other.GetComponent<Weapon_Shooter>().current_arena = null;
        }
    }
}
