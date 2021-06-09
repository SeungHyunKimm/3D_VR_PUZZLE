using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Photon_ConnectManager : MonoBehaviourPunCallbacks
{
    //게임버전
    public string gameVersion = "1";
    public void ModeD_Connection()
    {
        //1. Game Version을 설정한다.
        PhotonNetwork.GameVersion = gameVersion;
        //2. 씬을 동기화할건지 여부
        PhotonNetwork.AutomaticallySyncScene = true;
        //3. 접속
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnected()
    {
        base.OnConnected();
    }
}
