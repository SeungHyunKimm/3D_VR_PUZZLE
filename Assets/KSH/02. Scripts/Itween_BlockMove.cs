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
            iTween.MoveBy(gameObject, iTween.Hash("y", -5, "time", 2.5f, iTween.EaseType.easeInBounce));
            iTween.MoveBy(gameObject, iTween.Hash("y", 5, "time", 2.5f, iTween.EaseType.easeInBounce));

        }
    }

    void Update()
    {




        
    }
}
