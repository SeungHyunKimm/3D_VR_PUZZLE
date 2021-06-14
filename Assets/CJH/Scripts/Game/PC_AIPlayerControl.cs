using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PC_AIPlayerControl : MonoBehaviour
{
    public int preViewIndex;                   //������ ������ �ε���
    public PuzzleManager pr;
    public CanvasManager cv;
    public GameObject[] preView;       //���� ������ �迭
    Ray ray;
    RaycastHit hit;
    public float fusionSpeed;                //���� �ӵ�
    EffectSettings es;
    //public EffectSettings shotEffect;   //�߻� ����Ʈ
    public GameObject catchEffect;      //Catch ����Ʈ
    public ParticleSystem fusionEffect; //Fusion ����Ʈ
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
        GameObject pre = GameObject.Find("PreView");         //������ ���
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

    //����� ���� �����ϱ�
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
    //ĵ�۽� �󿡼� ���� �� ��ġ ���
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

    void MoveXY()    //Ž�� ���� ������ (0 , 0)���� ����
    {
        transform.position += Vector3.up * moveY;
        if (transform.position.y == height || transform.position.y == -1)    //���� y�� �� �� Ȥ�� �� ���� �� ���������� �� ĭ �̵�.
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
    //���߰� ���� �� ���� ����
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
                if (!quad[x, y])    //ĵ�۽� �󿡼� ���尡 �ƴϸ� �� ������ �� �� �ξ��� ���̶�� 
                    return;                                //����
            }
        }
        state = AIPlayerState.Control;
    }

    //���� ��ġ�� ���� �������� �ּ� �Ÿ� ����
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
        //�޼�
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
            if (preView[i].name == name)     //������ �ε��� ���� �� Catch���·� ��ȯ 
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
        if (preViewIndex / (puzzles.Length / 2) == playerPreViewIndex)  //�ڽ��� �ǿ� �´� ������ ���� üũ ���� ȣ��
            cv.CatchToCheckBox(preViewIndex % (puzzles.Length / 2));
        puzzles[i].GetComponent<Rigidbody>().isKinematic = true; //���߰� �����
    }
}
