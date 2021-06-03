using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop : MonoBehaviour
{
    public GameObject selectedPiece;
    bool isClick;
    Ray ray;
    RaycastHit hit;
    
    void Start()
    {

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isClick = true;
            selectedPiece = null;
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            

            if (Physics.Raycast(ray, out hit))
            {

                print(hit.transform.gameObject);
                for (int i = 0; i < 37; i++)
                {
                    if (hit.transform.gameObject.name == ("Pieces_("+i+ ")"))
                    {
                        print(hit.transform.gameObject.name);
                        selectedPiece = hit.transform.gameObject;
                    }
                }
                //마우스 우클릭을 했을때 
                //클릭한 놈의 GameObject를 움직일 수 있게 하자
            }

            else if (Input.GetMouseButtonUp(1))
            {
                isClick = false;
            }

            if (isClick == true && selectedPiece != null)
            {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                int layer = 1 << LayerMask.NameToLayer("Puzzle");   
                if(Physics.Raycast(ray, out hit))
                {
                    if (!hit.transform.GetComponent<PiecesScripts>().InRightPosition)
                    {
                        selectedPiece = hit.transform.gameObject;
                        selectedPiece.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
                        //selectedPiece.GetComponent<PiecesScripts>().Selected = true;
                    }
                }
            }
        }

    }
}









//현재 문제(21.06.02)
//1. 마우스로 클릭을 했는데 드래그와 움직이기가 안되고 클릭시 정답위치로 퍼즐이 순간이동한다.

