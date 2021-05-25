using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Right_BlockMoving : MonoBehaviour
{
    Transform catchObj;
    public float throwPower = 3;
    RaycastHit hit;

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


            print("½÷Áü");

            int layer = 1 << LayerMask.NameToLayer("Puzzle");
            if (Physics.SphereCast(ray, 1f, out hit, 100, layer))
            {
                print("ºÎµúÇø");
                hit.transform.SetParent(transform);
                hit.transform.position = transform.position + new Vector3(0,0,1);
                //print(transform.position);
                //print(Camera.main.transform.position);
            }
        }
    }

    void DropObj()
    {
        if (catchObj == null) return;
        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch))
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
