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
    //�� ����
    int block_number = 10;
    //ī��Ʈ
    int cnt = 0;
 


    //��հ� �浹�Ǵ� ��ü�� ������ �̸��� ���ٸ�
    //��հ� �浹�Ǵ� ���� �̸��� ���δ� Ȯ���غ���
    //���࿡ 
    //���� ó�� �޼����� ����غ���
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