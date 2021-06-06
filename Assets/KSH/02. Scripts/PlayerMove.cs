using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //플레이어 속도
    public float speed = 3;
    public float rotateSpeed = 3;
    Ray ray;
    RaycastHit hit;
    void Start()
    {



    }

    void Update()
    {
        //왼쪽 조이스틱의 ThumbStick의 움직임을 Vector2로 가져오자
        Vector2 joystickL = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch);
        //dir을 왼쪽 조이스틱 상하좌우의 움직임을 받게 하자
        Vector3 dir = transform.forward * joystickL.y + transform.right * joystickL.x;
        dir.Normalize();
        dir.y = 0;
        //위치를 dir, speed, 시간에 따라 움직일 수 있게 하고 싶다.
        transform.position += dir * speed * Time.deltaTime;
        //오른쪽 조이스틱으로 각도조절을 Vector2로 가져오자
        Vector2 joystickR = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);
        //y축으로 회전을 자유롭게 하고 싶다.
        transform.Rotate(0, joystickR.x * rotateSpeed * Time.deltaTime, 0);
        LController();
    }



    void LController()
    {
        ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out hit)&&OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.LTouch))
        {
            print("L Controller Y Button is activated");
            ButtonManager.instance.settingUI.SetActive(true);
        }
    }
}
