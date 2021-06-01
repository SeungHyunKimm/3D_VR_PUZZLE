using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public static int height = 11;
    public static int width = 11;
    private static Transform[,] grid = new Transform[width, height];
    Material mat;            //캔버스 자체 색상
    int plusMinus;           //캔버스 판 명암 플러스 마이너스
    float albedo;
    public GameObject[] puzzle; //퍼즐들의 그리드 및 색상 참조용.
    public GameObject[] quad;   //쿼드 활성 비활성 체크용.
    public bool[] checkpuzz;    //퍼즐들이 원래 위치에 위치하고 있는지 알아내는 여부
    Material pz, qd;            //퍼즐과 쿼드의 Material
    public GameObject blackHole;


    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
        plusMinus = 1;
        SetPuzzlePosition();
    }

    private void Update()
    {
        //SetAlbedo(mat);
    }

    void SetAlbedo(Material mat)                                         //캔버스의 명암 조절
    {

        if (mat.color.a >= 1 || mat.color.a <= 0)
            plusMinus *= -1;

        albedo += 0.001f * plusMinus;
        float r = mat.color.r;
        float g = mat.color.g;
        float b = mat.color.b;
        mat.color = new Color(r, g, b, albedo);
    }

    private void OnCollisionEnter(Collision collision)        //퍼즐이 캔버스에 끼워졌을 때 물리 작용 제거
    {
        PuzzleManager pr = collision.transform.GetComponent<PuzzleManager>();
        if (pr.state == PuzzleManager.PuzzleState.Fusion)
        {
            pr.state = PuzzleManager.PuzzleState.Fixed;
            collision.rigidbody.isKinematic = true;
            CheckBox(collision.gameObject);
            if (GameClear())                                     //퍼즐을 다맞추었을 때 효과 발생 및 게임 종료
            {
                blackHole.SetActive(true);
            }
        }
        else if(pr.state != PuzzleManager.PuzzleState.Fixed)                                         //튕겨내기
        {
            collision.rigidbody.AddForce(transform.position * -1, ForceMode.Impulse);
            pr.state = PuzzleManager.PuzzleState.Revolution;
        }
    }

    void CheckBox(GameObject puzzle)                                            //퍼즐을 붙였을 때 정답처리 여부
    {
        int index = GetIndex(puzzle);
        for (int i = 0; i < puzzle.transform.childCount; i++)
        {
            int positionX = Mathf.RoundToInt(puzzle.transform.GetChild(i).position.x);
            int positionY = Mathf.RoundToInt(puzzle.transform.GetChild(i).position.y);
            if (positionX >= 0 && positionX < width && positionY >= 0 && positionY < height) //캔버스의 범위
            {
                int positionindex = positionX + positionY * height;
                qd = quad[positionindex].GetComponent<MeshRenderer>().material; //쿼드의 색상 변경
                pz = puzzle.transform.GetComponent<MeshRenderer>().material;
                if (grid[positionX, positionY] == null || grid[positionX , positionY].name != puzzle.transform.GetChild(i).name)     //퍼즐 틀이 아니거나 퍼즐과 그리드의 이름이 일치하지 않으면 false
                {
                    checkpuzz[index] = false;
                    return;
                }
                else if(grid[positionX , positionY].name == puzzle.transform.GetChild(i).name)    //퍼즐의 이름과 그리드 값이 일치하면 색바꿈
                {
                    qd.color = pz.color;                    //원래 퍼즐 위치라면 쿼드를 퍼즐 색상으로 변경 
                }
            }
        }
        checkpuzz[index] = true;                     //끼워진 퍼즐이 캔퍼스와 틀에서 위치와 같으면 true 체크
        //pz.SetColor("_EmissionColor", pz.color * 10);
    }

    bool GameClear()
    {
        for (int i = 0; i < checkpuzz.Length; i++)
        {
            if (!checkpuzz[i])
            {
                checkpuzz[i] = false;
                return false;
            }
        }
        return true;
    }

    int GetIndex(GameObject puzz)
    {
        for (int i = 0; i < puzzle.Length; i++)
        {
            if (puzz == puzzle[i])
                return i;
        }
        return puzzle.Length;
    }

    public void CatchToCheckBox(int index)   
    {
        checkpuzz[index] = false;
    }

    void SetPuzzlePosition()                                //11 * 11 판에 퍼즐의 좌표 지정하는 함수
    {
        int k = 0;
        while (k < puzzle.Length)
        {
            int x = Random.Range(0, width);
            int y = Random.Range(0, height);
            int index = x + (height * y);                 //Canvas 의 자식 Quad들의 인덱스 접근
            if (CheckGrid(k, index)) continue;            //퍼즐들의 위치 중복 제거
            SetQuadColor(x, y, k, index);
            k++;
        }
    }

    bool CheckGrid(int k, int index)                              //퍼즐들의 위치 중복 제거
    {
        puzzle[k].transform.position = quad[index].transform.position;
        for (int i = 0; i < puzzle[k].transform.childCount; i++)
        {
            int positionX = Mathf.RoundToInt(puzzle[k].transform.GetChild(i).position.x);
            int positionY = Mathf.RoundToInt(puzzle[k].transform.GetChild(i).position.y);
            if (positionX >= 0 && positionX < width && positionY >= 0 && positionY < height)
            {
                if (grid[positionX, positionY])
                {
                    return true;
                }
            }
        }
        return false;
    }

    void SetQuadColor(int x, int y, int k, int index)               //캔버스 판에 퍼즐틀 제작
    {
        quad[index].SetActive(true);
        puzzle[k].transform.position = quad[index].transform.position - new Vector3(0, 0, 4);         //퍼즐을 쿼드 좌표에 위치 시키기.
        for (int i = 0; i < puzzle[k].transform.childCount; i++)
        {
            int positionX = Mathf.RoundToInt(puzzle[k].transform.GetChild(i).position.x);
            int positionY = Mathf.RoundToInt(puzzle[k].transform.GetChild(i).position.y);
            if (positionX >= 0 && positionX < width && positionY >= 0 && positionY < height)
            {
                int positionindex = positionX + (height * positionY);                      //쿼드와 퍼즐이 겹치는 부분을 재조정
                quad[positionindex].SetActive(true);
                grid[positionX, positionY] = puzzle[k].transform.GetChild(i);
            }
        }
    }
}
