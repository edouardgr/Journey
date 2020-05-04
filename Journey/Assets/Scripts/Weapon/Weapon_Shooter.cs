using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/*Edouard & Brandon*/
public class Weapon_Shooter : MonoBehaviour
{
    Weapon_Manager manager; //Player weapon manager
    Animator manager_ani; //Gun pivot animator
    Player_Movement1 movement; //Player movement
    RaycastHit spread_hit, normal_hit; 
    Transform ray_origin;
    public Canvas hit_marker; //Hit marker of gun
    public RectTransform aim_reticle; //Aiming reticle of player
    public Arena_Master current_arena; //Get the current arena we are in
    AudioSource audio_clip; //Play gun sounds

    //Bullet hole
    [Header("Bullet holes")]
    public GameObject bullet_hole; //Prefab for bullet holes to be placed on objects
    public GameObject bullet_trail;
    List<GameObject> bullet_hole_list = new List<GameObject>(); //List of all the created bullet holes
    public int max_bullet_holes = 100; //Max number of bullet holes that can exist, prevents lagging from too many bullet holes

    //Move objects
    [Header("Move Objects")]
    public float move_reach = 4f; //Max distance that we can interact with an object
    GameObject move_obj; //Reference to object that will be picked up
    public float throw_force = 10f; //Force that which we throw the held object
    bool is_holding = false;
    public bool perfect_aim;


    // Start is called before the first frame update
    void Awake()
    {
        manager = GetComponent<Weapon_Manager>(); //Reference to player weapon manager
        movement = GetComponent<Player_Movement1>(); //Reference to player movement
        ray_origin = transform.GetChild(0).transform; //Get the aiming point of fps
        manager_ani = manager.gun_pivot.GetComponent<Animator>();
        audio_clip = GetComponent<AudioSource>(); //sound stuff
    }

