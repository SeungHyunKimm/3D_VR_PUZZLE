using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Itween_BlockMove : MonoBehaviour
{
    bool isClick = false;
    void Start()
    {

        if(OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch))
        {

            isClick = true;
            

        }
    }

    void Update()
    {




        
    }
    
}
