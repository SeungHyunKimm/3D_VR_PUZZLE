using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerControl : MonoBehaviourPun, IPunObservable
{

    public Rigidbody rigid;
    public int preViewIndex;                   //������ ������ �ε���
    public PuzzleManager pr;
    public CanvasManager[] cv;
    public GameObject[] preView;       //���� ������ �迭
    Ray ray;
    RaycastHit hit;
    public float fusionSpeed;                //���� �ӵ�
    EffectSettings es;
    public GameObject shot;
    public GameObject pos;
    //public GameObject setting;
    GameObject control;
    Vector3 controlpos;
    //Photon
    public GameObject my;    //��
    public GameObject other; //���
    public GameObject camera;
    float rotX, rotY;
    int z = 0;
    int playerPreViewIndex = 1;

    //PhotonView[] puzzles;
    GameObject[] puzzles;
    // Start is called before the first frame update
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
        cv = new CanvasManager[2];
        //���� ���� �� ����
        my.SetActive(photonView.IsMine);            //���� Ȱ�� ��Ȱ��
        other.SetActive(!photonView.IsMine);
        camera.SetActive(photonView.IsMine);

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

        GameObject pre = GameObject.Find("PreView");         //������ ���
        for (int i = 0; i < pre.transform.childCount; i++)
            preView[i] = pre.transform.GetChild(i).gameObject;

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


    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine)       //���� ��ġ ���� 
        {
            transform.position = Vector3.Lerp(transform.position, otherpos, 0.05f);
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
        Vector3 dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        dir.Normalize();

        transform.position += dir * 5 * Time.deltaTime;

        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");

        rotX += x;
        rotY += y;

        transform.eulerAngles = new Vector3(-rotY, rotX, 0);
    }
    void ModeA_RightController()
    {

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);


        if (Physics.Raycast(ray, out hit))
        {
            if (Input.GetMouseButtonDown(0))                    //���������� Ŭ�� �� ���߰� 
            {
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

                photonView.RPC("Catch", RpcTarget.All/*, hit.point, pv.ViewID*/);
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
    void Catch(/*Vector3 hitPoint, int viewId*/)
    {
        //for (int i = 0; i < puzzles.Length; i++)
        //{
        //    if (viewId == puzzles[i].ViewID)
        //    {
        for (int i = 0; i < preView.Length; i++)
        {
            print(puzzles[i].name + preView[i].name + "   " + preView.Length);
            if (preView[i].name == puzzles[i].name)     //������ �ε��� ���� �� Catch���·� ��ȯ 
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
    public void PuzzleChoiceChange(GameObject go)                   //Shot �߻� �� Catch ���·� ��ȯ
    {
        for (int i = 0; i < preView.Length; i++)
        {
            if (preView[i].name == go.name)     //������ �ε��� ���� �� Catch���·� ��ȯ 
            {
                if (preView[preViewIndex].name != go.transform.gameObject.name && pr != null)
                {
                    if (pr.state == PuzzleManager.PuzzleState.Catch || pr.state == PuzzleManager.PuzzleState.Control)
                        pr.state = PuzzleManager.PuzzleState.Revolution;
                }
                rigid = go.transform.GetComponent<Rigidbody>();
                pr = go.transform.GetComponent<PuzzleManager>();
                pr.state = PuzzleManager.PuzzleState.Catch;
                rigid.isKinematic = true;
                preViewIndex = i;
                //cv.CatchToCheckBox(preViewIndex);
                break;
            }
        }
    }

    Vector3 otherpos;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) //����
            stream.SendNext(transform.position);

        if (stream.IsReading) // �ٸ���
            otherpos = (Vector3)stream.ReceiveNext();
    }
}
