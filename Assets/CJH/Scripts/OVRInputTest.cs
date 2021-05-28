using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OVRInputTest : MonoBehaviour
{
    public Transform rightHandle;
    public Transform dot;
    LineRenderer line;
    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(rightHandle.position, rightHandle.forward);
        RaycastHit hit;

        if(Physics.Raycast(ray , out hit))
        {
            dot.position = hit.point;
        }

        // 왼쪽 컨트롤러의 두 번째 키
        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.LTouch)) { }

        // 왼쪽 컨트롤러의 높이 값 반환
        Vector2 v = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch);

        DrawGuideLine();
        CatchObject();
    }

    void CatchObject()
    {
        line.SetPosition(0, transform.position);  
    }

    void DrawGuideLine()
    {
        // 속도 셋팅
        //OVRInput.GetLocalControllerVelocity
    }

}
