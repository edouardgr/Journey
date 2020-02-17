using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_info : MonoBehaviour, Shootable
{
    public int health = 100;

    public void Damage(int amount, GameObject sender)
    {
        health -= amount;
    }
}
