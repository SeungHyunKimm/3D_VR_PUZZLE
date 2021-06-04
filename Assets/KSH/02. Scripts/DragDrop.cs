using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop : MonoBehaviour
{
    public GameObject selectedPiece;
    bool isClick;
    Ray ray;
    RaycastHit hit;
    LineRenderer lr;

    void Start()
    {
        lr = GetComponent<LineRenderer>();

    }

    void Update()
    {
        DrawGuideLine();
        ObjectRay();



    }

    void ObjectRay()
    {
        ray = new Ray(transform.position, transform.forward);
        float v = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch);
        if (v > 0)
        {
            isClick = true;
            //selectedPiece = null;
            if (Physics.Raycast(ray, out hit, 100))
            {
                print(hit.transform.gameObject);
                selectedPiece = hit.transform.gameObject;
                for (int i = 0; i < 36; i++)
                {
                    int layer = 1 << LayerMask.NameToLayer("Puzzle");
                    if (Physics.Raycast(ray, out hit, 100, layer))
                    {

                        //print(selectedPiece);
                        
                        selectedPiece.transform.position = new Vector2(hit.point.x, hit.point.y);


                                               //트리거 당긴 넘의 z축을 이동하지 않게 하고 싶다.

                    }
                }
                //마우스 우클릭을 했을때 
                //클릭한 놈의 GameObject를 움직일 수 있게 하자
            }
        }

        else if (v < 0)
        {
            isClick = false;
        }
    }

    void DrawGuideLine()
    {
        //레이와 부딪힌 놈까지
        if (Physics.Raycast(ray, out hit))
        {
            //거리를 구해서 라인을 그린다.
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, hit.point);
        }
        else
        {
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, transform.position + transform.forward * 1);
        }
    }
}








