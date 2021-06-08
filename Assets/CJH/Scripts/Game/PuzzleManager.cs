using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class PuzzleManager : MonoBehaviourPun//, IPunObservable
{
    Vector3 center;         //���� �߽� ��ġ
    Rigidbody rigid;          //������ ���� ������Ʈ
    BoxCollider box;          //������ �ڽ� �ݶ��̴�
    Vector3 dir;              //���� ���� ����
    float dist;               //ĵ���� �߽������ �Ÿ�
    PlayerControl gm;           //������ �ε��� ��������
    public Vector3 controlpos;//��Ʈ�� ���� ��ġ ��
    float rvSpeed;            //���� ���ǵ�
    float[] xyz;               //��ü�� ���� ��
    float pre_z;
    Quaternion value;
    float x, y, z;

    public Vector3 puzzlerot;

    public enum PuzzleState
    {
        Revolution,           //���� ����
        Catch,                //���� ����
        Control,              //���� ����
        Fusion,               //���� ����
        Fixed                 //���� ����
    }

    enum PuzzleRot
    {
        Set,                   //���� ���� ����
        Change                 //���� ���� ������ ����
    }

    public PuzzleState state;
    PuzzleRot state2;
    // Start is called before the first frame update
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        box = GetComponent<BoxCollider>();
    }

    void Start()
    {
        xyz = new float[3];
        StartCoroutine(ResetGravity());
        center = new Vector3(5, 5, 0.5f);
        GameObject gmGo = GameObject.Find("GameManager");
        gm = gmGo.GetComponent<PlayerControl>();
        rigid.isKinematic = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PuzzleOperation();
        }
    }
    void PuzzleOperation()
    {
        switch (state)
        {
            case PuzzleState.Revolution:
                Revolution();
                break;
            case PuzzleState.Control:
                Control();
                break;
            case PuzzleState.Fusion:
                Fusion();
                break;
            case PuzzleState.Catch:
                Catch();
                break;
        }
    }

    IEnumerator ResetGravity()           //���� 0.5 �� �� ���߷� ���·� ��ȯ
    {
        yield return new WaitForSeconds(1);
        rigid.useGravity = false;
    }

    void Revolution()    // ĵ���� �߽� �����ϴ� �Լ�
    {
        rigid.isKinematic = false;
        dist = Vector3.Distance(transform.position, center);

        //transform.forward = center - transform.position;
        if (dist <= 30)
        {
            dir = transform.forward + transform.right + transform.up;
            rvSpeed = 2;
        }
        else
        {
            dir = center - transform.position;
            rvSpeed = 0.5f;
        }

        rigid.AddForce(dir * rvSpeed * Time.deltaTime, ForceMode.Impulse);
    }
    int rotz;
    void Fusion()                                     //�ǿ� ����� �� x , y , z ������ 0���� ���� 
    {
        int mix = (int)transform.eulerAngles.x + (int)transform.eulerAngles.y + (int)transform.eulerAngles.z;
        if (mix == 0)
            state2 = PuzzleRot.Change;
        if (state2 == PuzzleRot.Change && (int)transform.eulerAngles.z == pre_z) return;
        switch (state2)
        {
            case PuzzleRot.Set:
                xyz[0] = transform.rotation.x;
                xyz[1] = transform.rotation.y;
                xyz[2] = transform.rotation.z;
                transform.Rotate(xyz[0] * 3, xyz[1] * 3, xyz[2] * 3);
                break;
            case PuzzleRot.Change:
                transform.Rotate(0, 0, rotz);
                break;
        }
    }
    public void SetPreViewXYZ(GameObject prRot)
    {
        pre_z = prRot.transform.eulerAngles.z;
    }
    int xz = 0;
    void Catch()
    {
        transform.Rotate(1, 0, 0);
        state2 = PuzzleRot.Set;
        rotz = 1;
    }

    public void Move(Vector3 dir, float Speed)
    {
        rigid.isKinematic = false;
        rigid.AddForce(dir * Speed * Time.deltaTime, ForceMode.Impulse);
    }


    void Control()
    {
        Move(controlpos, 3);
        Catch();
    }

    private void OnCollisionEnter(Collision collision)
    {
        PuzzleManager pr = collision.transform.GetComponent<PuzzleManager>();
        if (collision.gameObject.layer == LayerMask.NameToLayer("Puzzle") && state != PuzzleState.Fixed)
        {
            if (pr.state != PuzzleState.Fixed)
                state = PuzzleState.Revolution;
        }
    }
}
