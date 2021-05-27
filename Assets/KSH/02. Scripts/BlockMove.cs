using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMove : MonoBehaviour
{
    //클릭 위치값 저장
    GameObject Position1;
    GameObject Position2;
    //마우스 클릭 횟수
    int cnt;


    void Start()
    {






    }

    void Update()
    {

        OnClickLeftMouse();

    }
    void OnClickLeftMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (cnt == 0)
                {
                    Position1 = hit.transform.gameObject;
                    cnt++;
                }
                else if (cnt == 1)
                {

                    Position2 = hit.transform.gameObject;
                    Vector3 temp = Position1.transform.position;
                    Position1.transform.position = Position2.transform.position;
                    Position2.transform.position = temp;

                    Position1 = null;
                    Position2 = null;
                    cnt = 0;
                }
            }
        }

    }

}
