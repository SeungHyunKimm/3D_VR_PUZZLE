using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class JigsawPuzzle_Photon : MonoBehaviourPun
{

    PiecesScripts_Photon [] pss;
    
    void Start()
    {

        GameObject ps = GameObject.Find("JigsawPuzzles");
        pss = new PiecesScripts_Photon[ps.transform.childCount];
        for (int i = 0; i < ps.transform.childCount; i++)
        {
            pss[i] = ps.transform.GetChild(i).GetComponent<PiecesScripts_Photon>();
            //print(ps.name + "    " + ps.transform.childCount + "   " + ps.transform.GetChild(i).name);
        }
    }

    void Update()
    {




    }
    public int answerCnt = 0;
    public bool DragSign_Photon()
    {
        //print("Drag");
        //����ġ�� ���ư� ���� ��/���� �Ǵ��Ͽ� ��� ������� ��ġ�� ���� ���
        //for (int i = 0; i < pss.Length; i++)
        //{
        //    if (pss[i].AnswerJigsawPuzzle_Photon() == false)
        //    {
        //        return false;
        //    }
        //}

        //������� ��ġ��
        if (answerCnt == transform.childCount)
        {
            return true;
        }
        else return false;


        //�̸� ������ Clear Ui�� �����Ű��
        //Ʈ���ŷ� ����� �������� ���´�.(Ʈ������ �������� �� �� �ִ� bool����)
     
        
    }
}
