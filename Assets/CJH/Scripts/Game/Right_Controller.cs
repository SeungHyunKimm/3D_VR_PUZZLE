using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Right_Controller : MonoBehaviourPun, IPunObservable
{
    public int preViewIndex;            //선택한 퍼즐의 프리 뷰
    public PuzzleManager pr;            //선택한 퍼즐의 PuzzleManager
    public float throwPower = 3;
    Vector3 controlpos, controldir, otherpos;

    Ray ray;
    RaycastHit hit;
    Transform catchObj;
    LineRenderer lr;
    public GameObject[] preView; //프리 뷰 담는 배열
    public float Speed;          //캔퍼스로 이동하는 속도

    public GameObject shot;
    public GameObject pos;
    EffectSettings es;



    GameObject SelectObj; // 선택한 놈 (승현)
    //클릭한 상태
    bool isClick;

    public CanvasManager[] cv;
    public float fusionSpeed;                //결합 속도
    //public GameObject setting;
    GameObject control;
    //Photon
    public GameObject my;    //나
    public GameObject other; //상대

    public Transform[] myBody;
    public Transform[] otherBody;

    float rotX, rotY;
    int z = 0;
    int playerPreViewIndex = 1;

    //PhotonView[] puzzles;
    GameObject[] puzzles;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            for (int i = 0; i< myBody.Length; i++)
            {
                stream.SendNext(myBody[i].position);
                stream.SendNext(myBody[i].rotation);
            }
        }

        if (stream.IsReading)
            otherpos = (Vector3)stream.ReceiveNext();
    }

    private void Awake()
    {
        GameObject puz = GameObject.Find("Puzzle");
        //puzzles = new PhotonView[puz.transform.childCount];
        puzzles = new GameObject[puz.transform.childCount];

        for (int i = 0; i < puz.transform.childCount; i++)
            //puzzles[i] = puz.transform.GetChild(i).gameObject.GetComponent<PhotonView>();         //각 퍼즐들의 포톤 뷰 정보
            puzzles[i] = puz.transform.GetChild(i).gameObject;
    }

    void Start()
    {
        GameObject pre = GameObject.Find("PreView");         //프리뷰 담기
        for (int i = 0; i < pre.transform.childCount; i++)
            preView[i] = pre.transform.GetChild(i).gameObject;

        lr = myBody[1].GetComponent<LineRenderer>();
        es = shot.GetComponent<EffectSettings>();

        cv = new CanvasManager[2];
        //포톤 생성 및 셋팅
        my.SetActive(photonView.IsMine);            //포톤 활성 비활성
        other.SetActive(!photonView.IsMine);

        GameObject canvas = GameObject.Find("Canvas");
        for (int i = 0; i < cv.Length; i++)
            cv[i] = canvas.transform.GetChild(i).GetComponent<CanvasManager>();

        if (PhotonNetwork.IsMasterClient)
        {
            transform.position = new Vector3(5, 5, -5);
            SetCanvas();
            z = 0;
            playerPreViewIndex = 0;
        }

        fusionSpeed = 10;

        control = GameObject.Find("ControlPos");
        es = shot.GetComponent<EffectSettings>();
    }

    void SetCanvas()
    {
        if (photonView.IsMine)
        {
            for (int i = 0; i < cv.Length; i++)
                cv[i].SetPuzzlePosition();
        }
    }

    void Update()
    {
        switch (ButtonManager.instance.state)
        {
            case ButtonManager.ButtonState.Start:
            case ButtonManager.ButtonState.Select:
                Start_Select_RightController();
                break;

            case ButtonManager.ButtonState.Mode_A:
                ModeA_RightController();
                break;
            case ButtonManager.ButtonState.Mode_B:
                ModeB_RightController();
                break;
            case ButtonManager.ButtonState.Mode_C:
                ModeC_RightController();
                break;
            case ButtonManager.ButtonState.Mode_D:
                ModeD_RightController();
                break;
        }
        DrawGuideLine();
    }

    void ModeA_RightController()                             //A 모드 오른손 컨트롤러
    {

        ray = new Ray(myBody[1].transform.position,myBody[1].transform.forward);

        if (Physics.Raycast(ray, out hit, 100))
        {
            PuzzleControl();
        }

        if (pr == null) return;
        if (pr.state == PuzzleManager.PuzzleState.Revolution) return;

        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
            photonView.RPC("ControlDown", RpcTarget.All);
        else if (OVRInput.Get(OVRInput.Button.One, OVRInput.Controller.RTouch))  //A 버튼 클릭 중
        {
            controldir = myBody[1].position - controlpos;
            photonView.RPC("Control", RpcTarget.All , controldir);
        }
        else if (OVRInput.GetUp(OVRInput.Button.One, OVRInput.Controller.RTouch)) //A 버튼 Up
            photonView.RPC("ControlUp", RpcTarget.All);                                                         //프리 뷰 생성 금지
    }
    void PuzzleControl()
    {
        float v = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch); //B 버튼
        //if (ButtonManager.instance.settingUI.activeSelf && v > 0)
        //{
        //    if (hit.transform.gameObject.name.Contains("Cancle"))
        //    {
        //        ButtonManager.instance.OnClickCancle();
        //    }
        //    if (hit.transform.gameObject.name.Contains("Retry"))
        //    {
        //        ButtonManager.instance.OnClickRetry();
        //    }
        //    if (hit.transform.gameObject.name.Contains("Other"))
        //    {
        //        ButtonManager.instance.OnClickOther();
        //    }
        //    return;
        //} // 만약 메뉴창이 활성화되고 Trigger버튼을 누르고 있지 않으면
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

    [PunRPC]
    void Rotate()
    {
        preView[preViewIndex].transform.Rotate(0, 0, 90);
    }

    [PunRPC]
    void FusionDown()
    {
        //왼손
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
        control.transform.position = controlpos;
    }
    [PunRPC]
    void Control(Vector3 controldir)
    {
        pr.controlpos = controldir;
    }

    [PunRPC]
    void ControlUp()
    {
        pr.GetComponent<Rigidbody>().isKinematic = true;
        pr.state = PuzzleManager.PuzzleState.Catch;
    }
    [PunRPC]
    void PreViewClose()
    {
        preView[preViewIndex].SetActive(false);
    }

    [PunRPC]
    void Catch(string name)
    {
        //for (int i = 0; i < puzzles.Length; i++)
        //{
        //    if (viewId == puzzles[i].ViewID)
        //    {
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
                cv[playerPreViewIndex].CatchToCheckBox(preViewIndex);
                puzzles[i].GetComponent<Rigidbody>().isKinematic = true; //멈추게 만들기
                puzzles[i].GetComponent<Rigidbody>().isKinematic = false;
                break;
            }
        }
        //break;
        //    }
        //}
        //shot.SetActive(true);
        //shot.transform.position = Camera.main.transform.position;
        //pos.transform.position = Camera.main.transform.position;
        //es.Target = hit.transform.gameObject;
    }
    void Shot()
    {
        shot.SetActive(true);
        shot.transform.position = transform.position;
        pos.transform.position = transform.position;
        es.Target = hit.transform.gameObject;
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
            //preView[preViewIndex].SetActive(false);
        }
    }


    void ModeB_RightController()            //B 모드 오른손 컨트롤러
    {


        CatchObj();

    }
    void ModeC_RightController()            //C 모드 오른손 컨트롤러
    {


        CatchObj();

    }
    void ModeD_RightController()            //D 모드 오른손 컨트롤러
    {


        CatchObj();

    }
    void Start_Select_RightController()         // Start & Select Mode 오른손 컨트롤러
    {
        OnClickButtonUI();
    }
    void OnClickButtonUI() //승현 StartScene에서 사용할 함수 - 버튼 누를 시 다음 씬 전환
    {
        ray = new Ray(transform.position, transform.forward);
        float v = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch); //B 버튼



        if (v > 0)
        {
            //int layer = 1 << LayerMask.NameToLayer("UI");
            //if (Physics.Raycast(ray, out hit, 1000, layer))
            if (hit.transform.gameObject.name.Contains("StartButton"))
            {
                ButtonManager.instance.OnClickStart();
            }
            if (hit.transform.gameObject.name.Contains("A_Mode"))
            {
                ButtonManager.instance.OnClickMode_A();
            }
            if (hit.transform.gameObject.name.Contains("B_Mode"))
            {
                ButtonManager.instance.OnClickMode_B();
            }
            if (hit.transform.gameObject.name.Contains("C_Mode"))
            {
                ButtonManager.instance.OnClickMode_C();
            }
            if (hit.transform.gameObject.name.Contains("D_Mode"))
            {
                ButtonManager.instance.OnClickMode_D();
            }
        }
    }
    void CatchObj()
    {
        isClick = true;
        SelectObj = null;
        //레이를 발사하여
        ray = new Ray(transform.position, transform.forward);

        float v = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch);
        //오른쪽 B버튼을 누르고 있으면

        if (ButtonManager.instance.settingUI.activeSelf && v > 0)
        {
            if (hit.transform.gameObject.name.Contains("Cancle"))
            {
                ButtonManager.instance.OnClickCancle();
            }
            if (hit.transform.gameObject.name.Contains("Retry"))
            {
                ButtonManager.instance.OnClickRetry();
            }
            if (hit.transform.gameObject.name.Contains("Other"))
            {
                ButtonManager.instance.OnClickOther();
            }
            return;
        }


        if (v > 0)
        {
            int layer = 1 << LayerMask.NameToLayer("Puzzle");
            //부딪힌 놈의 레이어가 Puzzle이면
            if (Physics.Raycast(ray, out hit, 100, layer))
            {
                print("isClick");
                SelectObj = hit.transform.gameObject;


            }
        }
        else if (v < 0)
        {
            isClick = false;
        }
        if (isClick == true && SelectObj != null)
        {
            ray = new Ray(transform.position, transform.forward);
            int layer = 1 << LayerMask.NameToLayer("Puzzle");
            if (Physics.Raycast(ray, out hit, 100, layer))
            {
                SelectObj.transform.position = hit.point;
                //hit.transform.position = new Vector3(transform.position.x, transform.position.y, 0);

            }
        }
        //문제는 오래 쥐고 있을 수록 퍼즐조각이 앞으로 다가와진다.

        if (OVRInput.Get(OVRInput.Button.One, OVRInput.Controller.RTouch))
        {

        }
        if (OVRInput.Get(OVRInput.Button.Two, OVRInput.Controller.RTouch))
        {


        }

    }
    void DropObj()
    {
        //만약 catchObj에 잡힌 물건이 없으면 이 함수를 빠져나가자
        if (catchObj == null) return;

        //오른쪽 B버튼을 떼면
        if (OVRInput.GetUp(OVRInput.Button.Two, OVRInput.Controller.RTouch))
        {
            //잡은놈이 아무것도 없다면
            catchObj.SetParent(null);
            //잡은 놈의 리지드바디를 활성화하기 위해 isKinematic을 비활성화하고
            catchObj.GetComponent<Rigidbody>().isKinematic = false;
            //던지는 함수 실행
            ThrowObj();
            //잡은놈 저장한 것 초기화
            catchObj = null;
        }
    }
    void ThrowObj()
    {
        //리지드바디 컴포넌트 가져오자
        Rigidbody rb = catchObj.GetComponent<Rigidbody>();
        //조이스틱 오른쪽 + 던지는 힘 = 블록을 앞으로 던지게끔 효과를 내보자
        rb.velocity = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch) * throwPower;

    }
}
