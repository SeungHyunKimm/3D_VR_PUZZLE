using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //�÷��̾� �ӵ�
    public float speed = 3;
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
        //��ġ�� dir, speed, �ð��� ���� ������ �� �ְ� �ϰ� �ʹ�.
        transform.position += dir * speed * Time.deltaTime;
        //������ ���̽�ƽ���� ���������� Vector2�� ��������
        Vector2 joystickR = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);
        //y������ ȸ���� �����Ӱ� �ϰ� �ʹ�.
        transform.Rotate(0, joystickR.x * 3 * Time.deltaTime, 0);


    }
}
