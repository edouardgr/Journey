using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Manager : MonoBehaviour
{
    public GameObject gun_pivot; //Gun_Pivot has all the guns as its children
    public int curr_index = -1; //Enables the selected weapon
    int max_index; //Highest value the index can go to
    public Weapon_Info info = null;
    //float switch_delay = 0.5f; //Delay between switching guns, prevents quick spamming //FUTURE IDEA

    // Start is called before the first frame update
    void Start()
    {
        max_index = gun_pivot.transform.childCount; //Get the max_amount of children;
        if (unlocked_count() > 0) { //Check if any guns are available
            get_next_weapon_index(true); //Get the first available gun
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!gun_pivot.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle")) { //Prevents quick swapping between weapons, remove for fun
            return;
        }
        
        for (int i = (int)KeyCode.Alpha1; i <= (int)KeyCode.Alpha9; i++) {
            if (Input.GetKeyDown((KeyCode)i) && (i - (int)KeyCode.Alpha1) != curr_index) {
                switch_weapon(i - (int)KeyCode.Alpha1); //Get numeric value from keyboard (1-9)
            }
        }
        float mouseWheel = Input.GetAxis("Mouse ScrollWheel"); //Cycle through weapons with mousewheel (loops)
        if (unlocked_count() > 0) { //Check if guns are available, infinite loop if not checked, learned the hard way :/
            if (mouseWheel > 0) { //Scroll up
                get_next_weapon_index(true);
            } else if (mouseWheel < 0) { //Scroll down
                get_next_weapon_index(false);
            }
        }
    }

    public int unlocked_count() { //Return number of guns that are unlocked
        int count = 0;
        for(int i = 0; i < gun_pivot.transform.childCount; i++) { //Cycle through weapons
            if(gun_pivot.transform.GetChild(0).GetComponent<Weapon_Info>().unlocked) { //Check if unlocked
                count++;
            }
        }
        return count;
    }

    void get_next_weapon_index(bool search_up) { //Update the curr_index by searching the next unlocked weapon
        int i = curr_index;
        while (true) { //Keep looping until we find a valid index
            i += (search_up ? 1 : -1); //Increment or decrement based on parameter
            if (i < 0) { i = max_index - 1; }//Prevent index from going out of bounds
            i %= max_index; //Prevent index from going out of bounds
            if (gun_pivot.transform.GetChild(i).GetComponent<Weapon_Info>().unlocked) { //Check if weapon is unlocked 
                break;
            }
        }
        if(curr_index != i) { //If the same index was found, do nothing
            switch_weapon(i); //Switch to weapon
        }
    }

    public void switch_weapon(int _index) {
        if (_index >= max_index || _index < 0 || !gun_pivot.transform.GetChild(_index).GetComponent<Weapon_Info>().unlocked) { //Prevents selection of index great than amount of weapons and that weapon is unlocked
            return;
        }
        curr_index = _index; //Update current gun index to new index

        for(int i = 0; i < max_index; i++) { //Cycle through guns and disable them all
            gun_pivot.transform.GetChild(i).gameObject.SetActive(false);
        }
        gun_pivot.transform.GetChild(curr_index).gameObject.SetActive(true); //Set the current index child to show
        gun_pivot.GetComponent<Animator>().Play("Weapon_load", 0); //Play equiping animation for selected gun
        info = gun_pivot.transform.GetChild(_index).GetComponent<Weapon_Info>(); //Save the info attached to the selected weapon
    }
}
