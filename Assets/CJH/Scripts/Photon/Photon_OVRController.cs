using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Photon_OVRController : MonoBehaviourPun, IPunObservable
{
    //���� ���� ����
    public GameObject[] preView;        //���� �� ��� �迭
    public int preViewIndex;            //������ ������ ���� �� �ε���
    public PuzzleManager pr;            //������ ������ PuzzleManager
    GameObject[] puzzles;               //���� �� ���� ��� �迭
    Vector3 controlpos, controldir;     //��Ʈ�� ���� �� ���� ���� 
    public CanvasManager[] cv;          //ĵ�۽� ��� �迭
    public float fusionSpeed;           //���� �ӵ�

    //Ray ���� ����
    Ray ray;
    RaycastHit hit;
    LineRenderer lr;

    //����Ʈ ���� ����
    public EffectSettings shotEffect;   //�߻� ����Ʈ
    public GameObject catchEffect;      //Catch ����Ʈ
    public ParticleSystem fusionEffect; //Fusion ����Ʈ
    EffectSettings es;

    //Photon ���� ����
    public GameObject my;          //��
    public GameObject other;       //���
    public Transform[] myBody;
    public Transform[] otherBody;
    int z = 1;                     //��� ��� �� ���� �� ���� z ��
    int canvasIndex = 1;           //��� ĵ�۽� ����
    Vector3 otherpos;              //���� ��ġ �� �޾ƿ���

    struct SyncData
    {
        public Vector3 pos;
        public Quaternion rotation;
    }

    SyncData[] syncData;

    //Player Part ��ġ ���� ����ȭ
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
        //Hierarchy���� ������ ã�� puzzles �迭�� ���
        GameObject puz = GameObject.Find("Puzzle");             
        puzzles = new GameObject[puz.transform.childCount];

        for (int i = 0; i < puz.transform.childCount; i++)
            puzzles[i] = puz.transform.GetChild(i).gameObject;
    }

    void Start()
    {
        //������ ���
        GameObject pre = GameObject.Find("PreView");
        for (int i = 0; i < pre.transform.childCount; i++)
            preView[i] = pre.transform.GetChild(i).gameObject;

        //LineRenderer �� ����Ʈ ����
        lr = myBody[1].GetComponent<LineRenderer>();

        //CanvasManager ũ�� ���� �� ���
        cv = new CanvasManager[2];
        GameObject canvas = GameObject.Find("Canvas");
        for (int i = 0; i < cv.Length; i++)
            cv[i] = canvas.transform.GetChild(i).GetComponent<CanvasManager>();

        //Photon_ Master , Client ���� ����
        my.SetActive(photonView.IsMine);
        other.SetActive(!photonView.IsMine);

        //������ �� �ʿ��� ���� �� ����
        if (PhotonNetwork.IsMasterClient)
            PhotonSetting();       
    }

    void PhotonSetting()    //�÷��̾� ���� �� ���� ���� ���� ����
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
        if (!photonView.IsMine)       //���� ��ġ ���� 
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

    void ModeA_RightController()                             //A ��� ������ ��Ʈ�ѷ�
    {

        ray = new Ray(myBody[1].transform.position, myBody[1].transform.forward);

        if (Physics.Raycast(ray, out hit, 100))
        {
            PuzzleRayControl();           //Ray�� �̿��� ���� Catch , Fusion ���
        }

        PuzzleClickControl();             //A ��ư���� ���� Control ���                                 
    }
    void PuzzleRayControl()
    {
        float v = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch); //B ��ư
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
                cv[canvasIndex].CatchToCheckBox(preViewIndex);
                puzzles[i].GetComponent<Rigidbody>().isKinematic = true; //���߰� �����
                puzzles[i].GetComponent<Rigidbody>().isKinematic = false;
                //shotEffect.MoveVector = pr.transform.position;
                //shotEffect.transform.gameObject.SetActive(true);
                break;
            }
        }
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
        }
    }
}
