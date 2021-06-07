using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckAnswer_Piller : MonoBehaviour
{
    //충돌 체크 카운트
    int collision_cnt = 1;
    BlockManager ga;

    public string BlockName;


    void Start()
    {

        GameObject bk = GameObject.Find("BlockManager");
        ga = bk.GetComponent<BlockManager>();

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == BlockName)
        {
            //랜덤으로 들어오는 블록의 갯수 세어준다.
            collision_cnt++;
            //print(collision_cnt);
            ga.totalBlockNumber++;
            //사용자가 블록을 옮길때 맞는 색상의 블록이면 collision_cnt에 더한다

            if (collision_cnt == 10)
            {
                //한줄 완료에 대한 카운트
                print("한줄 완료되었습니다.");
                //각 색상별 블록 정답처리(10개)가 되면 UI를 SetActive하는 함수를 실행시킨다.
                
            }
            if (ga.totalBlockNumber == 100)
            {
                print("모두 정답입니다.");
                //100개의 블록이 모두 위치하게 되면 UI를 SetActive하는 함수를 실행시킨다.
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //그리고 만약에 사용자가 클릭을 잘못해서 맞은 블록이 다른 곳으로 빠져나가면 마이너스 시켜준다.
        //만약 충돌해서 나가는 블록 이름이 DarkRed라면
        if (collision.gameObject.name == BlockName)
        {
            //print("기둥에서 다른 이름의 블록이 빠져나갔습니다.");
            collision_cnt--;
            //print(collision_cnt);
            ga.totalBlockNumber--;
        }
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