    // Update is called once per frame
    void Update()
    {
        Physics.Raycast(ray_origin.position, ray_origin.forward, out normal_hit); //Shoot ray to check if object is in range

        
        if(move_obj == null && normal_hit.collider != null && normal_hit.collider.tag == "Interactable" && normal_hit.distance <= move_reach) {
            if (normal_hit.collider.GetComponentInParent<Interactive>() != null && Input.GetKeyDown(KeyCode.E)) {
                normal_hit.collider.GetComponentInParent<Interactive>().enable();
            }
        }

        //Recticle enlargens when moving, to indicate worse accuracy when moving
        if (aim_reticle) { //ASK BRAIN
           float reticle_size = 40 + (Mathf.Max(Mathf.Abs(movement.curr_input_x), Mathf.Abs(movement.curr_input_z)) * 20);
           aim_reticle.sizeDelta = new Vector2(reticle_size, reticle_size);
        }

        //Weapon interaction
        if (!is_holding && manager.unlocked_count() > 0 && manager_ani.GetCurrentAnimatorStateInfo(0).IsName("Idle")) { //If no weapons are equiped, don't run
            Animator ani = manager.gun_pivot.transform.GetChild(manager.curr_index).GetComponent<Animator>(); //Get animator of current weapon
            if (Input.GetMouseButton(0) && ani.GetCurrentAnimatorStateInfo(0).IsName("Idle")) { //Left mouse button is pressed & animation is complete
                if (ani != null) {
                    ani.Play("Fire", 0); //Play firing animation for the equiped weapon
                }
                //Sound stuff
                if (manager.info.sound != null){
                    audio_clip.clip = manager.info.sound;
                    audio_clip.Play();
                }
                bool hit_confirmed = false;
                for (int i = 0; i < manager.info.bullet_amount; i++) { //Loop for each bullet that will be fired
                    Vector2 randxy = Random.insideUnitCircle * (manager.info.spread_radius + (manager.info.spread_move_radius * Mathf.Max(Mathf.Abs(movement.curr_input_x), Mathf.Abs(movement.curr_input_z)))); //Random point inside the spread radius
                    if(perfect_aim) { randxy = Vector2.zero; }
                    Physics.Raycast(ray_origin.position, ray_origin.forward + new Vector3(randxy.x, randxy.y, 0), out spread_hit); //Shoot random point from the player, spread gets larger the further from the object, also the spread increases when moving
                    if (spread_hit.collider != null && spread_hit.collider.tag != "Player") { //Check if ray has hit anything
                        create_bullet_holes(spread_hit); //Create bullet hole at the position of ray intersect
                        if (spread_hit.collider.GetComponentInParent<Shootable>() != null) { //Detect if hit object has a shootable property
                            spread_hit.collider.GetComponentInParent<Shootable>().Damage(manager.info.weapon_damage, gameObject); //Active shootable property
                            hit_confirmed = true;
                        }

                        if (spread_hit.collider.GetComponent<Rigidbody>() != null && spread_hit.collider.tag != "Non-Shootable") { //Check if object has a rigidbody
                            spread_hit.collider.GetComponent<Rigidbody>().AddForceAtPosition(ray_origin.forward * manager.info.bullet_force, spread_hit.point, ForceMode.Impulse); //Add an impulse to the point of contact on the object
                            if (hit_marker) { spread_hit.collider.gameObject.AddComponent<Throwable_Obj>().hit_marker = hit_marker; } //Flying objects will do damage
                            Debug.DrawLine(spread_hit.point, spread_hit.point - (ray_origin.forward * manager.info.bullet_force), Color.red, 10f); //Visual indicator of bullet impact and force applied
                        }
                    }
                    GameObject line = Instantiate(bullet_trail); //Bullet trail - visual feedback
                    line.GetComponent<LineRenderer>().SetPosition(0, manager.info.barrel_point.position); //From the barrel
                    if (spread_hit.collider != null && spread_hit.collider.tag != "Player") {
                        line.GetComponent<LineRenderer>().SetPosition(1, spread_hit.point); //To the contact point
                    } else {
                        line.GetComponent<LineRenderer>().SetPosition(1, ray_origin.position + (ray_origin.forward + new Vector3(randxy.x, randxy.y)) * 100f); //Trail into the distance
                    }
                }
                if(hit_marker && hit_confirmed) { hit_marker.GetComponent<Animator>().Play("Hit"); } //Play hit marker ani if we hit a shootable enemy
                if(current_arena) { current_arena.Alert_nearby_enemies(transform, manager.info.sound_radius, transform); }
            }
        }

        //Picking up objects
        if (move_obj) { //If we are holding an object
            if (Input.GetMouseButtonDown(0)) {
                set_holding_obj("Default", false, null, throw_force);
            } else if (Input.GetKeyDown(KeyCode.E)) { //Drop button pressed
                set_holding_obj("Default", false, null, 0f);
            } else { //Currently holding object
                RaycastHit pos; //FIX PHYX OBJECT FREAKING OUT
                move_obj.transform.position = Vector3.Lerp(move_obj.transform.position, (Physics.Raycast(ray_origin.position, ray_origin.forward, out pos, move_reach) ? pos.point : ray_origin.position + (ray_origin.forward * move_reach)), Time.deltaTime * 100f); //Set distance to max reach
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

        if(manager && manager.info) {
            Handles.color = Color.yellow;
            Handles.DrawWireDisc(transform.position, transform.up, manager.info.sound_radius);
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
        if(enable) { move_obj = obj; obj.transform.SetParent(ray_origin); /*Set reference to held object and set parent of object*/ }
        move_obj.layer = LayerMask.NameToLayer(layer); //Set layer to default so it can be interacted with again
        move_obj.GetComponent<Rigidbody>().freezeRotation = enable; //Freeze rotation
        move_obj.GetComponent<Rigidbody>().useGravity = !enable; //Disable or enable gravity
        move_obj.GetComponent<Rigidbody>().velocity = Vector3.zero; //Reset velocity, stops the object from slamming into the ground
        move_obj.GetComponent<Rigidbody>().AddForce(ray_origin.forward * launch_speed, ForceMode.Impulse); //Add force to launch object
        if (!enable) {
            if (hit_marker) { move_obj.AddComponent<Throwable_Obj>().hit_marker = hit_marker; }
            move_obj.transform.SetParent(null); //Remove parent
            move_obj = null; /*Remove reference to held object*/
            manager.enable_weapons();
        }
        is_holding = enable;
    }
}
