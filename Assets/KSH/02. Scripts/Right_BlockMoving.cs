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
        if(Physics.Raycast(ray, out hit))
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
        //������ B��ư�� ������ ������
        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch))
        {
            //���̸� �߻��Ͽ�
            Ray ray = new Ray(transform.position, transform.forward);


            print("����");
            //�ε��� ���� ���̾ Puzzle�̸�
            int layer = 1 << LayerMask.NameToLayer("Puzzle");
            if (Physics.SphereCast(ray, 0.7f, out hit, 100, layer))
            {
                print("�ε���");
                //catchObj�� �ε��� ���� ��ġ�� ���
                catchObj = hit.transform;

                #region ť�갡 õõ�� ������ ���ƿ��Բ� ����(����)
                //�����հ� ť����� �Ÿ��� ������
                //Vector3 dir =  transform.position - hit.transform.position;
                //�ڿ������� �� ������ �� �� �ְԲ� ��������
                //rigid.AddForce(dir * 0.2f, ForceMode.Impulse);
                #endregion

                //�ε��� ���� transform�� �ڱ��ڽ��� �θ��� transform���� ���
                hit.transform.SetParent(transform);
                //������ٵ� ��������
                rigid = hit.transform.GetComponent<Rigidbody>();
                //������ٵ� ȿ���� ���ֱ� ���� isKinematic�� Ȱ��ȭ����
                rigid.isKinematic = true;
                //�� �տ� ���� ȿ���� �α� ���� ���������� z���� ��ġ�� ��������
                hit.transform.position = transform.position + new Vector3(0,0,1);

                
            }
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
