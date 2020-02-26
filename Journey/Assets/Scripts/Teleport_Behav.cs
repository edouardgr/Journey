using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport_Behav : MonoBehaviour
{
    public Transform level_A, level_B;
    public GameObject player, teleport_placeholder;
    bool in_level_A = true;

    // Update is called once per frame
    void Update()
    {
        
        if (in_level_A) {
            teleport_placeholder.transform.position = (player.transform.position - level_A.position) + level_B.position;
        } else {
            teleport_placeholder.transform.position = (player.transform.position - level_B.position) + level_A.position;
        }

        if(Input.GetKeyDown(KeyCode.T)) {
            player.GetComponent<CharacterController>().enabled = false;
            Vector3 prev_pos = player.transform.position;
            player.transform.position = teleport_placeholder.transform.position;
            teleport_placeholder.transform.position = prev_pos;
            in_level_A = !in_level_A;
            player.GetComponent<CharacterController>().enabled = true;
        }
    }
}
