using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

public class Arena_Master : MonoBehaviour
{
    public Transform arena_center;
    public float arena_distance;
    [Header("Enemy Spawning")]
    public int max_enemies; //Max amount of enemies allowed into the arena
    public List<Enemy_Arena> current_enemies; //Current amount of numbers in the scene
    public int enemy_tickets; //Number of enemies that will be spawned over time
    public GameObject[] enemies; //List of enemies to spawn

    // Start is called before the first frame update
    void Start()
    {
        current_enemies = new List<Enemy_Arena>();
    }

    // Update is called once per frame
    void Update()
    {
        if(current_enemies.Count < max_enemies && enemy_tickets > 0) {
            Vector3 point;
            if(RandomPoint(out point)) {
                GameObject enemy = Instantiate(enemies[0], point, transform.rotation, null);
                enemy.GetComponent<Enemy_Arena>().master = this;
                current_enemies.Add(enemy.GetComponent<Enemy_Arena>());
                enemy_tickets--;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Handles.color = Color.blue;
        Handles.DrawWireDisc(arena_center.position, arena_center.up, arena_distance);
    }

    public void Alert_nearby_enemies(GameObject enemy, float radius, Transform target)
    {
        for(int i = 0; i < current_enemies.Count; i++) {
            if(!current_enemies[i].target && current_enemies[i] != enemy && Vector3.Distance(current_enemies[i].transform.position, enemy.transform.position) < radius) {
                current_enemies[i].target = target;
                current_enemies[i].state = Enemy_state.attacking;
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
}
