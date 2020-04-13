using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComeToView : MonoBehaviour, Interactive
{
     public int[] List_Of_Buttons;
     public GameObject[] List_Of_Interactables;
     bool Activated;
     Animation ani;

    void Start()
    {
          List_Of_Buttons = new int[9];

          for (int i = 0; i < List_Of_Buttons.Length; i++)
          {
               List_Of_Buttons[i] = 0;
          }


        ani = GetComponent<Animation>();
        Activated = false;
    }
    void Update()
    {
          //if the buttons are pressed in this arrangment, enable the puzzle
          // 0 1 0
          // 0 1 0
          // 1 1 1
          if ((!Activated) && List_Of_Buttons[0] == 0 && List_Of_Buttons[1] == 1 && List_Of_Buttons[2] == 0 && List_Of_Buttons[3] == 0 &&
                 List_Of_Buttons[4] == 1 && List_Of_Buttons[5] == 0 && List_Of_Buttons[6] == 1 
                 && List_Of_Buttons[7] == 1 && List_Of_Buttons[8] == 1) 
          {
               enable();
               Activated = true;
          }

    }

     public void enable()
     {
          ani.Play("ComeToView Doorway");
          for (int i = 0; i < List_Of_Interactables.Length; i++) { 

          }
     }
}
