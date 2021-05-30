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

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {

                print(hit.transform.gameObject);


                if (!hit.transform.GetComponent<PiecesScripts>().InRightPosition)
                {
                    selectedPiece.transform.position = hit.transform.position;
                    selectedPiece.GetComponent<PiecesScripts>().Selected = true;
                }

            }
            //마우스 우클릭을 했을때 
            //클릭한 놈의 GameObject를 움직일 수 있게 하자
        }
        if (Input.GetMouseButtonUp(1))
        {
            selectedPiece = null;
            selectedPiece.GetComponent<PiecesScripts>().Selected = true;
        }

        //if(selectedPiece == null)
        //{
        //    Vector3 mousePoint = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    selectedPiece.transform.position = new Vector3(mousePoint.x, mousePoint.y, 0);
        //}

    }
}
