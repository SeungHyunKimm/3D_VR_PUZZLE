using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PC_PlayerControl : MonoBehaviourPun, IPunObservable
{
    public int preViewIndex;                   //������ ������ �ε���
    public PuzzleManager pr;
    public CanvasManager[] cv;
    public GameObject[] preView;       //���� ������ �迭
    Ray ray;
    RaycastHit hit;
    public float fusionSpeed;                //���� �ӵ�
    //public EffectSettings shotEffect;   //�߻� ����Ʈ
    public GameObject catchEffect;      //Catch ����Ʈ
    public ParticleSystem fusionEffect; //Fusion ����Ʈ
    //public GameObject setting;
    GameObject control;
    Vector3 controlpos;
    //Photon
    public GameObject my;    //��
    public GameObject other; //���
    public Transform[] myBody;
    public Transform[] otherBody;
    float rotX, rotY;
    int z = 1;
    int playerPreViewIndex = 1;
    PC_AIPlayerControl AI;

    //PhotonView[] puzzles;
    //GameObject[] puzzles;
    PuzzleManager[] puzzles;
    string puzzle;
    public int k = 0;

    struct SyncData
    {
        public Vector3 pos;
        public Quaternion rotation;
    }

    SyncData[] syncData;
    // Start is called before the first frame update
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
            for (int i = 0; i < otherBody.Length; i++)
            {
                syncData[i].pos = (Vector3)stream.ReceiveNext();
                otherBody[i].rotation = (Quaternion)stream.ReceiveNext();
            }
        }
    }
    private void Awake()
    {
        //puzzles = new PhotonView[puz.transform.childCount];
        GameObject puz = GameObject.Find("Puzzle");
        puzzles = new PuzzleManager[puz.transform.childCount];

        for (int i = 0; i < puz.transform.childCount; i++)
            //puzzles[i] = puz.transform.GetChild(i).gameObject.GetComponent<PhotonView>();         //�� ������� ���� �� ����
            //puzzles[i] = puz.transform.GetChild(i).gameObject;
            puzzles[i] = puz.transform.GetChild(i).GetComponent<PuzzleManager>();
    }
    void Start()
    {
        if (photonView.IsMine == false)
            syncData = new SyncData[myBody.Length];

        //���� ���� �� ����
        my.SetActive(photonView.IsMine);            //���� Ȱ�� ��Ȱ��
        other.SetActive(!photonView.IsMine);

        GameObject canvas = GameObject.Find("Canvas");
        cv = new CanvasManager[canvas.transform.childCount];
        for (int i = 0; i < cv.Length; i++)
            cv[i] = canvas.transform.GetChild(i).GetComponent<CanvasManager>();

        if (PhotonNetwork.IsMasterClient)
        {
            //transform.position = new Vector3(5, 5, -5);
            transform.rotation = new Quaternion(0, 0, 0, 1);
            SetCanvas();
            z = 0;
            playerPreViewIndex = 0;
        }
        else photonView.RPC("Client", RpcTarget.MasterClient);

        GameObject pre = GameObject.Find("PreView");         //������ ���
        for (int i = 0; i < pre.transform.childCount; i++)
            preView[i] = pre.transform.GetChild(i).gameObject;

        fusionSpeed = 25;

        control = GameObject.Find("ControlPos");

        //StartCoroutine(AIStartAfter_Two());
        GameObject aiGO = GameObject.Find("AIPlayer");
        //AI = aiGO.GetComponent<PC_AIPlayerControl>();
        //AI.enabled = true;
    }

    IEnumerator AIStartAfter_Two()
    {
        yield return new WaitForSeconds(2);
        AI.enabled = true;
    }

    void SetCanvas()
    {
        if (photonView.IsMine)
        {
            //for (int i = 0; i < cv.Length; i++)
            //{
                int index = puzzles.Length / 2;
                cv[0].SetPuzzlePosition(index);
            //}
        }
    }
    bool client = false;
    [PunRPC]
    void Client()
    {
        client = true;
    }

    // Update is called once per frame
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
        switch (ButtonManager.instance.state)
        {
            case ButtonManager.ButtonState.Mode_A:
                PlayerMove();
                ModeA_RightController();
                break;
            case ButtonManager.ButtonState.Mode_B:
                break;
            case ButtonManager.ButtonState.Mode_C:
                break;
            case ButtonManager.ButtonState.Mode_D:
                break;
        }
    }
    void PlayerMove()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        
        transform.position += (transform.forward * z  + transform.right * x) * 5 * Time.deltaTime;

        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        rotX += mx;
        rotY += my;

        transform.eulerAngles = new Vector3(-rotY, rotX, 0);
    }
    void ModeA_RightController()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (Input.GetMouseButtonDown(0))                    //���������� Ŭ�� �� ���߰� 
            {
                //print(hit.transform.name);
                //if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Puzzle"))
                //{
                //    photonView.RPC("Shot", RpcTarget.All, hit.point, pv.ViewID);
                //}
                //PhotonView pv = hit.transform.GetComponent<PhotonView>();
                //if (pv == null)
                //{
                //    pv = hit.transform.parent.GetComponent<PhotonView>();
                //}
                //if (pv != null)
                //{
                //    photonView.RPC("Catch", RpcTarget.All, hit.point, pv.ViewID);
                //}
                photonView.RPC("Catch", RpcTarget.All, hit.transform.GetComponent<PuzzleManager>());
                //Shot();
                //PuzzleChoiceChange(hit.transform.gameObject);
            }

            if (pr == null) return;
            if (pr.state != PuzzleManager.PuzzleState.Catch)
                preView[preViewIndex].SetActive(false);

            if (Input.GetMouseButtonDown(1))
            {
                //�޼�
                //preView[preViewIndex].SetActive(true);
                photonView.RPC("FusionDown", RpcTarget.All);
            }
            else if (Input.GetMouseButton(1))
            {
                if (hit.transform.gameObject.name == "Canvas")  //������ ���� ��ġ ���� ĵ���� ����
                {
                    int x = (int)hit.point.x;
                    int y = (int)hit.point.y;
                    photonView.RPC("Fusion", RpcTarget.All, x, y);
                }
            }
            else if (Input.GetMouseButtonUp(1))                //���� �� ��ġ�� ��ü ������ �� ������ ��Ȱ��ȭ
            {
                photonView.RPC("FusionUp", RpcTarget.All);
            }
        }
        if (pr == null) return;
        if (pr.state == PuzzleManager.PuzzleState.Revolution) return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            photonView.RPC("ControlDown", RpcTarget.All);
        }
        if (Input.GetKey(KeyCode.Space))               //���� ��ü�� �������
        {
            controldir = transform.position - controlpos;
            photonView.RPC("Control", RpcTarget.All, controldir);
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            photonView.RPC("ControlUp", RpcTarget.All);
        }
        else if (Input.GetKeyDown(KeyCode.Z))
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
        controlpos = Camera.main.transform.position;
        control.transform.position = controlpos;
    }

    Vector3 controldir;
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
    void Catch(PuzzleManager pz)
    {
        //for (int i = 0; i < puzzles.Length; i++)
        //{
        //    if (viewId == puzzles[i].ViewID)
        //    {
        //preViewIndex = pz.puzzleIndex;
        //pr = pz;
        //if (preViewIndex / (puzzles.Length / cv.Length) == playerPreViewIndex)  //�ڽ��� �ǿ� �´� ������ ���� üũ ���� ȣ��
        //    cv[playerPreViewIndex].CatchToCheckBox(preViewIndex % (puzzles.Length / cv.Length));
        //pr.GetComponent<Rigidbody>().isKinematic = true; //���߰� �����

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
                if (preViewIndex / (puzzles.Length / cv.Length) == playerPreViewIndex)  //�ڽ��� �ǿ� �´� ������ ���� üũ ���� ȣ��
                {
                    cv[playerPreViewIndex].CatchToCheckBox(preViewIndex % (puzzles.Length / cv.Length));
                    //print(preViewIndex + "    " +  puzzles.Length/cv.Length + "    "  + preViewIndex%(puzzles.Length/cv.Length));
                }
                puzzles[i].GetComponent<Rigidbody>().isKinematic = true; //���߰� �����
                break;
            }
        }
    }
    Vector3 otherpos;

}
