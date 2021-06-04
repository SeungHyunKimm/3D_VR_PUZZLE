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


                                               //Ʈ���� ��� ���� z���� �̵����� �ʰ� �ϰ� �ʹ�.

                    }
                }
                //���콺 ��Ŭ���� ������ 
                //Ŭ���� ���� GameObject�� ������ �� �ְ� ����
            }
        }

        else if (v < 0)
        {
            isClick = false;
        }
    }

    void DrawGuideLine()
    {
        //���̿� �ε��� �����
        if (Physics.Raycast(ray, out hit))
        {
            //�Ÿ��� ���ؼ� ������ �׸���.
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








