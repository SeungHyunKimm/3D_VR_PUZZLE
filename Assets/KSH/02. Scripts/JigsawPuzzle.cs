using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JigsawPuzzle : MonoBehaviour
{

    PiecesScripts[] pss;
    public GameObject ClearUI;
    void Start()
    {

        GameObject ps = GameObject.Find("JigsawPuzzle");
        pss = new PiecesScripts[ps.transform.childCount];
        for (int i = 0; i < ps.transform.childCount; i++)
        {
            pss[i] = ps.transform.GetChild(i).GetComponent<PiecesScripts>();
            print(ps.name + "    " + ps.transform.childCount + "   " + ps.transform.GetChild(i).name);
        }
    }


    public void DragSign()
    {
        print("Drag");
        //����ġ�� ���ư� ���� ��/���� �Ǵ��Ͽ� ��� ������� ��ġ�� ���� ���
        for (int i = 0; i < pss.Length; i++)
        {
            if (pss[i].AnswerJigsawPuzzle() == false)
            {
                return;
            }
        }
        //�̸� ������ Clear Ui�� �����Ű��
        //Ʈ���ŷ� ����� �������� ���´�.(Ʈ������ �������� �� �� �ִ� bool����)
        ClearUI.SetActive(true);
        
    }
}
