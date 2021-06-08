using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Itween_ChooseModeScene : MonoBehaviour
{
    void Start()
    {


        iTween.MoveBy(gameObject, iTween.Hash("x", -1000, "time", 2, "easetype", iTween.EaseType.easeOutCirc));



    }
}
