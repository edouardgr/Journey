using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/*Edouard and Brian : )*/

public class Basic_Enemy : MonoBehaviour, Shootable
{
    public float wrath_bar_percent = 0.20f;
    public GameObject enemy; //Enemy to spawn
    Enemy_Info info; //Info related to the spawned enemy
    public float detection_radius = 10f; //Player enters radius, enemy becomes active
    public Transform target = null; //Object that the enemy will chase
    public GameObject projectile; //Projectile object enemy will shoot at target
    float timer = 0; //Timer for projectile firing delay
    public float Wrath_To_Add = 0.2f;
    // Start is called before the first frame update
    void Awake()
    {
        GameObject obj = Instantiate(enemy, transform); //Create the enemy
        info = obj.GetComponent<Enemy_Info>(); //Get info of the enemy
    }

    private void FixedUpdate()
    {
        if (target == null) { //Don't interact if no player is detected 
            return;
        }

        transform.LookAt(target, Vector3.up); //Look towards the target
        transform.position = Vector3.MoveTowards(transform.position, target.position, info.move_speed); //Move towards the target

        timer -= Time.deltaTime; //Update timer
        if(timer <= 0) { //Timer has reached 0, spawn a projectile
            timer = info.projectile_fire_rate; //Reset timer
            //SHOOT PROJECTILE
            GameObject obj = Instantiate(projectile, transform.GetChild(0).GetChild(0).position, Quaternion.identity, null); //Create projectile //MODIFY
            obj.transform.LookAt(target); //Point projectile to target
            obj.GetComponent<Projectile_Behaviour>().speed = info.projectile_speed; //Set projectile speed
            obj.GetComponent<Projectile_Behaviour>().damage = info.projectile_damage; //Set projectile damage
            obj.GetComponent<Projectile_Behaviour>().parent = transform.GetChild(0).GetChild(0).gameObject; //Set who created the projectile //REWORK
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null) { GetComponent<AudiioQueue>().acitve = true; }

        Collider[] cols = Physics.OverlapSphere(transform.position, detection_radius); //Check all entities in sphere
        foreach(Collider col in cols) { //Cycle through entities
            if(col.tag == "Player") { //Check if entity is a player
                target = col.transform; //Set target to entity
                break; //Stop checking entities
            }
        }
    }

    private void OnDrawGizmos()
    {
        //Handles.color = Color.blue;
        //Handles.DrawWireDisc(transform.position, transform.up, detection_radius); //Detection radius
        if(target) {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, target.position); //Line to show who the enemy is chasing
        }
    }

    public void Damage(int amount, GameObject sender) //Recieve damage
    {
        info.health -= amount; //Deplete enemy health
        if(info.health <= 0) { //Check if enemy health is below 0
               if(Wrath_Bar.Can_Be_Modified)
                    Wrath_Bar.Add_Wrath_Bar(Wrath_To_Add);

               Destroy(gameObject); //Remove enemy from the game            
        }
        if(!target && sender) { //Check if enemy is not currently chasing a target
            timer = info.projectile_fire_rate; //Initilize timer
            target = sender.transform; //Set the target to chase
        }
    }
}
