using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetClick : MonoBehaviour
{
    //��ŸƮ �� ĵ����
    public GameObject StartCanvas;
    Ray ray;
    RaycastHit hit;
    public static PlanetClick instance;

    void Start()
    {

        StartCanvas.SetActive(false);

    }

    void Update()
    {
        ////���̸� ���� 
        //ray = new Ray(transform.position, transform.forward);
        //float v = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch);

        //if (v > 0)
        //{
        //    if (Physics.Raycast(ray, out hit))
        //    {
        //        if(hit.transform.gameObject.name == "Saturn")
        //        {
        //            print(hit.transform.gameObject.name);
        //            StartCanvas.SetActive(true);
        //        }
        //    }
        //}
    }
}
