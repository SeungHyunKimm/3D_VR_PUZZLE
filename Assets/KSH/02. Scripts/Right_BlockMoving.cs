using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Right_BlockMoving : MonoBehaviour
{
    Transform catchObj;
    public float throwPower = 3;

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
            RaycastHit hit;

            print("½÷Áü");

            int layer = 1 << LayerMask.NameToLayer("Puzzle");
            if (Physics.Raycast(ray, out hit, 100, layer))
            {
                print("ºÎµúÇø");
                hit.transform.position = transform.position;
            }
        }
    }

    void DropObj()
    {
        if (catchObj == null) return;
        if(OVRInput.GetUp(OVRInput.Button.Two, OVRInput.Controller.RTouch))
        {
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
