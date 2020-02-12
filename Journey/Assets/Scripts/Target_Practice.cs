using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target_Practice : MonoBehaviour, Shootable
{
    float time_swap = 2f;
    float time = 0;
    int active_index = 0;
    bool[] hit;
    float scale_factor = 2f;

    public void Damage(int amount)
    {
        Debug.Log(active_index);
        time = 0;
        hit[active_index] = true;
        transform.GetChild(active_index).GetComponent<MeshCollider>().enabled = false;
        scale_factor /= 1.25f;
        for (int i = 0; i < hit.Length; i++) {
            if (!hit[i]) {
                transform.GetChild(i).localScale = new Vector3(scale_factor, 0.1f, scale_factor);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        reset();
    }

    // Update is called once per frame
    void Update()
    {
        int count = 0;
        for(int i = 0; i < hit.Length; i++) {
            if(hit[i]) {
                count++;
            }
        }
        if(count == hit.Length) {
            reset();
        }

        if (time <= 0) {
            time = time_swap;
            for (int i = 0; i < hit.Length; i++) {
                if (!hit[i]) {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
            }
            do {
                active_index = Random.Range(0, 5);
            } while (hit[active_index]);
            transform.GetChild(active_index).gameObject.SetActive(true);
        }
        time -= Time.deltaTime;
    }

    void reset()
    {
        scale_factor = 2f;
        hit = new bool[transform.childCount];
        for (int i = 0; i < hit.Length; i++) {
            hit[i] = false;
            transform.GetChild(i).localScale = new Vector3(scale_factor, 0.1f, scale_factor);
            transform.GetChild(i).gameObject.SetActive(false);
            transform.GetChild(i).GetComponent<MeshCollider>().enabled = true;
        }
        time = 0;
    }
}
