using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate_LP : MonoBehaviour
{
      int speed = 7;

    // Update is called once per frame
    void Update()
    {
          transform.Rotate(Vector3.forward * speed * Time.deltaTime * 100);
    }
}
