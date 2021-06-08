using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    
    public Transform[] pictures;
    [SerializeField]
    public GameObject winText;
    public static bool youWin;
    void Start()
    {
        //winText.SetActive(false)
        youWin = false;
        MovePic();
    }
    void MovePic()
    {
        //6개의 사진의 각도를 1번만 90, 180, 270 세 가지 중에 하나로 각각 바꿔서 적용하고 싶다.
        //3개의 각도의 값을 배열로 넣을 수 있다.

        pictures[0].transform.eulerAngles = new Vector3(0, 0, 90);
        pictures[1].transform.eulerAngles = new Vector3(0, 0, 270);
        pictures[2].transform.eulerAngles = new Vector3(0, 0, 180);
        pictures[3].transform.eulerAngles = new Vector3(0, 0, 90);
        pictures[4].transform.eulerAngles = new Vector3(0, 0, 270);
        pictures[5].transform.eulerAngles = new Vector3(0, 0, 180);
    }
    void Update()
    {
        if (pictures[0].transform.eulerAngles == new Vector3(0, 0, 0) &&  
            pictures[1].transform.eulerAngles == new Vector3(0, 0, 0) &&
            pictures[2].transform.eulerAngles == new Vector3(0, 0, 0) &&  
            pictures[3].transform.eulerAngles == new Vector3(0, 0, 0) &&
            pictures[4].transform.eulerAngles == new Vector3(0, 0, 0) &&  
            pictures[5].transform.eulerAngles == new Vector3(0, 0, 0))
        {
            print("모든 사진 완료");
            youWin = true;
            winText.SetActive(true);
        }
    }
}
