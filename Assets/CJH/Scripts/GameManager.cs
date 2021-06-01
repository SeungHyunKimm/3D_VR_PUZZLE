using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static Rigidbody rigid;
    public static int preViewIndex;                   //������ ������ �ε���
    public static PuzzleManager pr;
    public static CanvasManager cv;
    public GameObject[] preView;       //���� ������ �迭
    Ray ray;
    RaycastHit hit;
    public float Speed;
    EffectSettings es;
    public GameObject shot;
    public GameObject pos;
    public GameObject setting;
    public GameObject control;
    Vector3 controlpos;
    // Start is called before the first frame update
    void Start()
    {
        GameObject canvas = GameObject.Find("Canvas");
        cv = canvas.GetComponent<CanvasManager>();
        es = shot.GetComponent<EffectSettings>();
    }

    // Update is called once per frame
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

    void ModeA_RightController()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (Input.GetMouseButtonDown(0))                    //���������� Ŭ�� �� ���߰� 
                Shot();

            if (pr == null) return;
                if (pr.state != PuzzleManager.PuzzleState.Catch)
                    preView[preViewIndex].SetActive(false);

                if (Input.GetMouseButtonDown(1))                //�޼�
                    preView[preViewIndex].SetActive(true);
                else if (Input.GetMouseButtonUp(1))                //���� �� ��ġ�� ��ü ������ �� ������ ��Ȱ��ȭ
                {
                    if (pr.state == PuzzleManager.PuzzleState.Catch)
                    {
                        pr.state = PuzzleManager.PuzzleState.Fusion;
                        Vector3 dir = preView[preViewIndex].transform.position - rigid.transform.position;
                        pr.Move(dir, Speed);
                    }
                    preView[preViewIndex].SetActive(false);
                    pr = null;
                }

                if (hit.transform.gameObject.name == "Canvas")  //������ ���� ��ġ ���� ĵ���� ����
                {
                    int x = (int)hit.point.x;
                    int y = (int)hit.point.y;
                    preView[preViewIndex].transform.position = new Vector2(x, y);
                }
        }
        if (pr == null) return;
        if (pr.state == PuzzleManager.PuzzleState.Revolution) return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            pr.state = PuzzleManager.PuzzleState.Control;
            controlpos = Camera.main.transform.position;
            control.transform.position = controlpos;
        }
        if (Input.GetKey(KeyCode.Space))               //���� ��ü�� �������
        {
            Vector3 dir = Camera.main.transform.position - controlpos;
            pr.controlpos = dir;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            rigid.isKinematic = true;
            pr.state = PuzzleManager.PuzzleState.Catch;
        }
    }

    void Shot()
    {
        shot.SetActive(true);
        shot.transform.position = Camera.main.transform.position;
        pos.transform.position = Camera.main.transform.position;
        es.Target = hit.transform.gameObject;
    }
    public void PuzzleChoiceChange(GameObject go)                   //Shot �߻� �� Catch ���·� ��ȯ
    {
        for (int i = 0; i < preView.Length; i++)
        {
            if (preView[i].name == go.name)     //������ �ε��� ���� �� Catch���·� ��ȯ 
            {
                if (preView[preViewIndex].name != go.transform.gameObject.name && pr != null && pr.state == PuzzleManager.PuzzleState.Catch)
                    pr.state = PuzzleManager.PuzzleState.Revolution;
                rigid = go.transform.GetComponent<Rigidbody>();
                pr = go.transform.GetComponent<PuzzleManager>();
                pr.state = PuzzleManager.PuzzleState.Catch;
                rigid.isKinematic = true;
                preViewIndex = i;
                cv.CatchToCheckBox(preViewIndex);
                break;
            }
        }
    }
}
