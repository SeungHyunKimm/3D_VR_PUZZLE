using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class PuzzleManager : MonoBehaviourPun//, IPunObservable
{
    Vector3 center;         //���� �߽� ��ġ
    Rigidbody rigid;          //������ ���� ������Ʈ
    BoxCollider box;          //������ �ڽ� �ݶ��̴�
    Vector3 dir;              //���� ���� ����
    float dist;               //ĵ���� �߽������ �Ÿ�
    //PlayerControl gm;           //������ �ε��� ��������
    public Vector3 controlpos;//��Ʈ�� ���� ��ġ ��
    float rvSpeed;            //���� ���ǵ�
    float[] xyz;               //��ü�� ���� ��
    float pre_z;

    PC_AIPlayerControl AI;
    int width = 11, height = 11;
    public bool[,] puzzlePos;
    public enum PuzzleState
    {
        Revolution,           //���� ����
        Catch,                //���� ����
        Control,              //���� ����
        Fusion,               //���� ����
        Fixed,                 //���� ����
        Clear
    }

    public PuzzleState state;

    // Start is called before the first frame update
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        box = GetComponent<BoxCollider>();
    }

    void Start()
    {
        AI = GameObject.Find("AIPlayer").GetComponent<PC_AIPlayerControl>();
        xyz = new float[3];
        center = new Vector3(5, 5, 0.5f);
        puzzlePos = new bool[width, height];
    }

    //Update is called once per frame
    private void FixedUpdate()
    {
        PuzzleOperation();
    }

    public void PuzzleOperation()
    {
        //if (PhotonNetwork.IsMasterClient)
        //{
        switch (state)
        {
            case PuzzleState.Revolution:
                Revolution();
                break;
            case PuzzleState.Control:
                Control();
                break;
            case PuzzleState.Fusion:
                Fusion();
                break;
            case PuzzleState.Catch:
                Catch();
                break;
        }
        //}
    }

    void Revolution()    // ĵ���� �߽� �����ϴ� �Լ�
    {
        rigid.isKinematic = false;
        dist = Vector3.Distance(transform.position, center);

        //transform.forward = center - transform.position;
        //transform.forward = Vector3.forward + Vector3.right;
        //dir = Vector3.forward + Vector3.right;
        if (dist <= 40)
        {
            dir = transform.forward + transform.right;
            rvSpeed = 1.5f;
        }
        else
        {
            dir = center - transform.position;
            rvSpeed = 1;
        }

        rigid.AddForce(dir * rvSpeed * Time.deltaTime, ForceMode.Impulse);
    }
    int rotz;
    void Fusion()                                     //�ǿ� ����� �� x , y , z ������ 0���� ���� 
    {
        if ((int)transform.eulerAngles.z == pre_z) return;
        transform.Rotate(0, 0, 1);
    }
    public void SetPreViewXYZ(GameObject prRot)
    {
        pre_z = prRot.transform.eulerAngles.z;
    }
    int xz = 0;
    void Catch()
    {
        //int mix = (int)transform.eulerAngles.x + (int)transform.eulerAngles.y + (int)transform.eulerAngles.z;
        //if (mix == 0)
        xyz[0] = transform.rotation.x;
        xyz[1] = transform.rotation.y;
        xyz[2] = transform.rotation.z;
        transform.Rotate(xyz[0] * 5, xyz[1] * 5, xyz[2] * 5);
    }

    public void Move(Vector3 dir, float Speed)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            rigid.isKinematic = false;
            rigid.AddForce(dir * Speed * Time.deltaTime, ForceMode.Impulse);
        }
    }

    void Control()
    {
        Move(controlpos, 3);
        Catch();
    }

    public void Fixed()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            int x = Mathf.RoundToInt(transform.GetChild(i).position.x);
            int y = Mathf.RoundToInt(transform.GetChild(i).position.y);
            if (x >= 0 && x < width && y >= 0 && y < height)
                puzzlePos[x, y] = true;
        }
        rigid.isKinematic = true;
    }

    public void Clear()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            int x = Mathf.RoundToInt(transform.GetChild(i).position.x);
            int y = Mathf.RoundToInt(transform.GetChild(i).position.y);
            if (x >= 0 && x < width && y >= 0 && y < height)
                AI.quad[x, y] = false;
        }
        rigid.isKinematic = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state == PuzzleState.Clear || state == PuzzleState.Fixed) return;
        if (collision.gameObject.layer != LayerMask.NameToLayer("Puzzle")) return;
        PuzzleManager pr = collision.transform.GetComponent<PuzzleManager>();
        if (pr.state != PuzzleManager.PuzzleState.Clear)
            state = PuzzleState.Revolution;
    }
}
