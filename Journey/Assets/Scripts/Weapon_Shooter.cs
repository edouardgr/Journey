using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/*Edouard & Brandon*/
public class Weapon_Shooter : MonoBehaviour
{
    Weapon_Manager manager; //Player weapon manager
    Animator manager_ani; //Gun pivot animator
    Player_Movement movement; //Player movement
    RaycastHit spread_hit, normal_hit; 
    Transform ray_origin;
    public GameObject holding_pivot;

    //Bullet hole
    public GameObject bullet_hole; //Prefab for bullet holes to be placed on objects
    List<GameObject> bullet_hole_list = new List<GameObject>(); //List of all the created bullet holes
    public int max_bullet_holes = 100; //Max number of bullet holes that can exist, prevents lagging from too many bullet holes

    //Move objects
    public float move_reach = 4f; //Max distance that we can interact with an object
    GameObject move_obj; //Reference to object that will be picked up
    public float throw_force = 10f; //Force that which we throw the held object
    bool is_holding = false;

    // Start is called before the first frame update
    void Awake()
    {
        manager = GetComponent<Weapon_Manager>(); //Reference to player weapon manager
        movement = GetComponent<Player_Movement>(); //Reference to player movement
        ray_origin = transform.GetChild(0).transform; //Get the aiming point of fps
        manager_ani = manager.gun_pivot.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Physics.Raycast(ray_origin.position, ray_origin.forward, out normal_hit); //Shoot ray to check if object is in range

        //Weapon interaction
        if (!is_holding && manager.unlocked_count() > 0 && manager_ani.GetCurrentAnimatorStateInfo(0).IsName("Idle")) { //If no weapons are equiped, don't run
            Animator ani = manager.gun_pivot.transform.GetChild(manager.curr_index).GetComponent<Animator>(); //Get animator of current weapon
            if (Input.GetMouseButton(0) && ani.GetCurrentAnimatorStateInfo(0).IsName("Idle")) { //Left mouse button is pressed & animation is complete
                if (ani != null) {
                    ani.Play("Fire", 0); //Play firing animation for the equiped weapon
                }

                for (int i = 0; i < manager.info.bullet_amount; i++) { //Loop for each bullet that will be fired
                    Vector2 randxy = Random.insideUnitCircle * (manager.info.spread_radius + (manager.info.spread_move_radius * Mathf.Max(Mathf.Abs(movement.curr_input_x), Mathf.Abs(movement.curr_input_z)))); //Random point inside the spread radius
                    Physics.Raycast(ray_origin.position, ray_origin.forward + new Vector3(randxy.x, randxy.y, 0), out spread_hit); //Shoot random point from the player, spread gets larger the further from the object, also the spread increases when moving
                    if (spread_hit.collider != null) { //Check if ray has hit anything
                        create_bullet_holes(spread_hit); //Create bullet hole at the position of ray intersect
                        if (spread_hit.collider.GetComponentInParent<Shootable>() != null) { //Detect if hit object has a shootable property
                            spread_hit.collider.GetComponentInParent<Shootable>().Damage(manager.info.weapon_damage, gameObject); //Active shootable property
                        }

                        if (spread_hit.collider.GetComponent<Rigidbody>() != null) { //Check if object has a rigidbody
                            spread_hit.collider.GetComponent<Rigidbody>().AddForceAtPosition(ray_origin.forward * manager.info.bullet_force, spread_hit.point, ForceMode.Impulse); //Add an impulse to the point of contact on the object
                            Debug.DrawLine(spread_hit.point, spread_hit.point - (ray_origin.forward * manager.info.bullet_force), Color.red, 10f); //Visual indicator of bullet impact and force applied
                        }
                    }
                }
            }
        }

        //Picking up objects
        if (move_obj) { //If we are holding an object
            if (Input.GetMouseButtonDown(0)) {
                set_holding_obj("Default", false, null, throw_force);
            } else if (Input.GetKeyDown(KeyCode.E)) { //Drop button pressed
                set_holding_obj("Default", false, null, 0f);
            } else {
                //RaycastHit pos;
                /*if (Physics.Raycast(ray_origin.position, ray_origin.forward, out pos, move_reach)) { //Shoot ray to find distance to ground to prevent clipping
                    move_obj.transform.position = pos.point; //Set point to where the ray hit
                } else {*/
                    move_obj.transform.position = Vector3.Lerp(move_obj.transform.position, ray_origin.position + (ray_origin.forward * move_reach), Time.deltaTime * 100f); //Set distance to max reach
                //}
            }
        } else {
            if (normal_hit.collider != null) { //Shoot ray to check if object is in range
                if (normal_hit.collider.tag == "Movable" && normal_hit.distance < move_reach && Input.GetKeyDown(KeyCode.E)) { //Check if object can be picked up and pick up object
                    set_holding_obj("Ignore Raycast", true, normal_hit.collider.gameObject, 0f);
                    manager.disable_weapons();
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (ray_origin) {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(ray_origin.position, normal_hit.point); //Draw ray world space intersection point
            Gizmos.DrawSphere(normal_hit.point, 0.1f); //Draw world space intersection point

            if(normal_hit.collider != null && manager.unlocked_count() > 0) { //Check if we hit an object
                Handles.color = Color.green;
                RaycastHit temp;
                //Show spread radius 
                Physics.Raycast(ray_origin.position, ray_origin.forward + new Vector3(0, manager.info.spread_radius + (manager.info.spread_move_radius * Mathf.Max(Mathf.Abs(movement.curr_input_x), Mathf.Abs(movement.curr_input_z))), 0), out temp);
                Handles.DrawWireDisc(normal_hit.point, normal_hit.normal, temp.point.y - normal_hit.point.y);
            }
        }
    }

    void create_bullet_holes(RaycastHit ray) //Creates bullet decals on objects
    {
        if (ray.collider.tag != "Enemy") { //Check that player has not shot a monster
            GameObject obj = Instantiate(bullet_hole, ray.point, Quaternion.LookRotation(ray.normal), null); //Create bullet hole instance
            obj.transform.position += obj.transform.forward * 0.001f; //Slight offset, so there is not clipping with the wall
            obj.transform.parent = ray.collider.transform; //Set the bullet hole as child of the shot object, bullet move with moving walls
            bullet_hole_list.Add(obj); //Add bullet hole to the bullet list
            if (bullet_hole_list.Count > max_bullet_holes) { //Check if the amount of bullet holes exeeds the max bullet hole amount
                Destroy(bullet_hole_list[0]); //Remove the bullet holes in the game space
                bullet_hole_list.RemoveAt(0); //Remove bullet hole from the list
            }
        }
    }

    void set_holding_obj(string layer, bool enable, GameObject obj, float launch_speed)
    {
        if(enable) { move_obj = obj; /*Set reference to held object*/ }
        move_obj.layer = LayerMask.NameToLayer(layer); //Set layer to default so it can be interacted with again
        move_obj.GetComponent<Rigidbody>().freezeRotation = enable; //Freeze rotation
        move_obj.GetComponent<Rigidbody>().useGravity = !enable; //Disable or enable gravity
        move_obj.GetComponent<Rigidbody>().velocity = Vector3.zero; //Reset velocity, stops the object from slamming into the ground
        move_obj.GetComponent<Rigidbody>().AddForce(ray_origin.forward * launch_speed, ForceMode.Impulse); //Add force to launch object
        if (!enable) {
            move_obj = null; /*Remove reference to held object*/
            manager.enable_weapons();
        }
        is_holding = enable;
    }
}
