using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop : MonoBehaviour
{
    public GameObject selectedPiece;
    bool isClick;
    void Start()
    {

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isClick = true;
            selectedPiece = null;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {

                print(hit.transform.gameObject);
                for (int i = 0; i < 37; i++)
                {
                    if (hit.transform.gameObject.name == ("Pieces_"+"(" + i + ")"))
                    {
                        print(hit.transform.gameObject.name);
                        selectedPiece = hit.transform.gameObject;
                    }
                }
                //���콺 ��Ŭ���� ������ 
                //Ŭ���� ���� GameObject�� ������ �� �ְ� ����
            }
            //if (Input.GetMouseButtonUp(1))
            //{
            //    selectedPiece = null;
            //    selectedPiece.GetComponent<PiecesScripts>().Selected = true;
            //}

            //if (selectedPiece == null)
            //{
            //    Ray mousePoint = Camera.main.ScreenPointToRay(Input.mousePosition);

            //    selectedPiece.transform.position = new Vector3(mousePoint.x, mousePoint.y, 0);
            //}

        }
    }
}
