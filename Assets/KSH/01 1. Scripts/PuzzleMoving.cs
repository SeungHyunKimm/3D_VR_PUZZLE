using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleMoving : MonoBehaviour
{
    bool isCLick = false;
    GameObject selectObj;

    void Start()
    {



    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isCLick = true;
            selectObj = null;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;


            int layer = 1 << LayerMask.NameToLayer("Puzzle");

            if (Physics.Raycast(ray, out hit, 100, layer))
            {
                

                print(hit.transform.gameObject.name);
                selectObj = hit.transform.gameObject;

                
            }
        }

        else if (Input.GetMouseButtonDown(0))
        {

            isCLick = false;

        }

        //Ŭ���ϴ� ��
        if (isCLick == true && selectObj != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            int layer = 1 << LayerMask.NameToLayer("Ground");

            if (Physics.Raycast(ray, out hit, 100, layer))
            {

                selectObj.transform.position = hit.point;

            }
        }
    }

    //Ŭ���ϴ� ���� ���������� ���콺���� ������Ʈ�� �ȶ�����

}

