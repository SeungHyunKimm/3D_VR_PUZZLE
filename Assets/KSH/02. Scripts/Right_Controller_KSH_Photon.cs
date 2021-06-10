using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Right_Controller_KSH_Photon : MonoBehaviour
{
    public static Rigidbody rigid;             //선택한 퍼즐의 RigidBody
    public static int preViewIndex;            //선택한 퍼즐의 프리 뷰
    public static PuzzleManager pr;            //선택한 퍼즐의 PuzzleManager
    public float throwPower = 3;
    Ray ray;
    RaycastHit hit;
    Transform catchObj;
    LineRenderer lr;
    //public GameObject[] preView; //프리 뷰 담는 배열
    //public float Speed;          //캔퍼스로 이동하는 속도

    //public GameObject shot;
    //public GameObject pos;
    EffectSettings es;


    GameObject SelectObj; // 선택한 놈 (승현)
    //클릭한 상태
    bool isClick;
    ////스타트캔버스
    //public GameObject StartCanvas;
    ////행성들
    //public GameObject Star;
    ////스타트 화면을 놓을 변수
    //public GameObject Scene_Start;


    void Start()
    {


        lr = GetComponent<LineRenderer>();
        //es = shot.GetComponent<EffectSettings>();


    }

    void Update()
    {
        switch (ButtonManager.instance.state)
        {
            case ButtonManager.ButtonState.Start:
            case ButtonManager.ButtonState.Select:
                //Start_Select_RightController();
                break;

            case ButtonManager.ButtonState.Mode_A:
                //ModeA_RightController();
                break;
            case ButtonManager.ButtonState.Mode_B:
                //ModeB_RightController();
                break;
            case ButtonManager.ButtonState.Mode_C:
                //ModeC_RightController();
                break;
            case ButtonManager.ButtonState.Mode_D:
                ModeD_RightController();
                break;
        }
        DrawGuideLine();
    }

    //void ModeA_RightController()                             //A 모드 오른손 컨트롤러
    //{

    //    ray = new Ray(transform.position, transform.forward);

    //    if (Physics.Raycast(ray, out hit, 100))
    //    {
    //        //PuzzleControl();
    //    }
    //}
    //void PuzzleControl()
    //{
    //    float v = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch); //B 버튼
    //    if (ButtonManager.instance.settingUI.activeSelf && v > 0)
    //    {
    //        if (hit.transform.gameObject.name.Contains("Resume"))
    //        {
    //            ButtonManager.instance.OnClickResume();
    //        }
    //        if (hit.transform.gameObject.name.Contains("Retry"))
    //        {
    //            ButtonManager.instance.OnClickRetry();
    //        }
    //        if (hit.transform.gameObject.name.Contains("SelectMenu"))
    //        {
    //            ButtonManager.instance.OnClickSelectMenu();
    //        }
    //        if (hit.transform.gameObject.name.Contains("ExitGame"))
    //        {
    //            ButtonManager.instance.OnClickExitGame();
    //        }
    //        return;
    //    } // 만약 메뉴창이 활성화되고 Trigger버튼을 누르고 있지 않으면
    //    if (v > 0)                        //클릭 또는 클릭 중
    //        Shot();

    //    if (pr != null)                       //선택된 블록이 없을 시 값 참조에 오류 방지
    //    {
    //        if (OVRInput.Get(OVRInput.Button.One, OVRInput.Controller.RTouch))  //A 버튼 클릭 중
    //        {
    //            Vector3 dir = transform.position - hit.transform.position;
    //            pr.Move(dir, 0.025f);
    //        }
    //        else if (OVRInput.GetUp(OVRInput.Button.One, OVRInput.Controller.RTouch)) //A 버튼 Up
    //        {
    //            pr.state = PuzzleManager.PuzzleState.Revolution;                      //공전 상태로 전환
    //            pr = null;                                                            //프리 뷰 생성 금지
    //        }

    //        if (OVRInput.Get(OVRInput.Button.Two, OVRInput.Controller.RTouch))
    //        {
    //            if (hit.transform.name == "Canvas")                  //캔퍼스일 때 프리 뷰 생성
    //            {
    //                preView[preViewIndex].SetActive(true);

    //                int x = (int)hit.point.x;
    //                int y = (int)hit.point.y;

    //                preView[preViewIndex].transform.position = new Vector2(x, y);
    //            }
    //            else
    //                preView[preViewIndex].SetActive(false);
    //        }
    //        else if (OVRInput.GetUp(OVRInput.Button.Two, OVRInput.Controller.RTouch))
    //        {
    //            if (pr.state == PuzzleManager.PuzzleState.Catch)
    //            {
    //                pr.state = PuzzleManager.PuzzleState.Fusion;
    //                Vector3 dir = preView[preViewIndex].transform.position - rigid.transform.position;
    //                pr.Move(dir, Speed);
    //            }
    //            preView[preViewIndex].SetActive(false);
    //            pr = null;
    //        }
    //    }
    //}

    void ModeB_RightController()            //B 모드 오른손 컨트롤러
    {

        CatchObj_B();

    }
    void ModeC_RightController()            //C 모드 오른손 컨트롤러
    {

        CatchObj();

    }
    void ModeD_RightController()            //D 모드 오른손 컨트롤러
    {

        CatchObj();

    }

    void Start_Select_RightController()         // Start & Select Mode 오른손 컨트롤러
    {


        //OnClickButtonUI();

    }

    //void Shot()
    //{
    //    shot.SetActive(true);
    //    shot.transform.position = transform.position;
    //    pos.transform.position = transform.position;
    //    es.Target = hit.transform.gameObject;
    //}

    //public void PuzzleChoiceChange(GameObject go)                   //Shot 발사 후 Catch 상태로 전환
    //{
    //    for (int i = 0; i < preView.Length; i++)
    //    {
    //        if (preView[i].name == go.name)     //프리뷰 인덱스 저장 및 Catch상태로 변환 
    //        {
    //            if (preView[preViewIndex].name != go.transform.gameObject.name && pr != null && pr.state == PuzzleManager.PuzzleState.Catch)
    //                pr.state = PuzzleManager.PuzzleState.Revolution;
    //            rigid = go.transform.GetComponent<Rigidbody>();
    //            pr = go.transform.GetComponent<PuzzleManager>();
    //            pr.state = PuzzleManager.PuzzleState.Catch;
    //            rigid.isKinematic = true;
    //            preViewIndex = i;
    //            break;
    //        }
    //    }
    //}
    void DrawGuideLine()
    {
        ray = new Ray(transform.position, transform.forward);
        //레이와 부딪힌 놈까지
        if (Physics.Raycast(ray, out hit))
        {
            //거리를 구해서 라인을 그린다.
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, hit.point);
            //RotateButton(hit.transform.gameObject);
        }
        else
        {
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, transform.position + transform.forward * 1);
            //RotateButton(null);

        }
    }

    //void OnClickButtonUI() //승현 StartScene에서 사용할 함수 - 버튼 누를 시 다음 씬 전환
    //{
    //    ray = new Ray(transform.position, transform.forward);
    //    float v = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch); //B 버튼

    //    if (v > 0 && Physics.Raycast(ray, out hit))
    //    {
    //        if (hit.transform.gameObject.name.Contains("StartButton"))
    //        {
    //            ButtonManager.instance.OnClickStart();
    //        }
    //        if (hit.transform.gameObject.name.Contains("A_Mode"))
    //        {
    //            ButtonManager.instance.OnClickMode_A();
    //        }
    //        if (hit.transform.gameObject.name.Contains("B_Mode"))
    //        {
    //            ButtonManager.instance.OnClickMode_B();
    //        }
    //        if (hit.transform.gameObject.name.Contains("C_Mode"))
    //        {
    //            ButtonManager.instance.OnClickMode_C();
    //        }
    //        if (hit.transform.gameObject.name.Contains("D_Mode"))
    //        {
    //            ButtonManager.instance.OnClickMode_D();
    //        }
    //    }

    //    if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch))
    //    {
    //        //print(hit.transform.gameObject.name);
    //        //Saturn 행성을 클릭해서 StartScene을 활성화 하고 싶다.
    //        if (Physics.Raycast(ray, out hit))
    //        {
    //            if (hit.transform.gameObject.name.Contains("Saturn"))
    //            {
    //                //행성들 다 사라지게 한다.
    //                Star.SetActive(false);
    //                StartCanvas.SetActive(true);
    //                StartCanvas.transform.position = Scene_Start.transform.position;
    //            }
    //        }
    //    }
    //}


    GameObject focusBtn;
    void RotateButton(GameObject btn)
    {
        if (btn == null || btn.name.Contains("Mode") == false)
        {
            StopRotateButton();
        }

        else if (btn.name.Contains("Mode"))
        {
            if (focusBtn != btn)
            {
                StopRotateButton();
                iTween.RotateBy(btn.transform.GetChild(0).gameObject, iTween.Hash(
                    "x", .25,
                    "easytype", iTween.EaseType.easeInBounce,
                    "looptype", iTween.LoopType.pingPong,
                    "name", btn.name));
            }
            focusBtn = btn;
        }
    }


    void StopRotateButton()
    {
        if (focusBtn == null) return;

        iTween.StopByName(focusBtn.name);
        focusBtn.transform.GetChild(0).transform.localEulerAngles = Vector3.zero;
        focusBtn = null;
    }




    void CatchObj()
    {
        isClick = true;
        SelectObj = null;
        //레이를 발사하여
        ray = new Ray(transform.position, transform.forward);

        float v = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch);
        //오른쪽 B버튼을 누르고 있으면
        if(Physics.Raycast(ray, out hit))
        {
            if (ButtonManager.instance.settingUI.activeSelf && v > 0)
            {
                if (hit.transform.gameObject.name.Contains("Resume"))
                {
                    ButtonManager.instance.OnClickResume();
                }
                if (hit.transform.gameObject.name.Contains("Retry"))
                {
                    ButtonManager.instance.OnClickRetry();
                }
                if (hit.transform.gameObject.name.Contains("SelectMenu"))
                {
                    ButtonManager.instance.OnClickSelectMenu();
                }
                if (hit.transform.gameObject.name.Contains("ExitGame"))
                {
                    ButtonManager.instance.OnClickExitGame();
                }
                return;
            }
        }
        

        if (v > 0)
        {
            int layer = 1 << LayerMask.NameToLayer("Puzzle");
            //부딪힌 놈의 레이어가 Puzzle이면
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
            int layer = 1 << LayerMask.NameToLayer("Puzzle");
            if (Physics.Raycast(ray, out hit, 100, layer))
            {
                SelectObj.transform.position = new Vector3(hit.point.x, hit.point.y, -0.1f);
            }
        }


    }
    public bool isClear;
    void CatchObj_B()
    {
        ray = new Ray(transform.position, transform.forward);
        int layer1 = 1 << LayerMask.NameToLayer("Puzzle");

        if (Physics.Raycast(ray, out hit, 100, layer1) && isClear == false)
        {
            if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch))
            {
                //B번 물체
                print("rotate complete");
                hit.transform.Rotate(0, 0, 90);
            }
        }

        float v = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch);

        if (ButtonManager.instance.settingUI.activeSelf && v > 0 && Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.name.Contains("Resume"))
            {
                ButtonManager.instance.OnClickResume();
            }
            if (hit.transform.gameObject.name.Contains("Retry"))
            {
                ButtonManager.instance.OnClickRetry();
            }
            if (hit.transform.gameObject.name.Contains("SelectMenu"))
            {
                ButtonManager.instance.OnClickSelectMenu();
            }
            if (hit.transform.gameObject.name.Contains("ExitGame"))
            {
                ButtonManager.instance.OnClickExitGame();
            }
            return;
        }
        //선택한 놈의 레이어가 UI이면 트리거 선택이 되게끔 하자.

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
