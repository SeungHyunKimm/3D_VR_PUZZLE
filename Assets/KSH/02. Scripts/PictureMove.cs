using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PictureMove : MonoBehaviour
{
    public GameObject[] Pics;
    public Quaternion rot;
    public Transform rotss;
    void Start()
    {
        MovePic();
        //�ݺ������� ����(6��°����), ���� X ���� ����(ex - 90�� * 1 or 2 or 3)

        //eulerAngles �Լ��� ����ؼ� * 3
    }

    void Update()
    {
        //���࿡ ������ ������ ����ٸ�(�׸��� ���� ��ġ�� �� ����ٸ�)
        StartCoroutine(AnswerClear());

    }
    void MovePic()
    {
        //6���� ������ ������ 1���� 90, 180, 270 �� ���� �߿� �ϳ��� ���� �ٲ㼭 �����ϰ� �ʹ�.
        //3���� ������ ���� �迭�� ���� �� �ִ�.
        
        Pics[0].transform.eulerAngles = new Vector3(0, 0, 90);
        Pics[1].transform.eulerAngles = new Vector3(0, 0, 270);
        Pics[2].transform.eulerAngles = new Vector3(0, 0, 180);
        Pics[3].transform.eulerAngles = new Vector3(0, 0, 90);
        Pics[4].transform.eulerAngles = new Vector3(0, 0, 270);
        Pics[5].transform.eulerAngles = new Vector3(0, 0, 180);


    }

    IEnumerator AnswerClear()
    {
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < Pics.Length; i++)
        {
            if (Pics[i].transform.eulerAngles == new Vector3(0, 0, 0))
            {
                print("���� �Ϸ�");

            }
        }
    }
}
