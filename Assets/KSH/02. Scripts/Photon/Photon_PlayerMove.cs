using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Photon_PlayerMove : MonoBehaviour
{

    //플레이어 속도
    public float moveSpeed = 10;
    public float RotationSpeed = 50;
    public GameObject qwertyKey;
    int buttonCnt = 0;




    void Start()
    {



    }

    void Update()
    {


        Vector2 jsL = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch);
        Vector3 dir = transform.forward * jsL.y + transform.right * jsL.x;
        dir.Normalize();
        dir.y = 0;

        transform.position += dir * moveSpeed * Time.deltaTime;
        Vector3 jsR = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);
        transform.Rotate(0, jsR.x * RotationSpeed * Time.deltaTime, 0);


        LTouchControl();
        
    
    }


    void LTouchControl()
    {
        //L Controller의 X 버튼을 누르면 키보드 UI가 나오게끔 설정하자.
        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.LTouch))
        {
            //키보드 UI 나타내기
            qwertyKey.SetActive(true);
            //버튼 입력 횟수
            buttonCnt++;
        }
        if (buttonCnt == 2)
        {
            //키보드 UI 없애고
            qwertyKey.SetActive(false);
            //버튼 횟수 초기화하자
            buttonCnt = 0;

        }
    }
}
