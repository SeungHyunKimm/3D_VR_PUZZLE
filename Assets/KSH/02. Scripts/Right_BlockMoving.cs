using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Right_BlockMoving : MonoBehaviour
{
    Transform catchObj;
    public float throwPower = 3;
    RaycastHit hit;
    Rigidbody rigid;

    void Start()
    {




    }

    void Update()
    {
        CatchObj();
        DropObj();
    }

    void CatchObj()
    {
        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
        {
            Ray ray = new Ray(transform.position, transform.forward);


            print("����");

            int layer = 1 << LayerMask.NameToLayer("Puzzle");
            if (Physics.SphereCast(ray, 1f, out hit, 100, layer))
            {
                print("�ε���");
                catchObj = hit.transform;
                hit.transform.SetParent(transform);
                //�����հ� ť����� �Ÿ��� ������
                Vector3 dir = transform.position - hit.transform.position;
                //�ڿ������� �� ������ �� �� �ְԲ� ��������
                //rigid.AddForce(dir * 0.01f);
                //hit.transform.position += dir * 0.01f * Time.deltaTime;
                //hit.transform.position = rigid.AddForce(dir * 0.01f);
                hit.transform.position = transform.position + new Vector3(0,0,1);
                
            }
        }

    }

    void DropObj()
    {
        if (catchObj == null) return;

        if (OVRInput.GetUp(OVRInput.Button.Two, OVRInput.Controller.RTouch))
        {

            catchObj.SetParent(null);
            
            //catchObj.GetComponent<Rigidbody>().isKinematic = false;

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
