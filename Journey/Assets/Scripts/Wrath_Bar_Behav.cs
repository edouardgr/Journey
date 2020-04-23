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
    bool InWrath = true;
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

          if(progress_value >= 1 && !InWrath) //If they hit wrath bar 100%, apply a random (good) modifier 
          {
              
               print("yea");
               RandomEffect();
               progress_value = 0;
              
          }
    }
     void RandomEffect()
     {
          Weapon_Manager The_Manager = Player.GetComponent<Weapon_Manager>();
          var rand = new System.Random(); //random variable in csharp
          int pick_effect = rand.Next(2, 2); 

          switch (pick_effect)
          {
               case 1:
                    StartCoroutine(RapidFire(The_Manager));
                    
                    break;
               case 2:
                    StartCoroutine(BigDamage(The_Manager));
                    
                    break;
               case 3:
                    StartCoroutine(SuperAccuracy());
                    break;
          }
          
     //slowly bring progess bar down with the timer for the effect? 30 seconds?      
     }
     IEnumerator RapidFire(Weapon_Manager WM) { //First, we need a coroutine to be able to manipulate time
         
          int Max_Index = WM.max_index; 

          for(int i = 0; i < Max_Index; i++) //go through each weapon and set the shooting speed to x2
               WM.gun_pivot.transform.GetChild(i).GetComponent<Animator>().speed = 2;
               
          yield return new WaitForSeconds(15); //wait 15 seconds

          for (int i = 0; i < Max_Index; i++)//go through each weapon and set the shooting speed to normal
               WM.gun_pivot.transform.GetChild(i).GetComponent<Animator>().speed = 1;

     }
     IEnumerator BigDamage(Weapon_Manager WM) //First, we need a coroutine to be able to manipulate time
     {
          int Max_Index = WM.max_index;
 

          for (int i = 0; i < Max_Index; i++) { //go through each weapon and set the shooting damage to 2x

               WM.gun_pivot.transform.GetChild(i).GetComponent<Weapon_Info>().weapon_damage +=2 ;
          }
          yield return new WaitForSeconds(15); //wait 15 seconds

          for (int i = 0; i < Max_Index; i++) //go through each weapon and set the shooting damage to normal
               WM.gun_pivot.transform.GetChild(i).GetComponent<Weapon_Info>().weapon_damage -= 2;
     }
     IEnumerator SuperAccuracy() //First, we need a coroutine to be able to manipulate time
     {

          yield return new WaitForSeconds(15); //wait 15 seconds
     }
}
