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
        //반복문으로 묻고(6번째까지), 각도 X 랜덤 범위(ex - 90도 * 1 or 2 or 3)

        //eulerAngles 함수를 사용해서 * 3
    }

    void Update()
    {
        //만약에 퍼즐의 정답을 맞췄다면(그림의 각도 위치를 다 맞췄다면)
        StartCoroutine(AnswerClear());

    }
    void MovePic()
    {
        //6개의 사진의 각도를 1번만 90, 180, 270 세 가지 중에 하나로 각각 바꿔서 적용하고 싶다.
        //3개의 각도의 값을 배열로 넣을 수 있다.
        
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
                print("정답 완료");

            }
        }
    }
}
