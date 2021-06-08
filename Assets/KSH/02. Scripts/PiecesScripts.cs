using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiecesScripts : MonoBehaviour
{
    Vector3 rightposition;
    public bool InRightPosition;
    public bool Selected;
    public bool PuzzleisRight;
    void Start()
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

    public bool AnswerJigsawPuzzle()
    {
        if (transform.position == rightposition)
            PuzzleisRight = true;
        else
            PuzzleisRight = false;
        return transform.position == rightposition;
    }
}
