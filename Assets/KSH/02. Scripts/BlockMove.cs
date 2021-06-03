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

    //그리드 값 설정
    public static int width = 10;
    public static int height = 10;
    static Transform[,] grid = new Transform[width, height];
    //큐브 위치 담을 변수
    public GameObject[] Blocks;
    //그리드 값 받을 변수
    Transform blockPos1;


    void Start()
    {


        SetGrid();


    }

    void Update()
    {

        OnClickRTouch();

    }

    void SetGrid()
    {
        //그리드값
        //각 블록들의 x,y값을 
        //반복문 100개를 돌린다
        for (int i = 0; i < 10; i++) // 0 -> 9
        {
            for (int j = 0; j < 10; j++) // 0 -> 9
            {
                //x,y 좌표값을 담을 변수 index
                int index = ((i + j) + 9 * i);
                //index를 Grid에 담자
                grid[i, j] = Blocks[index].transform;

                //임의의 x,y값을 만든다.
                int x = Random.Range(0, i);
                int y = Random.Range(0, j);
                //담은 그리드에 랜덤 값을 넣는다.

                Vector3 TempGrid = grid[i, j].transform.position;

                //Shuffle함수 만들어서 랜덤 좌표값을 넣는다.
                //temp변수 만들어서 위치를 Swap한다.
                grid[i, j].transform.position = grid[x, y].transform.position;
                grid[x, y].transform.position = TempGrid;

                //정답 처리도 만들어보자(퍼즐을 원위치 시켰을 때)
                //각 색깔 10개마다 줄이 완성되면 정답 완성을 프린트 해보자.
                //각 색깔줄마다 수시로 검사를 하여 만약 한줄의 컬러가 같으면 정답 처리.
            }
        }
    }

    //void ColorAnswer(int index)
    //{
    //    //if(index.t)

    //}


    void OnClickRTouch()
    {
        //VR용
        //Ray ray = new Ray(transform.position, transform.forward);
        //마우스클릭용
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (Input.GetMouseButtonDown(0) || OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch))
            {
                if (cnt == 0)
                {
                    Position1 = hit.transform.gameObject;
                    cnt++;
                }

                else if (cnt == 1)
                {
                    //클릭한 놈 x,y값 지역변수 선언

                    //int x = (int)hit.transform.position.x;
                    //int y = (int)hit.transform.position.y;


                    //만약 grid[x, y]값의 이름이 마우스 클릭한 놈의 이름이 같다면
                    //if (grid[x, y].transform.gameObject.name == hit.transform.gameObject.name)
                    //{
                    //print(grid[x, y].transform.gameObject.name);

                    //정답처리를 한다.

                    //}

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
