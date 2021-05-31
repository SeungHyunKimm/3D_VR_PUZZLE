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
                //���콺 ��Ŭ���� ������ 
                //Ŭ���� ���� GameObject�� ������ �� �ְ� ����
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
                        //selectedPiece.GetComponent<PiecesScripts>().Selected = true;
                        
                    }
                }
            }
        }
    }
}
