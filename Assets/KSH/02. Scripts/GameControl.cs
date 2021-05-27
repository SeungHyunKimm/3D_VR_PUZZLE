using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    [SerializeField]
    Transform[] pictures;
    [SerializeField]
    GameObject winText;
    public static bool youWin;
    void Start()
    {
        winText.SetActive(false);
        youWin = false;
    }

    void Update()
    {
        for(int i =0; i < pictures.Length; i++)
        {
            
            if(pictures[i].rotation.z == 0)
            {
                youWin = true;
                winText.SetActive(true);
            }
        }
    }
}
