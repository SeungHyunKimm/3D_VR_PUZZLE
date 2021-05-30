using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchRotate : MonoBehaviour
{
    LineRenderer lr;
    void Start()
    {
        


    }
    void Update()
    {

        lr = GetComponent<LineRenderer>();
        LeftButtonOn();
        //DrawGuideLine();

    }
    //private void OnMouseDown()
    //{

    //    if (!GameControl.youWin) transform.Rotate(0, 0, 90f);
    //}
    void DrawGuideLine()
    {
        //�չٴ�(������)���� ���̸� ���
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        //���̿� �ε��� �����
        if (Physics.Raycast(ray, out hit))
        {
            //�Ÿ��� ���ؼ� ������ �׸���.
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, hit.point);
        }
        else
        {
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, transform.position + transform.forward * 1);
        }
    }


    void LeftButtonOn()
    {
        //VR ��Ʈ�ѷ��� ������ ��
        //Ray ray = new Ray(transform.position, transform.forward);
        //���콺�� ������ ��
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            //print("hit");
            if (Input.GetMouseButtonDown(0) || OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.LTouch))
            {
                print("rotate complete");
                hit.transform.Rotate(0, 0, 90f);
            }
        }
    }
}
