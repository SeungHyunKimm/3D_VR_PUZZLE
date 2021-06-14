using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class PuzzleManager : MonoBehaviourPun//, IPunObservable
{
    Vector3 center;         //공전 중심 위치
    Rigidbody rigid;          //퍼즐의 물리 컴포넌트
    BoxCollider box;          //퍼즐의 박스 콜라이더
    Vector3 dir;              //공전 방향 설정
    float dist;               //캔버스 중심축과의 거리
    //PlayerControl gm;           //프리뷰 인덱스 가져오기
    public Vector3 controlpos;//컨트롤 지정 위치 값
    float rvSpeed;            //공전 스피드
    float[] xyz;               //물체의 각도 값
    float pre_z;

    PC_AIPlayerControl AI;
    int width = 11, height = 11;
    public bool[,] puzzlePos;
    public enum PuzzleState
    {
        Revolution,           //공전 상태
        Catch,                //잡힌 상태
        Control,              //조정 상태
        Fusion,               //결합 상태
        Fixed,                 //고정 상태
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

    void Revolution()    // 캔버스 중심 공전하는 함수
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
    void Fusion()                                     //판에 끼우기 전 x , y , z 각도를 0으로 맞춤 
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
