using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckAnswer_Piller : MonoBehaviour
{
    //�浹 üũ ī��Ʈ
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
            //�������� ������ ����� ���� �����ش�.
            collision_cnt++;
            //print(collision_cnt);
            ga.totalBlockNumber++;
            //����ڰ� ����� �ű涧 �´� ������ ����̸� collision_cnt�� ���Ѵ�

            if (collision_cnt == 10)
            {
                //���� �Ϸῡ ���� ī��Ʈ
                print("���� �Ϸ�Ǿ����ϴ�.");
                //�� ���� ��� ����ó��(10��)�� �Ǹ� UI�� SetActive�ϴ� �Լ��� �����Ų��.
                
            }
            if (ga.totalBlockNumber == 100)
            {
                print("��� �����Դϴ�.");
                //100���� ����� ��� ��ġ�ϰ� �Ǹ� UI�� SetActive�ϴ� �Լ��� �����Ų��.
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //�׸��� ���࿡ ����ڰ� Ŭ���� �߸��ؼ� ���� ����� �ٸ� ������ ���������� ���̳ʽ� �����ش�.
        //���� �浹�ؼ� ������ ��� �̸��� DarkRed���
        if (collision.gameObject.name == BlockName)
        {
            //print("��տ��� �ٸ� �̸��� ����� �����������ϴ�.");
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
//        print("LightRed : �����Դϴ�.");
//    }
//    if (collision.gameObject.name == "DarkRed")
//    {
//        print("DarkRed : �����Դϴ�.");
//    }
//    if (collision.gameObject.name == "Orange")
//    {
//        print("Orange : �����Դϴ�.");
//    }
//    if (collision.gameObject.name == "Yellow")
//    {
//        print("Yellow : �����Դϴ�.");
//    }
//    if (collision.gameObject.name == "Green")
//    {
//        print("Green : �����Դϴ�.");
//    }
//    if (collision.gameObject.name == "Teal")
//    {
//        print("Teal : �����Դϴ�.");
//    }
//    if (collision.gameObject.name == "LightBlue")
//    {
//        print("LightBlue : �����Դϴ�");
//    }
//    if (collision.gameObject.name == "DarkBlue")
//    {
//        print("DarkBlue : �����Դϴ�.");
//    }
//    if (collision.gameObject.name == "Purple")
//    {
//        print("Purple : �����Դϴ�.");
//    }
//    if (collision.gameObject.name == "Black")
//    {
//        print("Black : �����Դϴ�.");
//    }
//}


////����÷��� ���� ����
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
