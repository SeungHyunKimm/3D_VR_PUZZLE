using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Rigidbody rigid;                    //hit ���� ������Ʈ�� rigid ������Ʈ
    public GameObject [] preView;       //���� ������ �迭
    int preViewIndex;                   //������ ������ �ε���
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        //if (Input.GetMouseButtonDown(0))
        //{
        //    if(Physics.Raycast(ray , out hit))
        //    {
        //        rigid = hit.transform.GetComponent<Rigidbody>();
        //        Vector3 dir = Camera.main.transform.position - hit.transform.position;
        //        rigid.AddForce(dir * 0.2f, ForceMode.Impulse);

        //    }
        //}

        //if (Input.GetMouseButtonDown(1))
        //{
        //    if(Physics.Raycast(ray , out hit))
        //    {
        //        rigid = hit.transform.GetComponent<Rigidbody>();
        //        hit.transform.position = transform.position;
        //        rigid.isKinematic = true;
        //    }
        //}
        if(Physics.Raycast(ray , out hit))
        {
            if (Input.GetMouseButtonDown(0))                    //���������� Ŭ�� �� ���߰� 
            {                                 
                for (int i = 0; i < preView.Length; i++)
                {
                    if (preView[i].name == hit.transform.gameObject.name) //������ �ε��� ���� �� Catch���·� ��ȯ 
                    {
                        rigid = hit.transform.GetComponent<Rigidbody>();
                        PuzzleManager.instance.state = PuzzleManager.PuzzleState.Catch; //Catch���� ��ȯ
                        rigid.isKinematic = true;                        //���� ȿ�� ���� ���·� ��ȯ
                        preViewIndex = i;
                        break;
                    }
                }
            }
            else if (Input.GetKey(KeyCode.Space))               //���� ��ü�� �������
            {
                rigid.isKinematic = false;
                Vector3 dir = Camera.main.transform.position - hit.transform.position;
                rigid.AddForce(dir * 0.05f, ForceMode.Impulse);
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                PuzzleManager.instance.state = PuzzleManager.PuzzleState.Revolution;
            }
            
            
            if (Input.GetMouseButton(1))                //�޼�
            {
                if (hit.transform.gameObject.name == "Canvas")  //������ ���� ��ġ ���� ĵ���� ����
                {
                    preView[preViewIndex].SetActive(true);

                    int x = (int)hit.point.x;
                    int y = (int)hit.point.y;

                    preView[preViewIndex].transform.position = new Vector2(x, y);
                }
            }
            else if (Input.GetMouseButtonUp(1))                //���� �� ��ġ�� ��ü ������ �� ������ ��Ȱ��ȭ
            {
                PuzzleManager.instance.state = PuzzleManager.PuzzleState.Catch;
                rigid.isKinematic = false;                     //����� �� true �����̹Ƿ� ��ȯ ������.
                Vector3 dir = preView[preViewIndex].transform.position - rigid.transform.position;
                
                rigid.AddForce(dir * 1, ForceMode.Impulse);
                preView[preViewIndex].SetActive(false);
            }
        }
    }
}
