using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Photon_RoomInformationButton : MonoBehaviour
{
    //정보를 보여줄 텍스트
    public Text info;
    public void SetInformation(string roomName, int currentPlayer, int MaxPlayer)
    {
        //방제목(현재인원/최대인원)
        info.text = roomName + " (" + currentPlayer + " / " + MaxPlayer + ") ";
    }
}
