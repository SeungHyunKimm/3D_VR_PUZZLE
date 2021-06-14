using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PC_AIPlayerControl : MonoBehaviour
{
    public int preViewIndex;                   //선택한 퍼즐의 인덱스
    public PuzzleManager pr;
    public CanvasManager cv;
    public GameObject[] preView;       //퍼즐 프리뷰 배열
    Ray ray;
    RaycastHit hit;
    public float fusionSpeed;                //결합 속도
    EffectSettings es;
    //public EffectSettings shotEffect;   //발사 이펙트
    public GameObject catchEffect;      //Catch 이펙트
    public ParticleSystem fusionEffect; //Fusion 이펙트
    //public GameObject setting;
    GameObject control;
    Vector3 controlpos;

    PuzzleManager[] puzzles;
    int playerPreViewIndex = 0;
    public bool[,] quad;

    int width = 11, height = 11;

    public enum AIPlayerState
    {
        Search,
        Choice,
        PreView,
        Control,
        End
    }

    public AIPlayerState state;

    private void Awake()
    {
        GameObject puz = GameObject.Find("Puzzle");
        puzzles = new PuzzleManager[puz.transform.childCount];

        for (int i = 0; i < puz.transform.childCount; i++)
            puzzles[i] = puz.transform.GetChild(i).GetComponent<PuzzleManager>();

        quad = new bool[width, height];
    }
    void Start()
    {
        GameObject pre = GameObject.Find("PreView");         //프리뷰 담기
        for (int i = 0; i < pre.transform.childCount; i++)
            preView[i] = pre.transform.GetChild(i).gameObject;

        fusionSpeed = 1000;

        control = GameObject.Find("ControlPos");

    }

    // Update is called once per frame
    void Update()
    {
        AIController();
    }

    void AIController()
    {
        switch (state)
        {
            case AIPlayerState.Search:
                SearchQuad();
                break;
            case AIPlayerState.Choice:
                ChoiceClosePuzzle();
                break;
            case AIPlayerState.PreView:
                PreViewSetting();
                break;
            case AIPlayerState.Control:
                Set_Min_Distance();
                break;
        }
    }

    //가까운 퍼즐 선택하기
    void ChoiceClosePuzzle()
    {
        float dist = 1000;
        int index = 0;
        for (int i = 0; i < puzzles.Length/2; i++)
        {
            if (dist >= Vector3.Distance(transform.position, puzzles[i].transform.position) && puzzles[i].state == PuzzleManager.PuzzleState.Revolution)
            {
                dist = Vector3.Distance(transform.position, puzzles[i].transform.position);
                index = i;
            }
        }
        ChoicePuzzle(index);
        PreViewOn();
        pr.transform.position = transform.position + new Vector3(0, 0, 1);
        state = AIPlayerState.PreView;
    }

    int moveX = 1;
    int moveY = 1;
    //캔퍼스 상에서 프리 뷰 위치 잡기
    void SearchQuad()
    {
        if (transform.position.x * transform.position.y < (width - 1) * (height - 1))
            MoveXY();
        else
            state = AIPlayerState.Choice;

        ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.name == "Quad")
            {
                if (!quad[(int)hit.point.x, (int)hit.point.y])
                    quad[(int)hit.point.x, (int)hit.point.y] = true;
            }
        }
    }

    void MoveXY()    //탐색 시작 지점을 (0 , 0)으로 지정
    {
        transform.position += Vector3.up * moveY;
        if (transform.position.y == height || transform.position.y == -1)    //판의 y축 맨 밑 혹은 맨 위일 때 오른쪽으로 한 칸 이동.
        {
            transform.position -= Vector3.up * moveY;
            moveY *= -1;
            transform.position += Vector3.right * moveX;
            if (transform.position.x == width || transform.position.x == -1)
            {
                transform.position -= Vector3.right * moveX;
                moveX *= -1;
            }
        }
    }

    float currtime = 0;
    public float checktime;
    //멈추고 프리 뷰 각도 셋팅
    void PreViewSetting()
    {
        if (pr.state != PuzzleManager.PuzzleState.Catch)
        {
            preView[preViewIndex].SetActive(false);
            state = AIPlayerState.Choice;
        }
        currtime += Time.deltaTime;
        if (checktime < currtime)
            currtime = 0;
        else
            return;
        MoveXY();
        ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Puzzle"))
            {
                PuzzleManager pz = hit.transform.GetComponent<PuzzleManager>();
                if (pz.state == PuzzleManager.PuzzleState.Fixed)
                {
                    pz.transform.position = new Vector3(0, 20, 0);
                    pz.state = PuzzleManager.PuzzleState.Revolution;
                    return;
                }
            }
        }
        PreViewStay((int)transform.position.x, (int)transform.position.y);

        for (int i = 0; i < preView[preViewIndex].transform.childCount; i++)
        {
            int x = Mathf.RoundToInt(preView[preViewIndex].transform.GetChild(i).position.x);
            int y = Mathf.RoundToInt(preView[preViewIndex].transform.GetChild(i).position.y);
            if (x >= 0 && x < width && y >= 0 && y < height)
            {
                if (!quad[x, y])    //캔퍼스 상에서 쿼드가 아니며 그 퍼즐을 한 번 두었던 곳이라면 
                    return;                                //리턴
            }
        }
        state = AIPlayerState.Control;
    }

    //퍼즐 위치에 따른 합쳐지는 최소 거리 조정
    void Set_Min_Distance()
    {
        if (pr == null) return;
        FusionUp();
        state = AIPlayerState.Choice;
    }

    void Rotate()
    {
        preView[preViewIndex].transform.Rotate(0, 0, 90);
    }

    void PreViewOn()
    {
        //왼손
        preView[preViewIndex].SetActive(true);
    }

    void PreViewStay(int x, int y)
    {
        preView[preViewIndex].transform.position = new Vector3(x, y);
    }
    void PreViewClose()
    {
        preView[preViewIndex].SetActive(false);
    }

    void PuzzleMove(Vector3 dir)
    {
        pr.Move(dir, fusionSpeed);
        pr.SetPreViewXYZ(preView[preViewIndex]);
        pr = null;
    }

    void FusionUp()
    {
        if (pr.state == PuzzleManager.PuzzleState.Catch || pr.state == PuzzleManager.PuzzleState.Control)
        {
            pr.transform.position = preView[preViewIndex].transform.position + new Vector3(0, 0, -1);
            pr.transform.rotation = Quaternion.identity;
            pr.state = PuzzleManager.PuzzleState.Fusion;
            //Vector3 dir = preView[preViewIndex].transform.position - pr.transform.position;
            PuzzleMove(Vector3.forward);
        }
        PreViewClose();
    }
    void ControlDown()
    {
        pr.state = PuzzleManager.PuzzleState.Control;
        controlpos = transform.position;
        control.transform.position = controlpos;
    }

    Vector3 controldir;
    void Control(Vector3 controldir)
    {
        pr.controlpos = controldir;
        catchEffect.SetActive(true);
        catchEffect.transform.position = pr.transform.position;
    }
    void ControlUp()
    {
        pr.GetComponent<Rigidbody>().isKinematic = true;
        pr.state = PuzzleManager.PuzzleState.Catch;
        catchEffect.SetActive(false);
        fusionEffect.transform.position = pr.transform.position;
        fusionEffect.Play();
    }
    void Catch(string name)
    {
        for (int i = 0; i < preView.Length; i++)
        {
            if (preView[i].name == name)     //프리뷰 인덱스 저장 및 Catch상태로 변환 
            {
                ChoicePuzzle(i);
                break;
            }
        }
    }

    void ChoicePuzzle(int i)
    {
        if (puzzles[i].state == PuzzleManager.PuzzleState.Clear) return;
        if (preView[preViewIndex].name != puzzles[i].name && pr != null)
        {
            if (pr.state == PuzzleManager.PuzzleState.Catch || pr.state == PuzzleManager.PuzzleState.Control)
            {
                PreViewClose();
                pr.state = PuzzleManager.PuzzleState.Revolution;
            }
        }
        preViewIndex = i;
        pr = puzzles[i].GetComponent<PuzzleManager>();
        pr.state = PuzzleManager.PuzzleState.Catch;
        if (preViewIndex / (puzzles.Length / 2) == playerPreViewIndex)  //자신의 판에 맞는 퍼즐일 때만 체크 퍼즐 호출
            cv.CatchToCheckBox(preViewIndex % (puzzles.Length / 2));
        puzzles[i].GetComponent<Rigidbody>().isKinematic = true; //멈추게 만들기
    }
}
