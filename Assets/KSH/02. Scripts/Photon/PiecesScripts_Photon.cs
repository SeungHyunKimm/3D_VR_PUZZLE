using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PiecesScripts_Photon : MonoBehaviourPunCallbacks
{
    Vector3 rightposition;
    public bool InRightPosition;
    public bool Selected;
    public bool PuzzleisRight;
    public int puzIndex;
    void Start()
    {
        RPC_PuzzlePos();
    }
    [PunRPC]
    void RPC_PuzzlePos()
    {
        rightposition = transform.position;
        //방장의 퍼즐을
        if (PhotonNetwork.IsMasterClient)
        {
            Vector3 sendpos;
            PuzzleisRight = false;
            sendpos = new Vector3(Random.Range(11, 20), Random.Range(9, 1.5f), 0);

            //위의 퍼즐들의 위치를 RPC함수로 각각 뿌려주고 싶다.
            photonView.RPC("RPC_ShufflePuzzle", RpcTarget.AllBuffered, sendpos);
        }
    }

    public void SetPuzIndex(int i)
    {
        puzIndex = i;
    }

    [PunRPC]
    void RPC_ShufflePuzzle(Vector3 getpos)
    {
        transform.position = getpos;
        //dir = new Vector3(Random.Range(11, 20), Random.Range(9, 1.5f), 0);
    }




    //void Update()
    //{

    //    if (Vector3.Distance(transform.position, rightposition) < 1f)
    //    {
    //        photonView.RPC("RPC_CorrectPuzzle", RpcTarget.All, rightposition);
    //    }
    //}

    public bool CheckCount()
    {
        if (Checkdist() < 1f)
        {
            photonView.RPC("RPC_CorrectPuzzle", RpcTarget.All, rightposition);
            return true;
        }
        return false;
    }

    public float Checkdist()
    {
        float dist = Vector3.Distance(transform.position, rightposition);
        return dist;
    }

    [PunRPC]
    void RPC_CorrectPuzzle(Vector3 rt)
    {

        transform.position = rt;

    }

    public bool AnswerJigsawPuzzle_Photon()
    {
        if (transform.position == rightposition)
            PuzzleisRight = true;

        PuzzleisRight = false;
        return transform.position ==
        rightposition;
    }
}
