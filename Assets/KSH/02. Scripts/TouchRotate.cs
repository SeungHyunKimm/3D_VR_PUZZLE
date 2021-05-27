using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchRotate : MonoBehaviour
{
    // void Update()
    //{
    //    if(Input.GetMouseButtonDown(0) || OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
    //    {
    //        if(!GameControl.youWin)
    //        transform.Rotate(0, 0, 90f);
    //    }

    //}
    private void OnMouseDown()
    {
        
        if (!GameControl.youWin) transform.Rotate(0, 0, 90f);
    }
    
}
