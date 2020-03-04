﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*Edouard*/
public class Player_Info : MonoBehaviour, Shootable
{
    public Transform health_bar; //Object will all health bar related elements
    Image main_health_bar; //Main health bar display
    Image sub_health_bar; //Chasing effect as seen when taking damage
    TextMeshProUGUI health_display; //Displays the health of the player in numbers

    public int max_health = 100; //Players health
    float sub_health; //Sub health bar effect
    int current_health; //Contains the actual health of the player

    private void Start()
    {
        sub_health_bar = health_bar.GetChild(0).GetComponent<Image>(); //Get the sub health bar
        main_health_bar = health_bar.GetChild(1).GetComponent<Image>(); //Get the main health bar
        health_display = health_bar.GetChild(2).GetComponent<TextMeshProUGUI>(); //Get the health display
        current_health = max_health; //Set the current player health to the max amount
        sub_health = current_health; //Set the sub health value to the current
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q)) {

            Damage(Random.Range(1, 99), null);
        }

        main_health_bar.fillAmount = ((float)current_health / max_health); //Fill the bar with the amount of health left
        sub_health = Mathf.Lerp(sub_health, current_health, Time.deltaTime * 2); //Calculate the current value of the sub health bar
        sub_health_bar.fillAmount = (sub_health / max_health); //Display the chasing effect of the sub health bar
        health_display.text = current_health.ToString(); //Set the display value
    }

    public void Damage(int amount, GameObject sender)
    {
        current_health -= amount; //Subtract recieved health
        current_health = Mathf.Clamp(current_health, 0, max_health); //Clamp the health so the value can't go above max_health or below 0
    }
}
