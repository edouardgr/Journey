using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class target : MonoBehaviour, Shootable
{
     public bool Value;
     public Material Activated;
     public Material Deactivated;
     public void Damage(int amount, GameObject sender)
     {
          Value = !Value;
          Change_Color();
     }

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
