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
    void Start()
    {
        RPC_RamdomPuzzlePos();
    }

    [PunRPC]
    void RPC_RamdomPuzzlePos()
    {

        PuzzleisRight = false;
        rightposition = transform.position;
        transform.position = new Vector3(Random.Range(11, 20), Random.Range(9, 1.5f));

    }

    void Update()
    {
        if (Vector3.Distance(transform.position, rightposition) < 1f)
        {
            transform.position = rightposition;
        }
    }

    public bool AnswerJigsawPuzzle_Photon()
    {
        if (transform.position == rightposition)
            PuzzleisRight = true;
        else
            PuzzleisRight = false;
        return transform.position == rightposition;
    }
}
