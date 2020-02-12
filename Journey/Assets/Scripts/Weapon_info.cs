using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_info : MonoBehaviour
{
    public bool unlocked = false; //Determine if the player can use it
    public int weapon_damage;
    public float spread_radius;
    public int bullet_amount;
    public float bullet_delay;
    //public float[] fire_delay; //Yet to implement, delay between shots, if greater than 1, then cycle through delay times (only for double barreled shotgun)
}
