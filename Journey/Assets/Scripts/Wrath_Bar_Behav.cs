﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Wrath_Bar_Behav : MonoBehaviour
{
    Image firstBar, secondBar, thirdBar;    
    [Range(0, 1)]
    public float progress_value = 0.0f;

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
        //Displays the amount of progress
        firstBar.fillAmount = (progress_value / 0.2f); //First 
        secondBar.fillAmount = ((progress_value - 0.2f) / 0.1f); //Second
        thirdBar.fillAmount = ((progress_value - 0.3f) / 0.7f); //Third
    }
}
