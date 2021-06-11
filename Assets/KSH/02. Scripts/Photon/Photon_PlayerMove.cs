using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;



public class Photon_PlayerMove : MonoBehaviourPunCallbacks, IPunObservable
{

    //�÷��̾� �ӵ�
    public float moveSpeed = 10;
    public float RotationSpeed = 50;
    public GameObject qwertyKey;
    int buttonCnt = 0;
    public GameObject VRcam;

    //���� ��ġ, ȸ����
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
        //�κ������ Ű���� UI�θ��� ����
        //L Controller�� X ��ư�� ������ Ű���� UI�� �����Բ� ��������.
        //if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch))
        //{
        //    //Ű���� UI ��Ÿ����
        //    qwertyKey.SetActive(true);
        //    //��ư �Է� Ƚ��
        //    buttonCnt++;
        //}

        //if (buttonCnt == 2)
        //{
        //    //Ű���� UI ���ְ�
        //    qwertyKey.SetActive(false);
        //    //��ư Ƚ�� �ʱ�ȭ����
        //    buttonCnt = 0;

        //}


        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.LTouch))
        {
            ButtonManager.instance.settingUI.SetActive(true);
        }

    }
}
