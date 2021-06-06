using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    
    public Transform[] pictures;
    [SerializeField]
    GameObject winText;
    public static bool youWin;
    void Start()
    {
        //winText.SetActive(false)
        youWin = false;
        MovePic();
    }
    void MovePic()
    {
        //6���� ������ ������ 1���� 90, 180, 270 �� ���� �߿� �ϳ��� ���� �ٲ㼭 �����ϰ� �ʹ�.
        //3���� ������ ���� �迭�� ���� �� �ִ�.

        pictures[0].transform.eulerAngles = new Vector3(0, 0, 90);
        pictures[1].transform.eulerAngles = new Vector3(0, 0, 270);
        pictures[2].transform.eulerAngles = new Vector3(0, 0, 180);
        pictures[3].transform.eulerAngles = new Vector3(0, 0, 90);
        pictures[4].transform.eulerAngles = new Vector3(0, 0, 270);
        pictures[5].transform.eulerAngles = new Vector3(0, 0, 180);
    }
    void Update()
    {
        if (pictures[0].rotation.z == 0 &&
            pictures[1].rotation.z == 0 &&
            pictures[2].rotation.z == 0 &&
            pictures[3].rotation.z == 0 &&
            pictures[4].rotation.z == 0 &&
            pictures[5].rotation.z == 0
            )
        {
            youWin = true;
            winText.SetActive(true);
        }
    }
}
