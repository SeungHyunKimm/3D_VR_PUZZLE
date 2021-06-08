using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Right_Controller : MonoBehaviourPun, IPunObservable
{
    public int preViewIndex;            //������ ������ ���� ��
    public PuzzleManager pr;            //������ ������ PuzzleManager
    public float throwPower = 3;
    Vector3 controlpos, controldir, otherpos;

    Ray ray;
    RaycastHit hit;
    Transform catchObj;
    LineRenderer lr;
    public GameObject[] preView; //���� �� ��� �迭
    public float Speed;          //ĵ�۽��� �̵��ϴ� �ӵ�

    public GameObject shot;
    public GameObject pos;
    EffectSettings es;



    GameObject SelectObj; // ������ �� (����)
    //Ŭ���� ����
    bool isClick;

    public CanvasManager[] cv;
    public float fusionSpeed;                //���� �ӵ�
    //public GameObject setting;
    GameObject control;
    //Photon
    public GameObject my;    //��
    public GameObject other; //���

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
            //puzzles[i] = puz.transform.GetChild(i).gameObject.GetComponent<PhotonView>();         //�� ������� ���� �� ����
            puzzles[i] = puz.transform.GetChild(i).gameObject;
    }

    void Start()
    {
        GameObject pre = GameObject.Find("PreView");         //������ ���
        for (int i = 0; i < pre.transform.childCount; i++)
            preView[i] = pre.transform.GetChild(i).gameObject;

        lr = myBody[1].GetComponent<LineRenderer>();
        es = shot.GetComponent<EffectSettings>();

        cv = new CanvasManager[2];
        //���� ���� �� ����
        my.SetActive(photonView.IsMine);            //���� Ȱ�� ��Ȱ��
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

    void ModeA_RightController()                             //A ��� ������ ��Ʈ�ѷ�
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
        else if (OVRInput.Get(OVRInput.Button.One, OVRInput.Controller.RTouch))  //A ��ư Ŭ�� ��
        {
            controldir = myBody[1].position - controlpos;
            photonView.RPC("Control", RpcTarget.All , controldir);
        }
        else if (OVRInput.GetUp(OVRInput.Button.One, OVRInput.Controller.RTouch)) //A ��ư Up
            photonView.RPC("ControlUp", RpcTarget.All);                                                         //���� �� ���� ����
    }
    void PuzzleControl()
    {
        float v = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch); //B ��ư
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
        //} // ���� �޴�â�� Ȱ��ȭ�ǰ� Trigger��ư�� ������ ���� ������
        if (v > 0)                        //Ŭ�� �Ǵ� Ŭ�� ��
            photonView.RPC("Catch", RpcTarget.All, hit.transform.name);

        if (pr == null) return;
        if (pr.state != PuzzleManager.PuzzleState.Catch)
            preView[preViewIndex].SetActive(false);

        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch))
            photonView.RPC("FusionDown", RpcTarget.All);
        else if (OVRInput.Get(OVRInput.Button.Two, OVRInput.Controller.RTouch))
        {
            if (hit.transform.name == "Canvas")                  //ĵ�۽��� �� ���� �� ����
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
        //�޼�
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
            if (preView[i].name == name)     //������ �ε��� ���� �� Catch���·� ��ȯ 
            {
                if (preView[preViewIndex].name != puzzles[i].name && pr != null)
                {
                    if (pr.state == PuzzleManager.PuzzleState.Catch || pr.state == PuzzleManager.PuzzleState.Control)
                    {
                        photonView.RPC("PreViewClose", RpcTarget.All);                //��� PC�� ���� �� ��Ȱ��ȭ
                        pr.state = PuzzleManager.PuzzleState.Revolution;
                    }
                }
                preViewIndex = i;
                pr = puzzles[i].GetComponent<PuzzleManager>();
                pr.state = PuzzleManager.PuzzleState.Catch;
                cv[playerPreViewIndex].CatchToCheckBox(preViewIndex);
                puzzles[i].GetComponent<Rigidbody>().isKinematic = true; //���߰� �����
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
        //���̿� �ε��� �����
        if (Physics.Raycast(ray, out hit))
        {
            //�Ÿ��� ���ؼ� ������ �׸���.
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


    void ModeB_RightController()            //B ��� ������ ��Ʈ�ѷ�
    {


        CatchObj();

    }
    void ModeC_RightController()            //C ��� ������ ��Ʈ�ѷ�
    {


        CatchObj();

    }
    void ModeD_RightController()            //D ��� ������ ��Ʈ�ѷ�
    {


        CatchObj();

    }
    void Start_Select_RightController()         // Start & Select Mode ������ ��Ʈ�ѷ�
    {
        OnClickButtonUI();
    }
    void OnClickButtonUI() //���� StartScene���� ����� �Լ� - ��ư ���� �� ���� �� ��ȯ
    {
        ray = new Ray(transform.position, transform.forward);
        float v = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch); //B ��ư



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
        //���̸� �߻��Ͽ�
        ray = new Ray(transform.position, transform.forward);

        float v = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch);
        //������ B��ư�� ������ ������

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
            //�ε��� ���� ���̾ Puzzle�̸�
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
        //������ ���� ��� ���� ���� ���������� ������ �ٰ�������.

        if (OVRInput.Get(OVRInput.Button.One, OVRInput.Controller.RTouch))
        {

        }
        if (OVRInput.Get(OVRInput.Button.Two, OVRInput.Controller.RTouch))
        {


        }

    }
    void DropObj()
    {
        //���� catchObj�� ���� ������ ������ �� �Լ��� ����������
        if (catchObj == null) return;

        //������ B��ư�� ����
        if (OVRInput.GetUp(OVRInput.Button.Two, OVRInput.Controller.RTouch))
        {
            //�������� �ƹ��͵� ���ٸ�
            catchObj.SetParent(null);
            //���� ���� ������ٵ� Ȱ��ȭ�ϱ� ���� isKinematic�� ��Ȱ��ȭ�ϰ�
            catchObj.GetComponent<Rigidbody>().isKinematic = false;
            //������ �Լ� ����
            ThrowObj();
            //������ ������ �� �ʱ�ȭ
            catchObj = null;
        }
    }
    void ThrowObj()
    {
        //������ٵ� ������Ʈ ��������
        Rigidbody rb = catchObj.GetComponent<Rigidbody>();
        //���̽�ƽ ������ + ������ �� = ����� ������ �����Բ� ȿ���� ������
        rb.velocity = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch) * throwPower;

    }
}
