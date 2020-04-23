using System.Collections;
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
        progress_value += Wrath_Bar.Get_Wrath_Bar();
        //Displays the amount of progress
        firstBar.fillAmount = (progress_value / 0.2f); //First 
        secondBar.fillAmount = ((progress_value - 0.2f) / 0.1f); //Second
        thirdBar.fillAmount = ((progress_value - 0.3f) / 0.7f); //Third

          if(progress_value >= 1) //If they hit wrath bar 100%, apply a random (good) modifier 
          {              
               InWrath = true;

               Weapon_Manager The_Manager = Player.GetComponent<Weapon_Manager>();
               var rand = new System.Random(); //random variable in csharp
               int pick_effect = rand.Next(2, 2);

               switch (pick_effect)
               {
                    case 1:
                         Enable_RapidFire(The_Manager);
                         break;
                    case 2:
                         Enable_BigDamage(The_Manager);
                         break;
                    case 3:
                         StartCoroutine(SuperAccuracy());
                         break;
               }
          }

    }
     void Enable_RapidFire(Weapon_Manager WM) { 
         
          int Max_Index = WM.max_index; 

          for(int i = 0; i < Max_Index; i++) //go through each weapon and set the shooting speed to x2
               WM.gun_pivot.transform.GetChild(i).GetComponent<Animator>().speed = 2;
               

     }
     void Disable_RapidFire(Weapon_Manager WM)
     { 
          int Max_Index = WM.max_index;

          for (int i = 0; i < Max_Index; i++)//go through each weapon and set the shooting speed to normal
               WM.gun_pivot.transform.GetChild(i).GetComponent<Animator>().speed = 1;

     }
     void Enable_BigDamage(Weapon_Manager WM) 
     {
          int Max_Index = WM.max_index;
 
          for (int i = 0; i < Max_Index; i++) { //go through each weapon and set the shooting damage to 2x
               WM.gun_pivot.transform.GetChild(i).GetComponent<Weapon_Info>().weapon_damage +=2 ;
          }
        
     }
     void Disable_BigDamage(Weapon_Manager WM)
     {
          int Max_Index = WM.max_index;

          for (int i = 0; i < Max_Index; i++) //go through each weapon and set the shooting damage to normal
               WM.gun_pivot.transform.GetChild(i).GetComponent<Weapon_Info>().weapon_damage -= 2;
     }
     void Enable_SuperAccuracy() 
     {
          
     }
     void Disable_SuperAccuracy() 
     {

          
     }
}
