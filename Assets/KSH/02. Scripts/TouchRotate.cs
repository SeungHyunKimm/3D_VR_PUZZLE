using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchRotate : MonoBehaviour
{

    //Ray ray;
    //RaycastHit hit;

    void Start()
    {



    }
    void Update()
    {

        //LeftButtonOn();
        //VR 컨트롤러로 움직일 때
        //ray = new Ray(transform.position, transform.forward);
        //마우스로 움직일 때
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    }

    void LeftButtonOn()
    {
        //if (Physics.Raycast(ray, out hit))
        //{
        //    //print("hit");
        //    if (Input.GetMouseButtonDown(0) || OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch))
        //    {
        //        //VR로 클릭시
        //        //hit.transform.Rotate(0, 0, 90);

        //        //마우스 클릭시
        //        //transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
        //    }
        //}
    }
}
