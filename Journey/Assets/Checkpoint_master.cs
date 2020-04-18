using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint_master : MonoBehaviour
{
    public Transform current_checkpoint;
    public Transform respawn_point;
    int index = -1;

    private void Start()
    {
        update_checkpoint();
    }
    
    public void update_checkpoint()
    {
        index++;
        current_checkpoint = transform.GetChild(index);
        current_checkpoint.GetChild(1).gameObject.SetActive(false);
        respawn_point = current_checkpoint.GetChild(0);
    }
}
