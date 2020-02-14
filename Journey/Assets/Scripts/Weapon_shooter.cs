using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Weapon_shooter : MonoBehaviour
{
    public Weapon_Manager manager;
    RaycastHit spread_hit, normal_hit;
    Transform ray_origin;

    //Bullet hole
    public GameObject bullet_hole;
    List<GameObject> bullet_hole_list = new List<GameObject>();
    public int max_bullet_holes = 5;

    // Start is called before the first frame update
    void Awake()
    {
        manager = GetComponent<Weapon_Manager>();
        ray_origin = transform.GetChild(0).transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(ray_origin.position, ray_origin.forward, out normal_hit)) {

        }

        

        //ADDD: MAKE RADIUS SMALLER IS STANDING STILL

        //Weapon interaction
        if (manager.unlocked_count() <= 0 || !manager.gun_pivot.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle")) { //If no weapons are equiped, don't run
            return; //Early exit
        }

        Animator ani = manager.gun_pivot.transform.GetChild(manager.curr_index).GetComponent<Animator>(); //Get animator of current weapon
        if (Input.GetMouseButtonDown(0) /*&& ani.GetCurrentAnimatorStateInfo(0).IsName("Idle")*/) { //Left mouse button is pressed & animation is complete
            if (ani != null) {
                ani.Play("Fire", 0); //Play firing animation for the equiped weapon
            }

            for (int i = 0; i < manager.info.bullet_amount; i++) {
                Vector2 randxy = Random.insideUnitCircle * manager.info.spread_radius;
                Physics.Raycast(ray_origin.position, ray_origin.forward + new Vector3(randxy.x, randxy.y, 0), out spread_hit);
                if (spread_hit.collider != null) {
                    create_bullet_holes(spread_hit);
                    if (spread_hit.collider.GetComponentInParent<Shootable>() != null) { //Detect if hit object has a shootable property
                        spread_hit.collider.GetComponentInParent<Shootable>().Damage(manager.info.weapon_damage, gameObject); //Active shootable property
                    }

                    if (spread_hit.collider.GetComponent<Rigidbody>() != null) { //Check if object has a rigidbody
                        spread_hit.collider.GetComponent<Rigidbody>().AddForceAtPosition(ray_origin.forward * manager.info.bullet_force, spread_hit.point, ForceMode.Impulse); //Add an impulse to the point of contact on the object
                        Debug.DrawLine(spread_hit.point, spread_hit.point - (ray_origin.forward * manager.info.bullet_force), Color.red, 10f);
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (ray_origin) {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(ray_origin.position, normal_hit.point);
            Gizmos.DrawSphere(normal_hit.point, 0.1f);

            if(normal_hit.collider != null) {
                Handles.color = Color.green;
                RaycastHit temp;
                Physics.Raycast(ray_origin.position, ray_origin.forward + new Vector3(0, manager.info.spread_radius, 0), out temp);
                Handles.DrawWireDisc(normal_hit.point, normal_hit.normal, temp.point.y - normal_hit.point.y);
            }
        }

        //
    }

    void create_bullet_holes(RaycastHit ray)
    {
        if (ray.collider.tag != "Enemy") { //Bullet hole concept (needs brainstorming)
            GameObject obj = Instantiate(bullet_hole, ray.point, Quaternion.LookRotation(ray.normal), null);
            obj.transform.position += obj.transform.forward * 0.001f;
            obj.transform.parent = ray.collider.transform;
            bullet_hole_list.Add(obj);
            if (bullet_hole_list.Count > max_bullet_holes) {
                Destroy(bullet_hole_list[0]);
                bullet_hole_list.RemoveAt(0);
            }
        }
    }
}
