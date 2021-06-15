using Photon.Pun;
using UnityEngine;

public class CanvasManager : MonoBehaviourPun//, IPunObservable
{
    public static int height = 11;
    public static int width = 11;
    private static Transform[,] grid = new Transform[width, height];

    public GameObject absorbEffect;
    public PuzzleManager[] puzzle; //������� �׸��� �� ���� ������.
    public GameObject[] quad;   //���� Ȱ�� ��Ȱ�� üũ��.
    public bool[] checkpuzz;    //������� ���� ��ġ�� ��ġ�ϰ� �ִ��� �˾Ƴ��� ����
    Material pz, qd;            //����� ������ Material
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

    private void OnCollisionEnter(Collision collision)        //������ ĵ������ �������� �� ���� �ۿ� ����
    {
        if (collision.transform.gameObject.layer == LayerMask.NameToLayer("Puzzle"))
        {
            pr = collision.transform.GetComponent<PuzzleManager>();
            if (pr.state == PuzzleManager.PuzzleState.Fusion)
            {
                pr.state = PuzzleManager.PuzzleState.Fixed;
                pr.Fixed();
                CheckBox(collision.gameObject);
                if (GameClear())                                     //������ �ٸ��߾��� �� ȿ�� �߻� �� ���� ����
                {
                    AI = GameObject.Find("AIPlayer").GetComponent<PC_AIPlayerControl>();
                    AI.state = PC_AIPlayerControl.AIPlayerState.End;
                    PhotonNetwork.Instantiate("BlackHole", transform.position, Quaternion.identity);
                }
            }
            else if (pr.state != PuzzleManager.PuzzleState.Fixed)                                         //ƨ�ܳ���
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

    void CheckBox(GameObject puzz)                                            //������ �ٿ��� �� ����ó�� ����
    {
        int index = GetIndex(puzz);
        if (index == puzzle.Length) return;
        for (int i = 0; i < puzz.transform.childCount ; i++)
        {
            int positionX = Mathf.RoundToInt(puzz.transform.GetChild(i).position.x);
            int positionY = Mathf.RoundToInt(puzz.transform.GetChild(i).position.y);
            if (positionX >= 0 && positionX < width && positionY >= 0 && positionY < height) //ĵ������ ����
            {
                int positionindex = positionX + positionY * height;
                qd = quad[positionindex].GetComponent<MeshRenderer>().material; //������ ���� ����
                pz = puzz.transform.GetComponent<MeshRenderer>().material;
                if (grid[positionX, positionY] == null || grid[positionX, positionY].name != puzz.transform.GetChild(i).name)     //���� Ʋ�� �ƴϰų� ����� �׸����� �̸��� ��ġ���� ������ false
                {
                    checkpuzz[index] = false;
                    return;
                }
                else if (grid[positionX, positionY].name == puzz.transform.GetChild(i).name)    //������ �̸��� �׸��� ���� ��ġ�ϸ� ���ٲ�
                    qd.color = pz.color;                    //���� ���� ��ġ��� ���带 ���� �������� ���� 
            }
        }
        //Absorb(puzz);
        pr.state = PuzzleManager.PuzzleState.Clear;
        //print(pr.state);
        pr.Clear();
        pr.GetComponent<Rigidbody>().isKinematic = true;
        checkpuzz[index] = true;                     //������ ������ ĵ�۽��� Ʋ���� ��ġ�� ������ true üũ
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

    public void SetPuzzlePosition(int length)                                //11 * 11 �ǿ� ������ ��ǥ �����ϴ� �Լ�
    {
        int k = 0;
        while (k < length)
        {
            int x = Random.Range(1, width);
            int y = Random.Range(1, height);
            int index = x + (height * y);                 //Canvas �� �ڽ� Quad���� �ε��� ����
            if (CheckGrid(k, index)) continue;            //������� ��ġ �ߺ� ����
            photonView.RPC("SetQuadColor", RpcTarget.AllBuffered, x, y, k, index);   //��� PC�� ���� Ʋ ��ġ �����ֱ�
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
                    return true;
            }
        }
        return false;
    }
    [PunRPC]
    void SetQuadColor(int x, int y, int k, int index)               //ĵ���� �ǿ� ����Ʋ ����
    {
        quad[index].SetActive(true);
        puzzle[k].transform.position = quad[index].transform.position + new Vector3(0, 0, 6);         //������ ���� ��ǥ�� ��ġ ��Ű��.
        quad[index].SetActive(false);
        for (int i = 0; i < puzzle[k].transform.childCount; i++)
        {
            int positionX = Mathf.RoundToInt(puzzle[k].transform.GetChild(i).position.x);
            int positionY = Mathf.RoundToInt(puzzle[k].transform.GetChild(i).position.y);
            if (positionX >= 0 && positionX < width && positionY >= 0 && positionY < height)
            {
                int positionindex = positionX + (height * positionY);                      //����� ������ ��ġ�� �κ��� ������
                quad[positionindex].SetActive(true);
                grid[positionX, positionY] = puzzle[k].transform.GetChild(i);
                //qd = quad[positionindex].GetComponent<MeshRenderer>().material; //������ ���� ����
                //pz = puzzle[k].transform.GetComponent<MeshRenderer>().material;
                //qd.color = pz.color;
            }
        }
    }
}
