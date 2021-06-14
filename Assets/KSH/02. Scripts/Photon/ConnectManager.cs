using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;


public class ConnectManager : MonoBehaviourPunCallbacks

{
    public string gameVersion = "1";
    public InputField nickname;


    private void Start()
    {
        
       Connect_ModeD();
        
    }

    public void Connect_ModeD()
    {

        //만약 nickname의 길이가 0이면
        //if (nickname.text.Length == 0)
        //{
        //    //접속을 차단한다.
        //    Debug.LogWarning("아이디를 입력하세요.");
        //}

        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 60;
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();

    }

    public override void OnConnected()
    {
        print("OnConnected");
    }
    public override void OnConnectedToMaster()
    {
        print("OnConnectedToMaster");
        //PhotonNetwork.NickName = nickname.text;
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }
    public override void OnJoinedLobby()
    {
        print("OnJoinedLobby");
        PhotonNetwork.LoadLevel("02. Photon_Main_LobbyScene");
        
    }

}
