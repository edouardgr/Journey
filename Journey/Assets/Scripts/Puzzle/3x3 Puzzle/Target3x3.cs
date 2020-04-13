using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//child of target, used to toggle/enable our interactive objects
public class Target3x3: MonoBehaviour, Shootable
{
     public bool Value = false;
     public int Button_Number;
     public Material Activated;
     public Material Deactivated;
     public GameObject Object_To_Activate;

     //basically when a bullet hits the target, in this case does not take damage but rather activates the button
     public void Damage(int amount, GameObject sender)
     {
          Value = !Value;
          Change_Color();
          //if object is set to activated, store a 1 in array, else store a 0
          Object_To_Activate.GetComponent<ComeToView>().List_Of_Buttons[Button_Number-1] = (Value ? 1 : 0);
          

     }
     //switches color based on activated or deactivated.
     public void Change_Color()
     {
          if (Value)
          {
               GetComponent<MeshRenderer>().material = Activated;

          }
          else
          {
               GetComponent<MeshRenderer>().material = Deactivated;
          }
     }

     private void Start()
     {
          Change_Color();
     }
}
