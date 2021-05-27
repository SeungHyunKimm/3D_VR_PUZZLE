using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public static int height = 11;
    public static int width = 11;
    private static Transform[,] grid = new Transform[width, height];
    Rigidbody rigid;         //퍼즐에 끼워진 물체의 rigid 
    Material mat;            //캔버스 자체 색상
    int k;
    float albedo;
    public GameObject[] puzzle;//퍼즐들의 그리드 및 색상 참조용.
    MeshRenderer [] quadMat; //시작 시 퍼즐의 위치에 맞는 색상 담기.
    public GameObject [] quad;  //쿼드 활성 비활성 체크용.

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

    void SetAlbedo(Material mat)                                         //캔버스의 명암 조절
    {
         
        if (mat.color.a >= 1 || mat.color.a <= 0)
            k *= -1;

        albedo += 0.001f * k;
        float r = mat.color.r;
        float g = mat.color.g;
        float b = mat.color.b;
        mat.color = new Color(r, g , b , albedo);
    }

    private void OnCollisionEnter(Collision collision)        //퍼즐이 캔버스에 끼워졌을 때 물리 작용 제거
    {
        rigid = collision.transform.GetComponent<Rigidbody>();
        rigid.isKinematic = true;
    }

    void SetPuzzlePosition()                                //11 * 11 판에 퍼즐의 좌표 지정하는 함수
    {
        for(int k = 0; k < puzzle.Length; k++)
        {
            int x = Random.Range(0, 11);
            int y = Random.Range(0, 11);
            if (grid[x, y]) continue;                  //x , y의 중복 제거
            int index = x + (11 * y);                  //Canvas 의 자식 Quad들의 인덱스 접근
            SetQuad(x, y, k, index);
        }
    }

    void SetQuad(int x , int y , int k , int index)               //캔버스 판에 퍼즐틀 맞추기
    {
        quad[index].SetActive(true);
        puzzle[k].transform.position = quad[index].transform.position;         //퍼즐을 쿼드 좌표에 위치 시키기.
        for (int i = 0; i < puzzle[k].transform.childCount; i++)
        {
            int positionX = Mathf.RoundToInt(puzzle[k].transform.GetChild(i).position.x);
            int positionY = Mathf.RoundToInt(puzzle[k].transform.GetChild(i).position.y);
            if(positionX >= 0 && positionX < width && positionY >= 0 && positionY < height)
            {
              int positionindex = positionX + (11 * positionY);                      //쿼드와 퍼즐이 겹치는 부분을 재조정
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
