using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraTrack : MonoBehaviour
{
    public float max_distance = 1000f;
    public Transform target;
    float x_min, x_max, y_min, y_max;

    // Start is called before the first frame update
    void Awake()
    {
        x_min = -max_distance;
        x_max = max_distance;
        y_min = -max_distance;
        y_max = max_distance;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);

        /*LayerMask mask = LayerMask.GetMask("CameraRay");
        
        //Left
        RaycastHit2D left = Physics2D.Raycast(transform.position, -transform.right, max_distance, mask);
        if (left && left.distance > 1) {
            
            x_min = left.point.x;
        }
        //Right
        RaycastHit2D right = Physics2D.Raycast(transform.position, transform.right, max_distance, mask);
        if (right && right.distance > 1) {
            x_max = right.point.x;
        }
        //Up
        RaycastHit2D up = Physics2D.Raycast(transform.position, transform.up, max_distance, mask);
        if (up && up.distance > 1) {
            y_max = up.point.y;
        }
        //Down
        RaycastHit2D down = Physics2D.Raycast(transform.position, -transform.up, max_distance, mask);
        if (down && down.distance > 1) {
            y_min = down.point.y;
        }
        Debug.DrawLine(new Vector2(x_min, transform.position.y), new Vector2(x_max, transform.position.y), Color.red);
        Debug.DrawLine(new Vector2(transform.position.x, y_min), new Vector2(transform.position.x, y_max), Color.red);

        transform.position = new Vector3(
            Mathf.Clamp(target.position.x, x_min, x_max),
            Mathf.Clamp(target.position.y, y_min, y_max),
            transform.position.z
            );
*/
    }

    private void OnDrawGizmos()
    {

    }
}
