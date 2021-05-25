using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Right_BlockMoving : MonoBehaviour
{
    void Start()
    {




    }

    void Update()
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
}
