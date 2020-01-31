using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBehav : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        for(int i = 0; i < collision.contactCount; i++) {
            if(collision.GetContact(i).collider.tag == "Player") {
                collision.GetContact(i).collider.GetComponent<MoveMonster>().Respawn();
            }
        }
    }
}
