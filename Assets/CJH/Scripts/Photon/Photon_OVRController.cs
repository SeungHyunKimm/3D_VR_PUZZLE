using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Photon_OVRController : MonoBehaviourPun, IPunObservable
{
    //게임 관련 변수
    public GameObject[] preView;        //프리 뷰 담는 배열
    public int preViewIndex;            //선택한 퍼즐의 프리 뷰 인덱스
    public PuzzleManager pr;            //선택한 퍼즐의 PuzzleManager
    GameObject[] puzzles;               //시작 시 퍼즐 담는 배열
    Vector3 controlpos, controldir;     //컨트롤 지점 및 방향 설정 
    public CanvasManager[] cv;          //캔퍼스 담는 배열
    public float fusionSpeed;           //결합 속도

    //Ray 관련 변수
    Ray ray;
    RaycastHit hit;
    LineRenderer lr;

    //이펙트 관련 변수
    public EffectSettings shotEffect;   //발사 이펙트
    public GameObject catchEffect;      //Catch 이펙트
    public ParticleSystem fusionEffect; //Fusion 이펙트
    EffectSettings es;

    //Photon 관련 변수
    public GameObject my;          //나
    public GameObject other;       //상대
    public Transform[] myBody;
    public Transform[] otherBody;
    int z = 1;                     //양면 모드 시 프리 뷰 생성 z 축
    int canvasIndex = 1;           //양면 캔퍼스 구분
    Vector3 otherpos;              //상대방 위치 값 받아오기

    struct SyncData
    {
        public Vector3 pos;
        public Quaternion rotation;
    }

    SyncData[] syncData;

    //Player Part 위치 각도 동기화
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            for (int i = 0; i < myBody.Length; i++)
            {
                stream.SendNext(myBody[i].position);
                stream.SendNext(myBody[i].rotation);
            }
        }
        if (stream.IsReading)
        {
            otherpos = (Vector3)stream.ReceiveNext();
            for(int i = 0; i < otherBody.Length; i++)
            {
                syncData[i].pos = (Vector3)stream.ReceiveNext();
                syncData[i].rotation = (Quaternion)stream.ReceiveNext();
            }
        }
    }

    private void Awake()
    {
        //Hierarchy상의 퍼즐을 찾아 puzzles 배열에 담기
        GameObject puz = GameObject.Find("Puzzle");             
        puzzles = new GameObject[puz.transform.childCount];

        for (int i = 0; i < puz.transform.childCount; i++)
            puzzles[i] = puz.transform.GetChild(i).gameObject;
    }

    void Start()
    {
        //프리뷰 담기
        GameObject pre = GameObject.Find("PreView");
        for (int i = 0; i < pre.transform.childCount; i++)
            preView[i] = pre.transform.GetChild(i).gameObject;

        //LineRenderer 및 이펙트 설정
        lr = myBody[1].GetComponent<LineRenderer>();

        //CanvasManager 크기 설정 및 담기
        cv = new CanvasManager[2];
        GameObject canvas = GameObject.Find("Canvas");
        for (int i = 0; i < cv.Length; i++)
            cv[i] = canvas.transform.GetChild(i).GetComponent<CanvasManager>();

        //Photon_ Master , Client 구분 짓기
        my.SetActive(photonView.IsMine);
        other.SetActive(!photonView.IsMine);

        //방장일 때 필요한 변수 값 조정
        if (PhotonNetwork.IsMasterClient)
            PhotonSetting();       
    }

    void PhotonSetting()    //플레이어 입장 후 게임 관련 변수 설정
    {
        transform.position = new Vector3(5, 5, -5);
        transform.rotation = Quaternion.identity;
        z = 0;
        canvasIndex = 0;
        if (photonView.IsMine)
        {
            for (int i = 0; i < cv.Length; i++)
                cv[i].SetPuzzlePosition();
        }
    }

    void Update()
    {
        if (!photonView.IsMine)       //상대방 위치 셋팅 
        {
            transform.position = Vector3.Lerp(transform.position, otherpos, 0.05f);
            for (int i = 0; i < otherBody.Length; i++)
            {
                otherBody[i].position = Vector3.Lerp(otherBody[i].transform.position, syncData[i].pos, 0.05f);
                otherBody[i].rotation = Quaternion.Lerp(otherBody[i].transform.rotation, syncData[i].rotation, 0.05f);
            }
            return;
        }
        ModeA_RightController();
        DrawGuideLine();
    }

    void ModeA_RightController()                             //A 모드 오른손 컨트롤러
    {

        ray = new Ray(myBody[1].transform.position, myBody[1].transform.forward);

        if (Physics.Raycast(ray, out hit, 100))
        {
            PuzzleRayControl();           //Ray를 이용한 퍼즐 Catch , Fusion 담당
        }

        PuzzleClickControl();             //A 버튼으로 퍼즐 Control 담당                                 
    }
    void PuzzleRayControl()
    {
        float v = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch); //B 버튼
        if (v > 0)                        //클릭 또는 클릭 중
            photonView.RPC("Catch", RpcTarget.All, hit.transform.name);

        if (pr == null) return;
        if (pr.state != PuzzleManager.PuzzleState.Catch)
            preView[preViewIndex].SetActive(false);

        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch))
            photonView.RPC("FusionDown", RpcTarget.All);
        else if (OVRInput.Get(OVRInput.Button.Two, OVRInput.Controller.RTouch))
        {
            if (hit.transform.name == "Canvas")                  //캔퍼스일 때 프리 뷰 생성
            {
                int x = (int)hit.point.x;
                int y = (int)hit.point.y;
                photonView.RPC("Fusion", RpcTarget.All, x, y);
            }
        }
        else if (OVRInput.GetUp(OVRInput.Button.Two, OVRInput.Controller.RTouch))
            photonView.RPC("FusionUp", RpcTarget.All);
    }

    void PuzzleClickControl()
    {
        if (pr == null) return;
        if (pr.state == PuzzleManager.PuzzleState.Revolution) return;

        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
            photonView.RPC("ControlDown", RpcTarget.All);
        else if (OVRInput.Get(OVRInput.Button.One, OVRInput.Controller.RTouch))
        {
            controldir = myBody[1].position - controlpos;
            photonView.RPC("Control", RpcTarget.All, controldir);
        }
        else if (OVRInput.GetUp(OVRInput.Button.One, OVRInput.Controller.RTouch))
            photonView.RPC("ControlUp", RpcTarget.All);

        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.LTouch))
            photonView.RPC("Rotate", RpcTarget.All);
    }

    [PunRPC]
    void Rotate()
    {
        preView[preViewIndex].transform.Rotate(0, 0, 90);
    }

    [PunRPC]
    void FusionDown()
    {
        preView[preViewIndex].SetActive(true);
    }

    [PunRPC]
    void Fusion(int x, int y)
    {
        preView[preViewIndex].transform.position = new Vector3(x, y, z);
    }

    [PunRPC]
    void BlockMove(Vector3 dir)
    {
        pr.Move(dir, fusionSpeed);
        pr.SetPreViewXYZ(preView[preViewIndex]);
        pr = null;
    }

    [PunRPC]
    void FusionUp()
    {
        if (pr.state == PuzzleManager.PuzzleState.Catch || pr.state == PuzzleManager.PuzzleState.Control)
        {
            pr.state = PuzzleManager.PuzzleState.Fusion;
            if (photonView.IsMine)
            {
                Vector3 dir = preView[preViewIndex].transform.position - pr.transform.position;
                photonView.RPC("BlockMove", RpcTarget.All, dir);
            }
        }
        PreViewClose();
    }
    [PunRPC]
    void ControlDown()
    {
        pr.state = PuzzleManager.PuzzleState.Control;
        controlpos = myBody[1].transform.position;
    }
    [PunRPC]
    void Control(Vector3 controldir)
    {
        pr.controlpos = controldir;
        catchEffect.SetActive(true);
        catchEffect.transform.position = pr.transform.position;
    }

    [PunRPC]
    void ControlUp()
    {
        pr.GetComponent<Rigidbody>().isKinematic = true;
        pr.state = PuzzleManager.PuzzleState.Catch;
        catchEffect.SetActive(false);
        fusionEffect.transform.position = pr.transform.position;
        fusionEffect.Play();
    }
    [PunRPC]
    void PreViewClose()
    {
        preView[preViewIndex].SetActive(false);
    }

    [PunRPC]
    void Catch(string name)
    {
        for (int i = 0; i < preView.Length; i++)
        {
            if (preView[i].name == name)     //프리뷰 인덱스 저장 및 Catch상태로 변환 
            {
                if (preView[preViewIndex].name != puzzles[i].name && pr != null)
                {
                    if (pr.state == PuzzleManager.PuzzleState.Catch || pr.state == PuzzleManager.PuzzleState.Control)
                    {
                        photonView.RPC("PreViewClose", RpcTarget.All);                //모든 PC에 프리 뷰 비활성화
                        pr.state = PuzzleManager.PuzzleState.Revolution;
                    }
                }
                preViewIndex = i;
                pr = puzzles[i].GetComponent<PuzzleManager>();
                pr.state = PuzzleManager.PuzzleState.Catch;
                cv[canvasIndex].CatchToCheckBox(preViewIndex);
                puzzles[i].GetComponent<Rigidbody>().isKinematic = true; //멈추게 만들기
                puzzles[i].GetComponent<Rigidbody>().isKinematic = false;
                //shotEffect.MoveVector = pr.transform.position;
                //shotEffect.transform.gameObject.SetActive(true);
                break;
            }
        }
    }
    void DrawGuideLine()
    {
        //레이와 부딪힌 놈까지
        if (Physics.Raycast(ray, out hit))
        {
            //거리를 구해서 라인을 그린다.
            lr.SetPosition(0, myBody[1].transform.position);
            lr.SetPosition(1, hit.point);
        }
        else
        {
            lr.SetPosition(0, myBody[1].transform.position);
            lr.SetPosition(1, myBody[1].transform.position + myBody[1].transform.forward * 1);
        }
    }
}
