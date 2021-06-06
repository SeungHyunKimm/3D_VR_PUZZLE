using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //�÷��̾� �ӵ�
    public float speed = 3;
    public float rotateSpeed = 3;
    Ray ray;
    RaycastHit hit;
    void Start()
    {



    }

    void Update()
    {
        //���� ���̽�ƽ�� ThumbStick�� �������� Vector2�� ��������
        Vector2 joystickL = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch);
        //dir�� ���� ���̽�ƽ �����¿��� �������� �ް� ����
        Vector3 dir = transform.forward * joystickL.y + transform.right * joystickL.x;
        dir.Normalize();
        dir.y = 0;
        //��ġ�� dir, speed, �ð��� ���� ������ �� �ְ� �ϰ� �ʹ�.
        transform.position += dir * speed * Time.deltaTime;
        //������ ���̽�ƽ���� ���������� Vector2�� ��������
        Vector2 joystickR = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);
        //y������ ȸ���� �����Ӱ� �ϰ� �ʹ�.
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
