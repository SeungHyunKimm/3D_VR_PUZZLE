using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Itween_StartCanvasMove : MonoBehaviour
{
    void Start()
    {

                iTween.MoveBy(gameObject, iTween.Hash("x", 250, "time", 2, "easetype", iTween.EaseType.easeOutCirc));

    }
    
}
