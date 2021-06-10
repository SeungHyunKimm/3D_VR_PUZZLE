using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Right_Controller_KSH_Photon : MonoBehaviour
{
    public static Rigidbody rigid;             //������ ������ RigidBody
    public static int preViewIndex;            //������ ������ ���� ��
    public static PuzzleManager pr;            //������ ������ PuzzleManager
    public float throwPower = 3;
    Ray ray;
    RaycastHit hit;
    Transform catchObj;
    LineRenderer lr;
    //public GameObject[] preView; //���� �� ��� �迭
    //public float Speed;          //ĵ�۽��� �̵��ϴ� �ӵ�

    //public GameObject shot;
    //public GameObject pos;
    EffectSettings es;


    GameObject SelectObj; // ������ �� (����)
    //Ŭ���� ����
    bool isClick;
    ////��ŸƮĵ����
    //public GameObject StartCanvas;
    ////�༺��
    //public GameObject Star;
    ////��ŸƮ ȭ���� ���� ����
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

    //void ModeA_RightController()                             //A ��� ������ ��Ʈ�ѷ�
    //{

    //    ray = new Ray(transform.position, transform.forward);

    //    if (Physics.Raycast(ray, out hit, 100))
    //    {
    //        //PuzzleControl();
    //    }
    //}
    //void PuzzleControl()
    //{
    //    float v = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch); //B ��ư
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
    //    } // ���� �޴�â�� Ȱ��ȭ�ǰ� Trigger��ư�� ������ ���� ������
    //    if (v > 0)                        //Ŭ�� �Ǵ� Ŭ�� ��
    //        Shot();

    //    if (pr != null)                       //���õ� ����� ���� �� �� ������ ���� ����
    //    {
    //        if (OVRInput.Get(OVRInput.Button.One, OVRInput.Controller.RTouch))  //A ��ư Ŭ�� ��
    //        {
    //            Vector3 dir = transform.position - hit.transform.position;
    //            pr.Move(dir, 0.025f);
    //        }
    //        else if (OVRInput.GetUp(OVRInput.Button.One, OVRInput.Controller.RTouch)) //A ��ư Up
    //        {
    //            pr.state = PuzzleManager.PuzzleState.Revolution;                      //���� ���·� ��ȯ
    //            pr = null;                                                            //���� �� ���� ����
    //        }

    //        if (OVRInput.Get(OVRInput.Button.Two, OVRInput.Controller.RTouch))
    //        {
    //            if (hit.transform.name == "Canvas")                  //ĵ�۽��� �� ���� �� ����
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

    void ModeB_RightController()            //B ��� ������ ��Ʈ�ѷ�
    {

        CatchObj_B();

    }
    void ModeC_RightController()            //C ��� ������ ��Ʈ�ѷ�
    {

        CatchObj();

    }
    void ModeD_RightController()            //D ��� ������ ��Ʈ�ѷ�
    {

        CatchObj();

    }

    void Start_Select_RightController()         // Start & Select Mode ������ ��Ʈ�ѷ�
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

    //public void PuzzleChoiceChange(GameObject go)                   //Shot �߻� �� Catch ���·� ��ȯ
    //{
    //    for (int i = 0; i < preView.Length; i++)
    //    {
    //        if (preView[i].name == go.name)     //������ �ε��� ���� �� Catch���·� ��ȯ 
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
        //���̿� �ε��� �����
        if (Physics.Raycast(ray, out hit))
        {
            //�Ÿ��� ���ؼ� ������ �׸���.
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

    //void OnClickButtonUI() //���� StartScene���� ����� �Լ� - ��ư ���� �� ���� �� ��ȯ
    //{
    //    ray = new Ray(transform.position, transform.forward);
    //    float v = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch); //B ��ư

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
    //        //Saturn �༺�� Ŭ���ؼ� StartScene�� Ȱ��ȭ �ϰ� �ʹ�.
    //        if (Physics.Raycast(ray, out hit))
    //        {
    //            if (hit.transform.gameObject.name.Contains("Saturn"))
    //            {
    //                //�༺�� �� ������� �Ѵ�.
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
        //���̸� �߻��Ͽ�
        ray = new Ray(transform.position, transform.forward);

        float v = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch);
        //������ B��ư�� ������ ������
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
            //�ε��� ���� ���̾ Puzzle�̸�
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
                //B�� ��ü
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
        //������ ���� ���̾ UI�̸� Ʈ���� ������ �ǰԲ� ����.

    }

    void DropObj()
    {
        //���� catchObj�� ���� ������ ������ �� �Լ��� ����������
        if (catchObj == null) return;

        //������ B��ư�� ����
        if (OVRInput.GetUp(OVRInput.Button.Two, OVRInput.Controller.RTouch))
        {
            //�������� �ƹ��͵� ���ٸ�
            catchObj.SetParent(null);
            //���� ���� ������ٵ� Ȱ��ȭ�ϱ� ���� isKinematic�� ��Ȱ��ȭ�ϰ�
            catchObj.GetComponent<Rigidbody>().isKinematic = false;
            //������ �Լ� ����
            ThrowObj();
            //������ ������ �� �ʱ�ȭ
            catchObj = null;
        }
    }

    void ThrowObj()
    {
        //������ٵ� ������Ʈ ��������
        Rigidbody rb = catchObj.GetComponent<Rigidbody>();
        //���̽�ƽ ������ + ������ �� = ����� ������ �����Բ� ȿ���� ������
        rb.velocity = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch) * throwPower;
    }
}
