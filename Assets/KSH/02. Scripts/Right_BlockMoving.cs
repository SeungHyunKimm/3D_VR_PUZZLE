using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Right_BlockMoving : MonoBehaviour
{
    //��ġ
    Transform catchObj;
    //������ ��
    public float throwPower = 3;
    //����ĳ��Ʈ
    RaycastHit hit;
    //������ٵ�
    Rigidbody rigid;
    //���η�����
    LineRenderer lr;
    //���� ������ �迭
    public GameObject[] preView;
    //������ ������ �ε���
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
        //�չٴ�(������)���� ���̸� ���
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        //���̿� �ε��� �����
        if (Physics.Raycast(ray, out hit))
        {
            //�Ÿ��� ���ؼ� ������ �׸���.
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
        //������ B��ư�� ������ ������
        if (v > 0)
        {
            //���̸� �߻��Ͽ�

            Ray ray = new Ray(transform.position, transform.forward);
            int layer = 1 << LayerMask.NameToLayer("Puzzle");
            //�ε��� ���� ���̾ Puzzle�̸�
            if (Physics.SphereCast(ray, 0.7f, out hit, 100, layer))
            {
                for (int i = 0; i < preView.Length; i++)
                {
                    if (preView[i].name == hit.transform.gameObject.name) //������ �ε��� ���� �� Catch���·� ��ȯ 
                    {
                        rigid = hit.transform.GetComponent<Rigidbody>();
                        PuzzleManager.instance.state = PuzzleManager.PuzzleState.Catch; //Catch���� ��ȯ
                        rigid.isKinematic = true;                        //���� ȿ�� ���� ���·� ��ȯ
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

            print("�ε���");
            //catchObj�� �ε��� ���� ��ġ�� ���
            //catchObj = hit.transform;

            #region ť�갡 õõ�� ������ ���ƿ��Բ� ����(����)
            //�����հ� ť����� �Ÿ��� ������
            //Vector3 dir =  transform.position - hit.transform.position;
            //�ڿ������� �� ������ �� �� �ְԲ� ��������
            //rigid.AddForce(dir * 0.2f, ForceMode.Impulse);
            #endregion

            //�ε��� ���� transform�� �ڱ��ڽ��� �θ��� transform���� ���
            //hit.transform.SetParent(transform);
            //������ٵ� ��������
            // rigid = hit.transform.GetComponent<Rigidbody>();
            //������ٵ� ȿ���� ���ֱ� ���� isKinematic�� Ȱ��ȭ����
            //rigid.isKinematic = true;
            //�� �տ� ���� ȿ���� �α� ���� ���������� z���� ��ġ�� ��������
            //hit.transform.position = transform.position + new Vector3(0, 0, 1);

            //�ݸ��� ���·� �� �տ� ���Բ� ��������
        }
        else if (OVRInput.GetUp(OVRInput.Button.One, OVRInput.Controller.RTouch))
        {
            PuzzleManager.instance.state = PuzzleManager.PuzzleState.Revolution;
        }
        
        
        if (OVRInput.Get(OVRInput.Button.Two, OVRInput.Controller.RTouch))
        {
            Ray ray = new Ray(transform.position, transform.forward);
            int layer = 1 << LayerMask.NameToLayer("Canvas");
            //�ε��� ���� ���̾ Puzzle�̸�
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
            rigid.isKinematic = false;                     //����� �� true �����̹Ƿ� ��ȯ ������.
            Vector3 dir = preView[preViewIndex].transform.position - rigid.transform.position;

            rigid.AddForce(dir * 1, ForceMode.Impulse);
            preView[preViewIndex].SetActive(false);
        }

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
