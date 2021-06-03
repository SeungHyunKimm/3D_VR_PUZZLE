using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckAnswer_Piller : MonoBehaviour
{

    public GameObject[] LightRed;
    public GameObject[] DarkRed;
    public GameObject[] Orange;
    public GameObject[] Yellow;
    public GameObject[] Green;
    public GameObject[] Teal;
    public GameObject[] LightBlue;
    public GameObject[] DarkBlue;
    public GameObject[] Purple;
    public GameObject[] Black;
    //블럭 개수
    int block_number = 10;
    //카운트
    int cnt = 0;
 


    //기둥과 충돌되는 물체들 전부의 이름이 같다면
    //기둥과 충돌되는 블럭의 이름을 전부다 확인해보고
    //만약에 
    //정답 처리 메세지를 출력해보자
    void Start()
    {
        
        

    }

    void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject)
        {

            print(collision.gameObject.name);
            
        }

    }















    //void trash()
    //{
    //    if (collision.gameObject.name == "LightRed")
    //    {
    //        print("LightRed : 정답입니다.");
    //    }
    //    if (collision.gameObject.name == "DarkRed")
    //    {
    //        print("DarkRed : 정답입니다.");
    //    }
    //    if (collision.gameObject.name == "Orange")
    //    {
    //        print("Orange : 정답입니다.");
    //    }
    //    if (collision.gameObject.name == "Yellow")
    //    {
    //        print("Yellow : 정답입니다.");
    //    }
    //    if (collision.gameObject.name == "Green")
    //    {
    //        print("Green : 정답입니다.");
    //    }
    //    if (collision.gameObject.name == "Teal")
    //    {
    //        print("Teal : 정답입니다.");
    //    }
    //    if (collision.gameObject.name == "LightBlue")
    //    {
    //        print("LightBlue : 정답입니다");
    //    }
    //    if (collision.gameObject.name == "DarkBlue")
    //    {
    //        print("DarkBlue : 정답입니다.");
    //    }
    //    if (collision.gameObject.name == "Purple")
    //    {
    //        print("Purple : 정답입니다.");
    //    }
    //    if (collision.gameObject.name == "Black")
    //    {
    //        print("Black : 정답입니다.");
    //    }
    //}


    ////블록컬러를 담을 변수
    //public BlockColor blocks;


    //enum BlockColor
    //{
    //    LightRed,
    //    DarkRed,
    //    Orange,
    //    Yellow,
    //    Green,
    //    Teal,
    //    LightBlue,
    //    DarkBlue,
    //    Purple,
    //    Black
    //}







    //    switch (blocks)
    //{


    //    case BlockColor.LightRed:
    //        break;
    //    case BlockColor.DarkRed:
    //        break;
    //    case BlockColor.Orange:
    //        break;
    //    case BlockColor.Yellow:
    //        break;
    //    case BlockColor.Green:
    //        break;
    //    case BlockColor.Teal:
    //        break;
    //    case BlockColor.LightBlue:
    //        break;
    //    case BlockColor.DarkBlue:
    //        break;
    //    case BlockColor.Purple:
    //        break;
    //    case BlockColor.Black:
    //        break;
    //}
}