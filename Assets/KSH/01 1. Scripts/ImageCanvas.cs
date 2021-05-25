using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageCanvas : MonoBehaviour
{
    Rigidbody rigid;

    void Start()
    {

        rigid = GetComponent<Rigidbody>();


    }

    void Update()
    {
        //쿼드랑 부딪히는 물체들을 못지나가게 하고 싶다.
        
        
    }
}
