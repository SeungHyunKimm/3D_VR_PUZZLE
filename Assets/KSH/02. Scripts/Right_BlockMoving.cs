using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Right_BlockMoving : MonoBehaviour
{
    //위치
    Transform catchObj;
    //던지는 힘
    public float throwPower = 3;
    //레이캐스트
    RaycastHit hit;
    //리지드바디
    Rigidbody rigid;
    //라인렌더러
    LineRenderer lr;
    //퍼즐 프리뷰 배열
    public GameObject[] preView;
    //선택한 퍼즐의 인덱스
    int preViewIndex;

    void Start()
    {



        lr = GetComponent<LineRenderer>();



    }

    void Update()
    {
        DrawGuideLine();
        CatchObj();
        DropObj();
    }

    void DrawGuideLine()
    {
        //손바닥(오른손)에서 레이를 쏜다
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        //레이와 부딪힌 놈까지
        if (Physics.Raycast(ray, out hit))
        {
            //거리를 구해서 라인을 그린다.
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, hit.point);
        }
        else
        {
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, transform.position + transform.forward * 1);
        }
    }

    void CatchObj()
    {

        float v = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch);
        //오른쪽 B버튼을 누르고 있으면
        if (v > 0)
        {
            //레이를 발사하여

            Ray ray = new Ray(transform.position, transform.forward);
            int layer = 1 << LayerMask.NameToLayer("Puzzle");
            //부딪힌 놈의 레이어가 Puzzle이면
            if (Physics.SphereCast(ray, 0.7f, out hit, 100, layer))
            {
                for (int i = 0; i < preView.Length; i++)
                {
                    if (preView[i].name == hit.transform.gameObject.name) //프리뷰 인덱스 저장 및 Catch상태로 변환 
                    {
                        rigid = hit.transform.GetComponent<Rigidbody>();
                        PuzzleManager.instance.state = PuzzleManager.PuzzleState.Catch; //Catch상태 전환
                        rigid.isKinematic = true;                        //물리 효과 적용 상태로 전환
                        preViewIndex = i;
                        break;
                    }
                }
            }
        }


        if (OVRInput.Get(OVRInput.Button.One, OVRInput.Controller.RTouch))
        {
            rigid.isKinematic = false;
            Vector3 dir = transform.position - hit.transform.position;
            rigid.AddForce(dir * 0.05f, ForceMode.Impulse);

            print("부딪힘");
            //catchObj에 부딪힌 놈의 위치를 담고
            //catchObj = hit.transform;

            #region 큐브가 천천히 손으로 날아오게끔 하자(실패)
            //오른손과 큐브와의 거리를 구하자
            //Vector3 dir =  transform.position - hit.transform.position;
            //자연스럽게 손 안으로 올 수 있게끔 연출하자
            //rigid.AddForce(dir * 0.2f, ForceMode.Impulse);
            #endregion

            //부딪힌 놈의 transform을 자기자신의 부모의 transform으로 담고
            //hit.transform.SetParent(transform);
            //리지드바디를 가져오자
            // rigid = hit.transform.GetComponent<Rigidbody>();
            //리지드바디 효과를 없애기 위해 isKinematic을 활성화하자
            //rigid.isKinematic = true;
            //손 앞에 놓는 효과를 두기 위해 인위적으로 z축의 위치를 조정하자
            //hit.transform.position = transform.position + new Vector3(0, 0, 1);

            //콜리전 형태로 손 앞에 오게끔 설계하자
        }
        else if (OVRInput.GetUp(OVRInput.Button.One, OVRInput.Controller.RTouch))
        {
            PuzzleManager.instance.state = PuzzleManager.PuzzleState.Revolution;
        }
        
        
        if (OVRInput.Get(OVRInput.Button.Two, OVRInput.Controller.RTouch))
        {
            Ray ray = new Ray(transform.position, transform.forward);
            int layer = 1 << LayerMask.NameToLayer("Canvas");
            //부딪힌 놈의 레이어가 Puzzle이면
            if (Physics.SphereCast(ray, 0.7f, out hit, 100, layer))
            {
                preView[preViewIndex].SetActive(true);

                int x = (int)hit.point.x;
                int y = (int)hit.point.y;

                preView[preViewIndex].transform.position = new Vector2(x, y);
            }
        }
        else if (OVRInput.GetUp(OVRInput.Button.Two, OVRInput.Controller.RTouch))
        {
            PuzzleManager.instance.state = PuzzleManager.PuzzleState.Catch;
            rigid.isKinematic = false;                     //잡았을 때 true 상태이므로 전환 시켜줌.
            Vector3 dir = preView[preViewIndex].transform.position - rigid.transform.position;

            rigid.AddForce(dir * 1, ForceMode.Impulse);
            preView[preViewIndex].SetActive(false);
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
