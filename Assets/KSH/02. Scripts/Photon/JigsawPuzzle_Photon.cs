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
        //원위치로 돌아간 것을 참/거짓 판단하여 모든 퍼즐들의 위치가 참일 경우
        //for (int i = 0; i < pss.Length; i++)
        //{
        //    if (pss[i].AnswerJigsawPuzzle_Photon() == false)
        //    {
        //        return false;
        //    }
        //}

        //퍼즐들의 위치를
        if (answerCnt == transform.childCount)
        {
            return true;
        }
        else return false;


        //미리 만들어둔 Clear Ui를 실행시키고
        //트리거로 블록의 움직임을 막는다.(트리거의 움직임을 알 수 있는 bool형식)
     
        
    }
}
