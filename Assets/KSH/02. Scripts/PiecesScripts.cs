using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiecesScripts : MonoBehaviour
{
    Vector3 rightposition;
    public bool InRightPosition;
    public bool Selected;
    void Start()
    {
        rightposition = transform.position;
        transform.position = new Vector3(Random.Range(11, 20), Random.Range(9, 1.5f));
    }

    void Update()
    {
        if(Vector3.Distance(transform.position, rightposition) < 1f)
        {
        //    if (!Selected)
        //    {
            transform.position = rightposition;
        //    InRightPosition = true;
        //    }
        }
    }
}
