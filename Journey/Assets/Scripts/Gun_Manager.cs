using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_Manager : MonoBehaviour
{
    public GameObject gun_pivot; //Gun_Pivot has all the guns as its children
    public int curr_index = 0; //Enables the selected weapon
    int max_index; //Highest value the index can go to

    // Start is called before the first frame update
    void Start()
    {
        max_index = gun_pivot.transform.childCount; //Get the max_amount of children;
        switch_weapon(curr_index); //Set currently selected gun to the current index
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = (int)KeyCode.Alpha1; i <= (int)KeyCode.Alpha9; i++) {
            if (Input.GetKeyDown((KeyCode)i)) {
                switch_weapon(i - (int)KeyCode.Alpha1); //Get numeric value from keyboard (1-9)
            }
        }
        float mouseWheel = Input.GetAxis("Mouse ScrollWheel"); //Cycle through weapons with mousewheel (loops)
        if(mouseWheel > 0) { //Scroll up
            curr_index++;
            if(curr_index >= max_index) { //Check if index is higher than last weapon
                curr_index = 0; //Loop to first weapon
            }
            switch_weapon(curr_index);
        } else if(mouseWheel < 0) { //Scroll down
            curr_index--;
            if(curr_index < 0) { //Check if index is lower than first weapon
                curr_index = max_index - 1; //Loop to last weapon
            }
            switch_weapon(curr_index);
        }
    }

    void switch_weapon(int _index) {
        if (_index >= max_index || _index < 0) { //Prevents selection of index great than amount of weapons
            return;
        }
        curr_index = _index; //Update current gun index to new index

        for(int i = 0; i < max_index; i++) { //Cycle through guns and disable them all
            gun_pivot.transform.GetChild(i).gameObject.SetActive(false);
        }
        gun_pivot.transform.GetChild(curr_index).gameObject.SetActive(true); //Set the current index child to show
    }
}
