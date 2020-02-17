using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_info : MonoBehaviour
{
    public bool unlocked = false; //Determine if the player can use it
    public int weapon_damage; //How much damage a bullet does
    public float bullet_force;
    public float spread_radius; //The maximum offset of a bullet
    public int bullet_amount; //Amount of bullet that will be shot in a single fire
    public float bullet_delay; //Delay between each fired bullet
}
