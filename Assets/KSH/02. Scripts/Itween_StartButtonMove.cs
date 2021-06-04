using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Itween_StartButtonMove : MonoBehaviour
{
    void Start()
    {
        


    }

    void Update()
    {

        StartCoroutine(ItweenStartButtonMove());

    }


    IEnumerator ItweenStartButtonMove()

    {
        yield return new WaitForSeconds(3);


        iTween.RotateBy(gameObject, iTween.Hash("x", .25, "easeType", "easeInOutBack", "loopType", "pingPong", "delay", .4));

    }
}
