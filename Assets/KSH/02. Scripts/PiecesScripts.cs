using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiecesScripts : MonoBehaviour
{
    Vector3 rightposition;
    public bool InRightPosition;
    public bool Selected;
    // Start is called before the first frame update
    void Start()
    {
        rightposition = transform.position;
        transform.position = new Vector3(Random.Range(11, 20), Random.Range(9, 1.5f));
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, rightposition) > 30f)
        {
            transform.position = rightposition;
            InRightPosition = true;
        }

    }
}