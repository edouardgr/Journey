using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_shooter : MonoBehaviour
{
    public Weapon_Manager manager;
    RaycastHit hit;
    Transform ray_origin;

    // Start is called before the first frame update
    void Awake()
    {
        manager = GetComponent<Weapon_Manager>();
        ray_origin = transform.GetChild(0).transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(ray_origin.position, ray_origin.forward, out hit)) {

        }

        //Weapon interaction
        if (manager.unlocked_count() <= 0 || !manager.gun_pivot.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle")) { //If no weapons are equiped, don't run
            return; //Early exit
        }

        Animator ani = manager.gun_pivot.transform.GetChild(manager.curr_index).GetComponent<Animator>(); //Get animator of current weapon
        if (Input.GetMouseButtonDown(0) && ani.GetCurrentAnimatorStateInfo(0).IsName("Idle")) { //Left mouse button is pressed & animation is complete
            ani.Play("Fire", 0); //Play firing animation for the equiped weapon

            if (hit.collider != null && hit.collider.GetComponentInParent<Shootable>() != null) { //Detect if hit object has a shootable property
                hit.collider.GetComponentInParent<Shootable>().Damage(manager.info.weapon_damage); //Active shootable property
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (ray_origin) {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(ray_origin.position, hit.point);
            Gizmos.DrawSphere(hit.point, 0.1f);
        }
    }
}
