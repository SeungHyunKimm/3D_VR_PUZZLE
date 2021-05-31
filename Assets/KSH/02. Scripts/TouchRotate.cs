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
        //손바닥(오른손)에서 레이를 쏜다
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        //레이와 부딪힌 놈까지
        if (Physics.Raycast(ray, out hit))
        {
            //거리를 구해서 라인을 그린다.
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
        //VR 컨트롤러로 움직일 때
        //Ray ray = new Ray(transform.position, transform.forward);
        //마우스로 움직일 때
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
