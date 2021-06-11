using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;



public class Photon_PlayerMove : MonoBehaviourPunCallbacks, IPunObservable
{

    //플레이어 속도
    public float moveSpeed = 10;
    public float RotationSpeed = 50;
    public GameObject qwertyKey;
    int buttonCnt = 0;
    public GameObject VRcam;

    //상대방 위치, 회전값
    Vector3 otherPos;
    Quaternion otherRot;




    void Start()
    {


        if (photonView.IsMine)
        {
            VRcam.SetActive(true); ;
        }
        else
        {
            VRcam.SetActive(false);
        }


    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        if (stream.IsReading)
        {
            otherPos = (Vector3)stream.ReceiveNext();
            otherRot = (Quaternion)stream.ReceiveNext();
        }
    }

    void Update()
    {




        if (photonView.IsMine)
        {

            Vector2 jsL = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch);
            Vector3 dir = transform.forward * jsL.y + transform.right * jsL.x;
            dir.Normalize();
            dir.y = 0;

            transform.position += dir * moveSpeed * Time.deltaTime;
            Vector3 jsR = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);
            transform.Rotate(0, jsR.x * RotationSpeed * Time.deltaTime, 0);

            if(PhotonNetwork.IsMasterClient)  LTouchControl();

        }

        else 
        {
            transform.position = Vector3.Lerp(transform.position, otherPos, 0.2f);
            transform.rotation = Quaternion.Lerp(transform.rotation, otherRot, 0.2f);
        }
    }


    void LTouchControl()
    {
        //로비씬에서 키보드 UI부르는 셋팅
        //L Controller의 X 버튼을 누르면 키보드 UI가 나오게끔 설정하자.
        //if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch))
        //{
        //    //키보드 UI 나타내기
        //    qwertyKey.SetActive(true);
        //    //버튼 입력 횟수
        //    buttonCnt++;
        //}

        //if (buttonCnt == 2)
        //{
        //    //키보드 UI 없애고
        //    qwertyKey.SetActive(false);
        //    //버튼 횟수 초기화하자
        //    buttonCnt = 0;

        //}


        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.LTouch))
        {
            ButtonManager.instance.settingUI.SetActive(true);
        }

    }
}
