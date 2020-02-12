using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Basic_Enemy : MonoBehaviour, Shootable
{
    public GameObject enemy;
    Enemy_info info;
    public float detection_radius = 10f;
    public Transform target = null;
    public GameObject projectile;
    float timer = 0;

    // Start is called before the first frame update
    void Awake()
    {
        GameObject obj = Instantiate(enemy, transform);
        info = obj.GetComponent<Enemy_info>();
    }

    private void FixedUpdate()
    {
        if (target == null) { //Don't interact if no player is detected 
            return;
        }

        transform.LookAt(target, Vector3.up);
        transform.position = Vector3.MoveTowards(transform.position, target.position, info.move_speed);

        timer -= Time.deltaTime;
        if(timer <= 0) {
            timer = info.projectile_fire_rate;
            //SHOOT PROJECTILE
            GameObject obj = Instantiate(projectile, transform.GetChild(0).GetChild(0).position, Quaternion.identity, null); //MODIFY
            obj.transform.LookAt(target);
            obj.GetComponent<Projectile_Behaviour>().speed = info.projectile_speed;
            obj.GetComponent<Projectile_Behaviour>().damage = info.projectile_damage;
            obj.GetComponent<Projectile_Behaviour>().parent = transform.GetChild(0).GetChild(0).gameObject; //REWORK
        }
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, detection_radius); //Check all entities in sphere
        foreach(Collider col in cols) { //Cycle through entities
            if(col.tag == "Player") {
                target = col.transform;
                break;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Handles.color = Color.blue;
        Handles.DrawWireDisc(transform.position, transform.up, detection_radius);
        if(target) {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, target.position);
        }
    }

    public void Damage(int amount, GameObject sender) //Recieve damage
    {
        info.health -= amount;
        if(info.health <= 0) {
            Destroy(gameObject);
        }
        if(!target) {
            timer = info.projectile_fire_rate;
            target = sender.transform;
        }
    }
}
