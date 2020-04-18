using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint_master : MonoBehaviour
{
    public Transform current_checkpoint;
    int index = 0;

    private void Start()
    {
        current_checkpoint = transform.GetChild(index);
    }
    
    public void update_checkpoint()
    {
        index++;
        current_checkpoint.GetChild(1).gameObject.SetActive(false);
        current_checkpoint = transform.GetChild(index);
    }
}
