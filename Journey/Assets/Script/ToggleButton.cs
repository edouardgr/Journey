using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleButton : MonoBehaviour, Interactable
{
    public bool active = false;
    public GameObject[] obj;

    public bool isActive()
    {
        return true;
    }

    public void Toggle()
    {
        active = !active;
        setColor();
        for (int i = 0; i < obj.Length; i++) {
            obj[i].SetActive(!obj[i].activeSelf);
            /*Interactable inter = obj[i].GetComponent<Interactable>();
            if (inter != null && inter.isActive()) {
                inter.Toggle();
            }*/
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        setColor();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void setColor()
    {
        GetComponent<SpriteRenderer>().color = (active ? Color.green : Color.red);
    }
}
