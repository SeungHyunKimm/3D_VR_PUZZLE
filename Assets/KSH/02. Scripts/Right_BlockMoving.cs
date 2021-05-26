using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Right_BlockMoving : MonoBehaviour
{
    Transform catchObj;
    public float throwPower = 3;
    RaycastHit hit;
    Rigidbody rigid;
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
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
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
        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch))
        {
            Ray ray = new Ray(transform.position, transform.forward);


            print("����");

            int layer = 1 << LayerMask.NameToLayer("Puzzle");
            if (Physics.SphereCast(ray, 0.7f, out hit, 100, layer))
            {
                print("�ε���");
                catchObj = hit.transform;
                //�����հ� ť����� �Ÿ��� ������
                //Vector3 dir =  transform.position - hit.transform.position;
                //�ڿ������� �� ������ �� �� �ְԲ� ��������
                //rigid.AddForce(dir * 0.2f, ForceMode.Impulse);
                
                hit.transform.SetParent(transform);
                rigid = hit.transform.GetComponent<Rigidbody>();
                rigid.isKinematic = true;
                
                hit.transform.position = transform.position + new Vector3(0,0,1);

                
            }
        }
    }

    void DropObj()
    {
        if (catchObj == null) return;

        if (OVRInput.GetUp(OVRInput.Button.Two, OVRInput.Controller.RTouch))
        {
            //Ray ray = new Ray(hit.transform.position, transform.position);
            //RaycastHit hit;
            //if(Physics.SphereCast())
            catchObj.SetParent(null);
            
            catchObj.GetComponent<Rigidbody>().isKinematic = false;

            ThrowObj();

            catchObj = null;

        }
    }

    void ThrowObj()
    {

        Rigidbody rb = catchObj.GetComponent<Rigidbody>();
        rb.velocity = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch) * throwPower;

    }

}
