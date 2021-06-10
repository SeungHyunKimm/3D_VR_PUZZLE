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

        //왼쪽, 오른쪽 컨트롤러 x, b 버튼을 누르면
        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.LTouch) || OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch))
        {
            //레이를 발사한다.
            if (Physics.Raycast(ray, out hit))
            {
                //레이와 부딪치는 놈을 Keyboard UI를 터치하고 싶다.
                //key 0 - 32번까지 있음
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
