using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Edouard*/
public class Enemy_info : MonoBehaviour
{
    public int health; //Enemy health
    public float move_speed; //Movement speed
    public float projectile_speed; //Speed of instanced projectiles
    public float projectile_fire_rate; //Delay between each instanced projectile
    public int projectile_damage; //Amount of damage each projectile will cause
}
