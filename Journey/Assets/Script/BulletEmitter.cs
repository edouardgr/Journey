using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEmitter : MonoBehaviour
{
    public GameObject bullet_prefab;
    MoveMonster player_move;
    public float bullet_speed = 30f;
    public float cool_down_time = 1f;
    public float cool_down_timer = 0f;

    private void Awake()
    {
        player_move = transform.parent.GetComponent<MoveMonster>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.x, (player_move.facing_right ? 180 : 0), transform.rotation.z);

        if(cool_down_timer > 0) {
            cool_down_timer -= Time.deltaTime;
        }

        if(Input.GetButtonDown("Fire1") && cool_down_timer <= 0) {
            GameObject obj = Instantiate(bullet_prefab, transform.GetChild(0).position, transform.parent.rotation, null);
            cool_down_timer = cool_down_time;
            obj.GetComponent<Rigidbody2D>().velocity = new Vector2((player_move.facing_right ? 1 : -1) * bullet_speed, 0);
        }
    }
}
