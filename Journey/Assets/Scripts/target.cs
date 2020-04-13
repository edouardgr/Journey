using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//child of target, used to toggle/enable our interactive objects
public class target : MonoBehaviour, Shootable
{
     public GameObject[] my_interactive;
     public bool Value;
     public Material Activated;
     public Material Deactivated;
     public void Damage(int amount, GameObject sender)
     {
          Value = !Value;
          Change_Color();

          foreach (GameObject obj in my_interactive)
          {
               obj.GetComponent<Interactive>().enable();
          }
     }
     //switches color based on activated or deactivated.
     public void Change_Color() {
          if (Value)
          {
               GetComponent<MeshRenderer>().material = Activated;

          }
          else {
               GetComponent<MeshRenderer>().material = Deactivated;
          }
     }

     private void Start()
     {
          Change_Color();
     }
}
