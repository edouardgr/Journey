﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Wrath_Bar_Behav : MonoBehaviour
{
     Image firstBar, secondBar, thirdBar;
     [Range(0, 1)]
     public float progress_value = 0.0f;
     public GameObject Player;
     bool InWrath = false;
     int pick_effect;
     //time stuff
     float time = 0f;
     public float run_time = 10f;
     public AudioClip sound;
     
     // Start is called before the first frame update
     void Start()
     {
          firstBar = transform.GetChild(0).GetComponent<Image>();
          secondBar = transform.GetChild(1).GetComponent<Image>();
          thirdBar = transform.GetChild(2).GetComponent<Image>();

     }

     // Update is called once per frame
     void Update()
     {
          progress_value = Wrath_Bar.Get_Wrath_Bar();

          //Displays the amount of progress
          firstBar.fillAmount = (progress_value / 0.2f); //First 
          secondBar.fillAmount = ((progress_value - 0.2f) / 0.1f); //Second
          thirdBar.fillAmount = ((progress_value - 0.3f) / 0.7f); //Third

          var rand = new System.Random(); //random variable in csharp
         
          Weapon_Manager The_Manager = Player.GetComponent<Weapon_Manager>();

          if (Wrath_Bar.Get_Wrath_Bar() >= 1 && (InWrath == false)) //If they hit wrath bar 100%, apply a random (good) modifier.
          {

               pick_effect = rand.Next(1, 4); //random number between 1 and 3 inclusive.               
               InWrath = true;
               Wrath_Bar.Can_Be_Modified = false; //Make sure nothings can add to the wrath bar after its already started.
               Player.GetComponent<AudiioQueue>().queue.Add(sound);
               //pick a random effect
               switch (pick_effect)
               {
                    case 1:
                         Enable_RapidFire(The_Manager);
                         break;
                    case 2:
                         Enable_BigDamage(The_Manager);
                         break;
                    case 3:
                         Enable_SuperAccuracy();
                         break;
               }
          }
          if (InWrath)
          {
               //If current time less then time we want to run till, add to time and update the wrath bar.
               if (time < run_time)
               {                   
                    time += Time.deltaTime;
                    Wrath_Bar.Set_Wrath_Bar((run_time - time) / run_time);
               }
               else
               {
                    time = 0; //reset the time to 0, for progress bar.
                    InWrath = false;
                    Wrath_Bar.Can_Be_Modified = true; //Make so killing enemies re adds to the wrath bar.
                    Wrath_Bar.Set_Wrath_Bar(0); //reset the bar back to 0 so that when you kill enemies it adds back up evenly.
                    switch (pick_effect)
                    {
                         case 1:
                              Disable_RapidFire(The_Manager);
                              break;
                         case 2:
                              Disable_BigDamage(The_Manager);
                              break;
                         case 3:
                              Disable_SuperAccuracy();
                              break;
                    }
               }
          }


     }
     void Enable_RapidFire(Weapon_Manager WM)
     {
          transform.GetChild(4).gameObject.SetActive(true);

          int Max_Index = WM.max_index;

          for (int i = 0; i < Max_Index; i++) //Go through each weapon and set the shooting speed to x2
               WM.gun_pivot.transform.GetChild(i).GetComponent<Animator>().speed = 2;
     }
     void Disable_RapidFire(Weapon_Manager WM)
     {
          transform.GetChild(4).gameObject.SetActive(false);
          int Max_Index = WM.max_index;

          for (int i = 0; i < Max_Index; i++) //Go through each weapon and set the shooting speed to normal
               WM.gun_pivot.transform.GetChild(i).GetComponent<Animator>().speed = 1;
     }
     void Enable_BigDamage(Weapon_Manager WM)
     {
          transform.GetChild(3).gameObject.SetActive(true);
          int Max_Index = WM.max_index;

          for (int i = 0; i < Max_Index; i++) //Go through each weapon and set the shooting damage to 2x
          { 
               WM.gun_pivot.transform.GetChild(i).GetComponent<Weapon_Info>().weapon_damage *= 2;
          }
     }
     void Disable_BigDamage(Weapon_Manager WM)
     {
          transform.GetChild(3).gameObject.SetActive(false);
          int Max_Index = WM.max_index;

          for (int i = 0; i < Max_Index; i++) //Go through each weapon and set the shooting damage to normal
               WM.gun_pivot.transform.GetChild(i).GetComponent<Weapon_Info>().weapon_damage /= 2;
     }
     void Enable_SuperAccuracy()
     {
          transform.GetChild(5).gameObject.SetActive(true);
          Player.GetComponent<Weapon_Shooter>().perfect_aim = true;
     }
     void Disable_SuperAccuracy()
     {
          transform.GetChild(5).gameObject.SetActive(false);
          Player.GetComponent<Weapon_Shooter>().perfect_aim = false;

     }
}
