using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basic_IK_Controller : MonoBehaviour
{
    Animator ani;
    public Transform left_foot;
    public Transform right_foot;
    
    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        ani.SetIKPosition(AvatarIKGoal.LeftFoot, left_foot.position);
        ani.SetIKRotation(AvatarIKGoal.LeftFoot, left_foot.rotation);
    }
}
