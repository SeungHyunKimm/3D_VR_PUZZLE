using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public static int height = 11;
    public static int width = 11;
    private static Transform[,] grid = new Transform[width, height];
    Material mat;            //ĵ���� ��ü ����
    int plusMinus;           //ĵ���� �� ��� �÷��� ���̳ʽ�
    float albedo;
    public GameObject[] puzzle; //������� �׸��� �� ���� ������.
    public GameObject[] quad;   //���� Ȱ�� ��Ȱ�� üũ��.
    public bool[] checkpuzz;    //������� ���� ��ġ�� ��ġ�ϰ� �ִ��� �˾Ƴ��� ����
    Material pz, qd;            //����� ������ Material
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

    void SetAlbedo(Material mat)                                         //ĵ������ ��� ����
    {

        if (mat.color.a >= 1 || mat.color.a <= 0)
            plusMinus *= -1;

        albedo += 0.001f * plusMinus;
        float r = mat.color.r;
        float g = mat.color.g;
        float b = mat.color.b;
        mat.color = new Color(r, g, b, albedo);
    }

    private void OnCollisionEnter(Collision collision)        //������ ĵ������ �������� �� ���� �ۿ� ����
    {
        PuzzleManager pr = collision.transform.GetComponent<PuzzleManager>();
        if (pr.state == PuzzleManager.PuzzleState.Fusion)
        {
            pr.state = PuzzleManager.PuzzleState.Fixed;
            collision.rigidbody.isKinematic = true;
            CheckBox(collision.gameObject);
            if (GameClear())                                     //������ �ٸ��߾��� �� ȿ�� �߻� �� ���� ����
            {
                blackHole.SetActive(true);
            }
        }
        else if(pr.state != PuzzleManager.PuzzleState.Fixed)                                         //ƨ�ܳ���
        {
            collision.rigidbody.AddForce(transform.position * -1, ForceMode.Impulse);
            pr.state = PuzzleManager.PuzzleState.Revolution;
        }
    }

    void CheckBox(GameObject puzzle)                                            //������ �ٿ��� �� ����ó�� ����
    {
        int index = GetIndex(puzzle);
        for (int i = 0; i < puzzle.transform.childCount; i++)
        {
            int positionX = Mathf.RoundToInt(puzzle.transform.GetChild(i).position.x);
            int positionY = Mathf.RoundToInt(puzzle.transform.GetChild(i).position.y);
            if (positionX >= 0 && positionX < width && positionY >= 0 && positionY < height) //ĵ������ ����
            {
                int positionindex = positionX + positionY * height;
                qd = quad[positionindex].GetComponent<MeshRenderer>().material; //������ ���� ����
                pz = puzzle.transform.GetComponent<MeshRenderer>().material;
                if (grid[positionX, positionY] == null || grid[positionX , positionY].name != puzzle.transform.GetChild(i).name)     //���� Ʋ�� �ƴϰų� ����� �׸����� �̸��� ��ġ���� ������ false
                {
                    checkpuzz[index] = false;
                    return;
                }
                else if(grid[positionX , positionY].name == puzzle.transform.GetChild(i).name)    //������ �̸��� �׸��� ���� ��ġ�ϸ� ���ٲ�
                {
                    qd.color = pz.color;                    //���� ���� ��ġ��� ���带 ���� �������� ���� 
                }
            }
        }
        checkpuzz[index] = true;                     //������ ������ ĵ�۽��� Ʋ���� ��ġ�� ������ true üũ
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

    void SetPuzzlePosition()                                //11 * 11 �ǿ� ������ ��ǥ �����ϴ� �Լ�
    {
        int k = 0;
        while (k < puzzle.Length)
        {
            int x = Random.Range(0, width);
            int y = Random.Range(0, height);
            int index = x + (height * y);                 //Canvas �� �ڽ� Quad���� �ε��� ����
            if (CheckGrid(k, index)) continue;            //������� ��ġ �ߺ� ����
            SetQuadColor(x, y, k, index);
            k++;
        }
    }

    bool CheckGrid(int k, int index)                              //������� ��ġ �ߺ� ����
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

    void SetQuadColor(int x, int y, int k, int index)               //ĵ���� �ǿ� ����Ʋ ����
    {
        quad[index].SetActive(true);
        puzzle[k].transform.position = quad[index].transform.position - new Vector3(0, 0, 4);         //������ ���� ��ǥ�� ��ġ ��Ű��.
        for (int i = 0; i < puzzle[k].transform.childCount; i++)
        {
            int positionX = Mathf.RoundToInt(puzzle[k].transform.GetChild(i).position.x);
            int positionY = Mathf.RoundToInt(puzzle[k].transform.GetChild(i).position.y);
            if (positionX >= 0 && positionX < width && positionY >= 0 && positionY < height)
            {
                int positionindex = positionX + (height * positionY);                      //����� ������ ��ġ�� �κ��� ������
                quad[positionindex].SetActive(true);
                grid[positionX, positionY] = puzzle[k].transform.GetChild(i);
            }
        }
    }
}
