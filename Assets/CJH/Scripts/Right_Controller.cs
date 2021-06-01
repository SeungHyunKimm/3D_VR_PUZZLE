using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Right_Controller : MonoBehaviour
{
    public static Rigidbody rigid;             //선택한 퍼즐의 RigidBody
    public static int preViewIndex;            //선택한 퍼즐의 프리 뷰
    public static PuzzleManager pr;            //선택한 퍼즐의 PuzzleManager
    public float throwPower = 3;
    Ray ray;
    RaycastHit hit;
    Transform catchObj;
    LineRenderer lr;
    public GameObject[] preView; //프리 뷰 담는 배열
    public float Speed;          //캔퍼스로 이동하는 속도

    public GameObject shot;
    public GameObject pos;
    EffectSettings es;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        es = shot.GetComponent<EffectSettings>();
    }

    void Update()
    {
        switch (ButtonManager.instance.state)
        {
            case ButtonManager.ButtonState.Mode_A:
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

    void ModeA_RightController()                             //A 모드 오른 손 컨트롤러
    {
        ray = new Ray(transform.position, transform.forward);

        if (Physics.SphereCast(ray, 0.7f, out hit, 100))
        {
            PuzzleControl();
        }
    }
    void PuzzleControl()
    {
        float v = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch); //B 버튼
        if (v > 0)                        //클릭 또는 클릭 중
        {
            Shot();
        }
        if (pr != null)                       //선택된 블록이 없을 시 값 참조에 오류 방지
        {
            if (OVRInput.Get(OVRInput.Button.One, OVRInput.Controller.RTouch))  //A 버튼 클릭 중
            {
                Vector3 dir = transform.position - hit.transform.position;
                pr.Move(dir, 0.05f);
            }
            else if (OVRInput.GetUp(OVRInput.Button.One, OVRInput.Controller.RTouch)) //A 버튼 Up
            {
                pr.state = PuzzleManager.PuzzleState.Revolution;                      //공전 상태로 전환
                pr = null;                                                            //프리 뷰 생성 금지
            }

            if (OVRInput.Get(OVRInput.Button.Two, OVRInput.Controller.RTouch))
            {
                if (hit.transform.name == "Canvas")                  //캔퍼스일 때 프리 뷰 생성
                {
                    preView[preViewIndex].SetActive(true);

                    int x = (int)hit.point.x;
                    int y = (int)hit.point.y;

                    preView[preViewIndex].transform.position = new Vector2(x, y);
                }
                else
                    preView[preViewIndex].SetActive(false);
            }
            else if (OVRInput.GetUp(OVRInput.Button.Two, OVRInput.Controller.RTouch))
            {
                if (pr.state == PuzzleManager.PuzzleState.Catch)
                {
                    pr.state = PuzzleManager.PuzzleState.Fusion;
                    Vector3 dir = preView[preViewIndex].transform.position - rigid.transform.position;
                    pr.Move(dir, Speed);
                }
                preView[preViewIndex].SetActive(false);
                pr = null;                         //프리 뷰 생성 금지
            }

        }
    }


    void Shot()
    {
        shot.SetActive(true);
        shot.transform.position = transform.position;
        pos.transform.position = transform.position;
        es.Target = hit.transform.gameObject;
    }
    public void PuzzleChoiceChange(GameObject go)                   //Shot 발사 후 Catch 상태로 전환
    {
        for (int i = 0; i < preView.Length; i++)
        {
            if (preView[i].name == go.name)     //프리뷰 인덱스 저장 및 Catch상태로 변환 
            {
                if (preView[preViewIndex].name != go.transform.gameObject.name && pr != null && pr.state == PuzzleManager.PuzzleState.Catch)
                    pr.state = PuzzleManager.PuzzleState.Revolution;
                rigid = go.transform.GetComponent<Rigidbody>();
                pr = go.transform.GetComponent<PuzzleManager>();
                pr.state = PuzzleManager.PuzzleState.Catch;
                rigid.isKinematic = true;
                preViewIndex = i;
                break;
            }
        }
    }
    void DrawGuideLine()
    {
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
