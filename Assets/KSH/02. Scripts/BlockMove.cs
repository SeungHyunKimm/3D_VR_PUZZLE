using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMove : MonoBehaviour
{
    //Ŭ�� ��ġ�� ����
    GameObject Position1;
    GameObject Position2;
    //���콺 Ŭ�� Ƚ��
    int cnt;

    //�׸��� �� ����
    public static int width = 10;
    public static int height = 10;
    static Transform[,] grid = new Transform[width, height];
    //ť�� ��ġ ���� ����
    public GameObject[] Blocks;
    //�׸��� �� ���� ����
    Transform blockPos1;


    void Start()
    {


        SetGrid();


    }

    void Update()
    {

        OnClickLeftMouse();

    }

    void SetGrid()
    {
        //�׸��尪
        //�� ���ϵ��� x,y���� 
        //�ݺ��� 100���� ������
        for (int i = 0; i < 10; i++) // 0 -> 9
        {
            for (int j = 0; j < 10; j++)
            {
                //x,y ��ǥ���� ���� ���� index
                int index = ((i + j) + 9 * i);

                //index�� Grid�� ����
                grid[i, j] = Blocks[index].transform;

                //������ x,y���� �����.
                int x = Random.Range(0, i);
                int y = Random.Range(0, j);
                //���� �׸��忡 ���� ���� �ִ´�.

                Vector3 TempGrid = grid[i, j].transform.position;

                //Shuffle�Լ� ���� ���� ��ǥ���� �ִ´�.
                //temp���� ���� ��ġ�� Swap�Ѵ�.
                grid[i, j].transform.position = grid[x, y].transform.position;
                grid[x, y].transform.position = TempGrid;
                
                //���� ó���� ������(������ ����ġ ������ ��)
                //�� ���� 10������ ���� �ϼ��Ǹ� ���� �ϼ��� ����Ʈ �غ���.
            }
        }
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