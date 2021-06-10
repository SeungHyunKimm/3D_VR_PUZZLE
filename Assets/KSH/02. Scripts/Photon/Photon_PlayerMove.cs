using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Photon_PlayerMove : MonoBehaviour
{

    //�÷��̾� �ӵ�
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
        //L Controller�� X ��ư�� ������ Ű���� UI�� �����Բ� ��������.
        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.LTouch))
        {
            //Ű���� UI ��Ÿ����
            qwertyKey.SetActive(true);
            //��ư �Է� Ƚ��
            buttonCnt++;
        }
        if (buttonCnt == 2)
        {
            //Ű���� UI ���ְ�
            qwertyKey.SetActive(false);
            //��ư Ƚ�� �ʱ�ȭ����
            buttonCnt = 0;

        }
    }
}
