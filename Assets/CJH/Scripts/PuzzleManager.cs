using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager instance;
    Transform center;         //���� �߽� ��ġ
    Rigidbody rigid;          //������ ���� ������Ʈ
    Vector3 dir;              //���� ���� ����
    float dist;               //ĵ���� �߽������ �Ÿ�
    float [] xyz;             //��ü�� ���� ��
    int[] rotspeed;           //ȸ�� �ӵ�

    public enum PuzzleState
    {
        Revolution,           //���� ����
        Catch,                //���� ����
    }

    public PuzzleState state;
    // Start is called before the first frame update

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        xyz = new float[3];
        rotspeed = new int[3];
        rigid = GetComponent<Rigidbody>();
        StartCoroutine(ResetGravity());
        GameObject canvasGo = GameObject.Find("Center");
        center = canvasGo.GetComponent<Transform>();
    }

    // Update is called once per frame
    private void Update()
    {
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

        print(transform.rotation.x);
    }

    IEnumerator ResetGravity()           //���� 1�� �� ���߷� ���·� ��ȯ
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
                rotspeed[i] = -1;
        }
        transform.Rotate(xyz[0] , xyz[1] , xyz[2]);
    }
}
