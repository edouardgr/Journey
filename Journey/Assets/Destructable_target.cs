using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable_target : MonoBehaviour, Shootable
{
    public List<GameObject> remove_obj;
    int shoot_count = 1;

    public void Damage(int amount, GameObject sender) {
        shoot_count--;

        if(shoot_count >= 0) {
            return;
        }

        foreach(GameObject obj in remove_obj) {
            Destroy(obj);
        }
        //Destroy(transform.gameObject);
    }
}
