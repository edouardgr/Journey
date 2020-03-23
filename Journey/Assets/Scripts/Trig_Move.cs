using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trig_Move : MonoBehaviour
{
    public Vector3 sin_frequency, sin_amplitude, sin_time_offset;
    public Vector3 cos_frequency, cos_amplitude, cos_time_offset;
    Vector3 origin;

    // Start is called before the first frame update
    void Start()
    {
        if (transform.parent == null) {
            origin = transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = (transform.parent ? transform.parent.position : origin)
            + new Vector3(Mathf.Sin((Time.time + sin_time_offset.x) * sin_frequency.x) * sin_amplitude.x, Mathf.Sin((Time.time + sin_time_offset.y) * sin_frequency.y) * sin_amplitude.y, Mathf.Sin((Time.time + sin_time_offset.z) * sin_frequency.z) * sin_amplitude.z)
            + new Vector3(Mathf.Cos((Time.time + cos_time_offset.x) * cos_frequency.x) * cos_amplitude.x, Mathf.Cos((Time.time + cos_time_offset.y) * cos_frequency.y) * cos_amplitude.y, Mathf.Cos((Time.time + cos_time_offset.z) * cos_frequency.z) * cos_amplitude.z);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player") {
            other.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player") {
            other.transform.SetParent(null);
        }
    }
}
