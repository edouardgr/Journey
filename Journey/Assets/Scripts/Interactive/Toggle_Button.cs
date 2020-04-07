using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toggle_Button : MonoBehaviour, Interactive
{
    public GameObject[] interactive_objs;

    public void enable()
    {
        foreach (GameObject obj in interactive_objs) {
            obj.GetComponent<Interactive>().enable();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
