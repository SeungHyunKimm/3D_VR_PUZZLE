using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public static int height = 11;
    public static int width = 11;
    private static Transform[,] grid = new Transform[width, height];
    Rigidbody rigid;         //���� ������ ��ü�� rigid 
    Material mat;            //ĵ���� ��ü ����
    int k;
    float albedo;
    public GameObject[] puzzle;//������� �׸��� �� ���� ������.
    MeshRenderer [] quadMat; //���� �� ������ ��ġ�� �´� ���� ���.
    public GameObject [] quad;  //���� Ȱ�� ��Ȱ�� üũ��.

    // Start is called before the first frame update
    void Start()
    {
        quadMat = GetComponentsInChildren<MeshRenderer>();
        mat = GetComponent<MeshRenderer>().material;
        k = 1;
        SetPuzzlePosition();
    }

    private void Update()
    {
        SetAlbedo(mat);
    }

    void SetAlbedo(Material mat)                                         //ĵ������ ��� ����
    {
         
        if (mat.color.a >= 1 || mat.color.a <= 0)
            k *= -1;

        albedo += 0.001f * k;
        float r = mat.color.r;
        float g = mat.color.g;
        float b = mat.color.b;
        mat.color = new Color(r, g , b , albedo);
    }

    private void OnCollisionEnter(Collision collision)        //������ ĵ������ �������� �� ���� �ۿ� ����
    {
        rigid = collision.transform.GetComponent<Rigidbody>();
        rigid.isKinematic = true;
    }

    void SetPuzzlePosition()                                //11 * 11 �ǿ� ������ ��ǥ �����ϴ� �Լ�
    {
        for(int k = 0; k < puzzle.Length; k++)
        {
            int x = Random.Range(0, 11);
            int y = Random.Range(0, 11);
            if (grid[x, y]) continue;                  //x , y�� �ߺ� ����
            int index = x + (11 * y);                  //Canvas �� �ڽ� Quad���� �ε��� ����
            SetQuad(x, y, k, index);
        }
    }

    void SetQuad(int x , int y , int k , int index)               //ĵ���� �ǿ� ����Ʋ ���߱�
    {
        quad[index].SetActive(true);
        puzzle[k].transform.position = quad[index].transform.position;         //������ ���� ��ǥ�� ��ġ ��Ű��.
        for (int i = 0; i < puzzle[k].transform.childCount; i++)
        {
            int positionX = Mathf.RoundToInt(puzzle[k].transform.GetChild(i).position.x);
            int positionY = Mathf.RoundToInt(puzzle[k].transform.GetChild(i).position.y);
            if(positionX >= 0 && positionX < width && positionY >= 0 && positionY < height)
            {
              int positionindex = positionX + (11 * positionY);                      //����� ������ ��ġ�� �κ��� ������
              quad[positionindex].SetActive(true);
              grid[positionX, positionY] = puzzle[k].transform.GetChild(i);
              Material gr = quad[positionindex].GetComponent<MeshRenderer>().material;
              Material pz = puzzle[k].transform.GetComponent<MeshRenderer>().material;
              gr.color = pz.color;
            }
        }
        puzzle[k].SetActive(false);
    }
}
