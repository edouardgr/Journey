using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Edouard*/
public class Player_Info : MonoBehaviour, Shootable
{
    public int health = 100; //Players health

    public void Damage(int amount, GameObject sender)
    {
        health -= amount; //Subtract recieved health
    }
}
