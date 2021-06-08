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
    PlayerControl gm;           //프리뷰 인덱스 가져오기
    public Vector3 controlpos;//컨트롤 지정 위치 값
    float rvSpeed;            //공전 스피드
    float[] xyz;               //물체의 각도 값
    float pre_z;
    Quaternion value;
    float x, y, z;

    public Vector3 puzzlerot;

    public enum PuzzleState
    {
        Revolution,           //공전 상태
        Catch,                //잡힌 상태
        Control,              //조정 상태
        Fusion,               //결합 상태
        Fixed                 //고정 상태
    }

    enum PuzzleRot
    {
        Set,                   //월드 각도 세팅
        Change                 //프리 뷰의 각도로 변경
    }

    public PuzzleState state;
    PuzzleRot state2;
    // Start is called before the first frame update
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        box = GetComponent<BoxCollider>();
    }

    void Start()
    {
        xyz = new float[3];
        StartCoroutine(ResetGravity());
        center = new Vector3(5, 5, 0.5f);
        GameObject gmGo = GameObject.Find("GameManager");
        gm = gmGo.GetComponent<PlayerControl>();
        rigid.isKinematic = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PuzzleOperation();
        }
    }
    void PuzzleOperation()
    {
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
    }

    IEnumerator ResetGravity()           //시작 0.5 초 후 무중력 상태로 전환
    {
        yield return new WaitForSeconds(1);
        rigid.useGravity = false;
    }

    void Revolution()    // 캔버스 중심 공전하는 함수
    {
        rigid.isKinematic = false;
        dist = Vector3.Distance(transform.position, center);

        //transform.forward = center - transform.position;
        if (dist <= 30)
        {
            dir = transform.forward + transform.right + transform.up;
            rvSpeed = 2;
        }
        else
        {
            dir = center - transform.position;
            rvSpeed = 0.5f;
        }

        rigid.AddForce(dir * rvSpeed * Time.deltaTime, ForceMode.Impulse);
    }
    int rotz;
    void Fusion()                                     //판에 끼우기 전 x , y , z 각도를 0으로 맞춤 
    {
        int mix = (int)transform.eulerAngles.x + (int)transform.eulerAngles.y + (int)transform.eulerAngles.z;
        if (mix == 0)
            state2 = PuzzleRot.Change;
        if (state2 == PuzzleRot.Change && (int)transform.eulerAngles.z == pre_z) return;
        switch (state2)
        {
            case PuzzleRot.Set:
                xyz[0] = transform.rotation.x;
                xyz[1] = transform.rotation.y;
                xyz[2] = transform.rotation.z;
                transform.Rotate(xyz[0] * 3, xyz[1] * 3, xyz[2] * 3);
                break;
            case PuzzleRot.Change:
                transform.Rotate(0, 0, rotz);
                break;
        }
    }
    public void SetPreViewXYZ(GameObject prRot)
    {
        pre_z = prRot.transform.eulerAngles.z;
    }
    int xz = 0;
    void Catch()
    {
        transform.Rotate(1, 0, 0);
        state2 = PuzzleRot.Set;
        rotz = 1;
    }

    public void Move(Vector3 dir, float Speed)
    {
        rigid.isKinematic = false;
        rigid.AddForce(dir * Speed * Time.deltaTime, ForceMode.Impulse);
    }


    void Control()
    {
        Move(controlpos, 3);
        Catch();
    }

    private void OnCollisionEnter(Collision collision)
    {
        PuzzleManager pr = collision.transform.GetComponent<PuzzleManager>();
        if (collision.gameObject.layer == LayerMask.NameToLayer("Puzzle") && state != PuzzleState.Fixed)
        {
            if (pr.state != PuzzleState.Fixed)
                state = PuzzleState.Revolution;
        }
    }
}
