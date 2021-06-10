using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Raycast_BothHand : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;

    LineRenderer lr;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    void Update()
    {
        DrawGuideLine();
        TouchKeyboard_BothHand();
    }

    void TouchKeyboard_BothHand()
    {
        ray = new Ray(transform.position, transform.forward);

        //����, ������ ��Ʈ�ѷ� x, b ��ư�� ������
        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.LTouch) || OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch))
        {
            //���̸� �߻��Ѵ�.
            if (Physics.Raycast(ray, out hit))
            {
                //���̿� �ε�ġ�� ���� Keyboard UI�� ��ġ�ϰ� �ʹ�.
                //key 0 - 32������ ����
                print(hit.transform.gameObject.name);

            }

        }
    }

    void DrawGuideLine()
    {
        ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out hit))
        {
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, hit.point);

        }
        else
        {
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, transform.position + transform.forward * 1);
        }
    }
}
