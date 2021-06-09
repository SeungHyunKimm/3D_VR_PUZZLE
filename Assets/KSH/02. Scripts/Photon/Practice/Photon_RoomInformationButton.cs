using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Photon_RoomInformationButton : MonoBehaviour
{
    //������ ������ �ؽ�Ʈ
    public Text info;
    public void SetInformation(string roomName, int currentPlayer, int MaxPlayer)
    {
        //������(�����ο�/�ִ��ο�)
        info.text = roomName + " (" + currentPlayer + " / " + MaxPlayer + ") ";
    }
}
