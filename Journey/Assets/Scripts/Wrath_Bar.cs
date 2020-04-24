using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//static class for the wrath bar, so that any class can call and add to it, as well as read the value.
public static class Wrath_Bar 
{
     public static bool Can_Be_Modified = true;

     static float Wrath_Bar_Meter = 0.0f; 
     public static float Get_Wrath_Bar() 
     {
          return Wrath_Bar_Meter;
     }
     public static void Add_Wrath_Bar(float amount) 
     {
          Wrath_Bar_Meter += amount;
          return;
     }
     public static void Sub_Wrath_Bar(float amount)
     {
          Wrath_Bar_Meter -= amount;
          return;
     }
     public static void Set_Wrath_Bar(float amount) {
          Wrath_Bar_Meter = amount;
     }


}

