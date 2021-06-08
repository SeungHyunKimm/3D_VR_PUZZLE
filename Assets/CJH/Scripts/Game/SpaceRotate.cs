using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceRotate : MonoBehaviour
{
    public float speed;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0 , speed , 0);         //행성 공전 및 자전
    }
}
