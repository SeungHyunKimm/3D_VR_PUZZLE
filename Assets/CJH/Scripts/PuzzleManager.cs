using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    Transform center;         //공전 중심 위치
    Rigidbody rigid;          //퍼즐의 물리 컴포넌트
    BoxCollider box;          //퍼즐의 박스 콜라이더
    Vector3 dir;              //공전 방향 설정
    float dist;               //캔버스 중심축과의 거리
    float [] xyz;             //물체의 각도 값
    int[] rotspeed;           //회전 속도
    public int puzzleIndex;   //퍼즐 인덱스
    GameManager gm;           //프리뷰 인덱스 가져오기
    public bool rink;         //캔버스와의 연결 여부
 
    public enum PuzzleState
    {
        Revolution,           //공전 상태
        Catch,                //잡힌 상태
    }

    public PuzzleState state;
    // Start is called before the first frame update
    void Start()
    {
        xyz = new float[3];
        rotspeed = new int[3];
        rigid = GetComponent<Rigidbody>();
        box = GetComponent<BoxCollider>();
        StartCoroutine(ResetGravity());
        GameObject canvasGo = GameObject.Find("Center");
        center = canvasGo.GetComponent<Transform>();
        GameObject gmGo = GameObject.Find("GameManager");
        gm = gmGo.GetComponent<GameManager>();
    }

    // Update is called once per frame
    private void Update()
    {
        if(!rink)           //캔버스에 붙어있다면 다른 퍼즐을 잡아도 움직이지 않게 체크 
        //CheckPreView();
        switch (state)
        {
            case PuzzleState.Revolution:
                Revolution();
                break;
            case PuzzleState.Catch:
                Catch();
                break;
            default:
                break;
        }

    }

    IEnumerator ResetGravity()           //시작 0.5 초 후 무중력 상태로 전환
    {
        yield return new WaitForSeconds(0.5f);
        rigid.useGravity = false;
    }

   void Revolution()    // 캔버스 중심 공전하는 함수
    {
        dist = Vector3.Distance(transform.position, center.transform.position);
        transform.forward = center.transform.position - transform.position;

        if (dist <= 30)
        {
            dir = transform.forward + transform.right;
            rigid.AddForce(dir * 0.005f, ForceMode.Impulse);
        }
        else
        {
            dir = transform.position - center.transform.position;
            rigid.AddForce(-dir * 0.01f, ForceMode.Impulse);
        }
    }

    void Catch()                                     //판에 끼우기 전 x , y , z 각도를 0으로 맞춤 
    {
        xyz[0] = transform.rotation.x;
        xyz[1] = transform.rotation.y;
        xyz[2] = transform.rotation.z;
        for (int i = 0; i < xyz.Length; i++)
        {
            if (xyz[i] <= 0.008f && xyz[i] >= -0.008f)
                rotspeed[i] = 0;
            else
                rotspeed[i] = -3;
        }
        transform.Rotate(xyz[0] , xyz[1] , xyz[2]);
    }

    public void CheckPreView()
    {
        if (gm.preViewIndex != puzzleIndex)
        {
            state = PuzzleState.Revolution;
            rigid.isKinematic = false;
        }
    }
}
