using Photon.Pun;
using UnityEngine;

public class CanvasManager : MonoBehaviourPun//, IPunObservable
{
    public static int height = 11;
    public static int width = 11;
    private static Transform[,] grid = new Transform[width, height];

    public GameObject absorbEffect;
    public PuzzleManager[] puzzle; //퍼즐들의 그리드 및 색상 참조용.
    public GameObject[] quad;   //쿼드 활성 비활성 체크용.
    public bool[] checkpuzz;    //퍼즐들이 원래 위치에 위치하고 있는지 알아내는 여부
    Material pz, qd;            //퍼즐과 쿼드의 Material
    PuzzleManager pr;
    PC_AIPlayerControl AI;

    // Start is called before the first frame update
    void Start()
    {
        //GameObject getPuzzle = GameObject.Find("Puzzle");
        //puzzle = new GameObject[getPuzzle.transform.childCount];
        //for(int i = 0; i < getPuzzle.transform.childCount; i++)
        //    puzzle[i] = getPuzzle.transform.GetChild(i).gameObject;
    }

    private void OnCollisionEnter(Collision collision)        //퍼즐이 캔버스에 끼워졌을 때 물리 작용 제거
    {
        if (collision.transform.gameObject.layer == LayerMask.NameToLayer("Puzzle"))
        {
            pr = collision.transform.GetComponent<PuzzleManager>();
            if (pr.state == PuzzleManager.PuzzleState.Fusion)
            {
                pr.state = PuzzleManager.PuzzleState.Fixed;
                pr.Fixed();
                CheckBox(collision.gameObject);
                if (GameClear())                                     //퍼즐을 다맞추었을 때 효과 발생 및 게임 종료
                {
                    AI = GameObject.Find("AIPlayer").GetComponent<PC_AIPlayerControl>();
                    AI.state = PC_AIPlayerControl.AIPlayerState.End;
                    PhotonNetwork.Instantiate("BlackHole", transform.position, Quaternion.identity);
                }
            }
            else if (pr.state != PuzzleManager.PuzzleState.Fixed)                                         //튕겨내기
            {
                if (PhotonNetwork.IsMasterClient) Collision(collision.gameObject);
                pr.state = PuzzleManager.PuzzleState.Revolution;
            }
        }
    }
    void Collision(GameObject collision)
    {
        collision.GetComponent<Rigidbody>().AddForce(collision.transform.position - transform.position * -2 * Time.deltaTime, ForceMode.Impulse);
    }

    void CheckBox(GameObject puzz)                                            //퍼즐을 붙였을 때 정답처리 여부
    {
        int index = GetIndex(puzz);
        if (index == puzzle.Length) return;
        for (int i = 0; i < puzz.transform.childCount ; i++)
        {
            int positionX = Mathf.RoundToInt(puzz.transform.GetChild(i).position.x);
            int positionY = Mathf.RoundToInt(puzz.transform.GetChild(i).position.y);
            if (positionX >= 0 && positionX < width && positionY >= 0 && positionY < height) //캔버스의 범위
            {
                int positionindex = positionX + positionY * height;
                qd = quad[positionindex].GetComponent<MeshRenderer>().material; //쿼드의 색상 변경
                pz = puzz.transform.GetComponent<MeshRenderer>().material;
                if (grid[positionX, positionY] == null || grid[positionX, positionY].name != puzz.transform.GetChild(i).name)     //퍼즐 틀이 아니거나 퍼즐과 그리드의 이름이 일치하지 않으면 false
                {
                    checkpuzz[index] = false;
                    return;
                }
                else if (grid[positionX, positionY].name == puzz.transform.GetChild(i).name)    //퍼즐의 이름과 그리드 값이 일치하면 색바꿈
                    qd.color = pz.color;                    //원래 퍼즐 위치라면 쿼드를 퍼즐 색상으로 변경 
            }
        }
        //Absorb(puzz);
        pr.state = PuzzleManager.PuzzleState.Clear;
        //print(pr.state);
        pr.Clear();
        pr.GetComponent<Rigidbody>().isKinematic = true;
        checkpuzz[index] = true;                     //끼워진 퍼즐이 캔퍼스와 틀에서 위치와 같으면 true 체크
        //pz.SetColor("_EmissionColor", pz.color * 10);
    }

    void Absorb(GameObject pz)
    {
        GameObject eff = Instantiate(absorbEffect);
        Destroy(eff, 2);
        Destroy(pz , 2);
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
            if (puzz.name == puzzle[i].name)
                return i;
        }
        return puzzle.Length;
    }

    public void CatchToCheckBox(int index)
    {
        checkpuzz[index] = false;
    }

    public void SetPuzzlePosition(int length)                                //11 * 11 판에 퍼즐의 좌표 지정하는 함수
    {
        int k = 0;
        while (k < length)
        {
            int x = Random.Range(1, width);
            int y = Random.Range(1, height);
            int index = x + (height * y);                 //Canvas 의 자식 Quad들의 인덱스 접근
            if (CheckGrid(k, index)) continue;            //퍼즐들의 위치 중복 제거
            photonView.RPC("SetQuadColor", RpcTarget.AllBuffered, x, y, k, index);   //모든 PC에 퍼즐 틀 위치 전해주기
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
                    return true;
            }
        }
        return false;
    }
    [PunRPC]
    void SetQuadColor(int x, int y, int k, int index)               //캔버스 판에 퍼즐틀 제작
    {
        quad[index].SetActive(true);
        puzzle[k].transform.position = quad[index].transform.position + new Vector3(0, 0, 6);         //퍼즐을 쿼드 좌표에 위치 시키기.
        quad[index].SetActive(false);
        for (int i = 0; i < puzzle[k].transform.childCount; i++)
        {
            int positionX = Mathf.RoundToInt(puzzle[k].transform.GetChild(i).position.x);
            int positionY = Mathf.RoundToInt(puzzle[k].transform.GetChild(i).position.y);
            if (positionX >= 0 && positionX < width && positionY >= 0 && positionY < height)
            {
                int positionindex = positionX + (height * positionY);                      //쿼드와 퍼즐이 겹치는 부분을 재조정
                quad[positionindex].SetActive(true);
                grid[positionX, positionY] = puzzle[k].transform.GetChild(i);
                //qd = quad[positionindex].GetComponent<MeshRenderer>().material; //쿼드의 색상 변경
                //pz = puzzle[k].transform.GetComponent<MeshRenderer>().material;
                //qd.color = pz.color;
            }
        }
    }
}
