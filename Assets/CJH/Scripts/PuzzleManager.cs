using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    Transform center;         //���� �߽� ��ġ
    Rigidbody rigid;          //������ ���� ������Ʈ
    BoxCollider box;          //������ �ڽ� �ݶ��̴�
    Vector3 dir;              //���� ���� ����
    float dist;               //ĵ���� �߽������ �Ÿ�
    float [] xyz;             //��ü�� ���� ��
    int[] rotspeed;           //ȸ�� �ӵ�
    public int puzzleIndex;   //���� �ε���
    GameManager gm;           //������ �ε��� ��������
    public bool rink;         //ĵ�������� ���� ����
 
    public enum PuzzleState
    {
        Revolution,           //���� ����
        Catch,                //���� ����
    }

    public PuzzleState state;
    // Start is called before the first frame update
    void Start()
    {
        xyz = new float[3];
        rotspeed = new int[3];
        rigid = GetComponent<Rigidbody>();
        box = GetComponent<BoxCollider>();
        StartCoroutine(ResetGravity());
        GameObject canvasGo = GameObject.Find("Center");
        center = canvasGo.GetComponent<Transform>();
        GameObject gmGo = GameObject.Find("GameManager");
        gm = gmGo.GetComponent<GameManager>();
    }

    // Update is called once per frame
    private void Update()
    {
        if(!rink)           //ĵ������ �پ��ִٸ� �ٸ� ������ ��Ƶ� �������� �ʰ� üũ 
        //CheckPreView();
        switch (state)
        {
            case PuzzleState.Revolution:
                Revolution();
                break;
            case PuzzleState.Catch:
                Catch();
                break;
            default:
                break;
        }

    }

    IEnumerator ResetGravity()           //���� 0.5 �� �� ���߷� ���·� ��ȯ
    {
        yield return new WaitForSeconds(0.5f);
        rigid.useGravity = false;
    }

   void Revolution()    // ĵ���� �߽� �����ϴ� �Լ�
    {
        dist = Vector3.Distance(transform.position, center.transform.position);
        transform.forward = center.transform.position - transform.position;

        if (dist <= 30)
        {
            dir = transform.forward + transform.right;
            rigid.AddForce(dir * 0.005f, ForceMode.Impulse);
        }
        else
        {
            dir = transform.position - center.transform.position;
            rigid.AddForce(-dir * 0.01f, ForceMode.Impulse);
        }
    }

    void Catch()                                     //�ǿ� ����� �� x , y , z ������ 0���� ���� 
    {
        xyz[0] = transform.rotation.x;
        xyz[1] = transform.rotation.y;
        xyz[2] = transform.rotation.z;
        for (int i = 0; i < xyz.Length; i++)
        {
            if (xyz[i] <= 0.008f && xyz[i] >= -0.008f)
                rotspeed[i] = 0;
            else
                rotspeed[i] = -3;
        }
        transform.Rotate(xyz[0] , xyz[1] , xyz[2]);
    }

    public void CheckPreView()
    {
        if (gm.preViewIndex != puzzleIndex)
        {
            state = PuzzleState.Revolution;
            rigid.isKinematic = false;
        }
    }
}
