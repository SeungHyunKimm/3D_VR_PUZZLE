using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    public float speed = 3;
    void Start()
    {
        


    }

    void Update()
    {
        Vector2 joystickL = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch);

        Vector3 dir = transform.forward * joystickL.y + transform.right * joystickL.x;
        dir.Normalize();

        transform.position += dir * speed * Time.deltaTime;

        Vector2 joystickR = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);
        transform.Rotate(0, joystickR.x * 3 * Time.deltaTime, 0);


    }
}
