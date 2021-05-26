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
        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch))
        {
            Ray ray = new Ray(transform.position, transform.forward);


            print("쏴짐");

            int layer = 1 << LayerMask.NameToLayer("Puzzle");
            if (Physics.SphereCast(ray, 0.7f, out hit, 100, layer))
            {
                print("부딪힘");
                catchObj = hit.transform;
                //오른손과 큐브와의 거리를 구하자
                //Vector3 dir =  transform.position - hit.transform.position;
                //자연스럽게 손 안으로 올 수 있게끔 연출하자
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
