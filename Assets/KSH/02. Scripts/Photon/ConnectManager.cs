using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ConnectManager : MonoBehaviourPunCallbacks
{
    public string gameVersion = "1";
    void Start()
    {
        



    }

    void Update()
    {
        


    }

    public void Connect_ModeD()
    {
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
    }
    public override void OnJoinedLobby()
    {
        print("OnJoinedLobby");
        PhotonNetwork.LoadLevel("Photon_Main_LobbyScene");
        CreateRoom();
    }

    public void CreateRoom()
    {
        RoomOptions roomOption = new RoomOptions();
        roomOption.MaxPlayers = 2;
        PhotonNetwork.JoinOrCreateRoom("πÊ¿Ã∏ß", roomOption, TypedLobby.Default);
    }

    public void JoinRoom()
    {
        print("JoinRoom is completed");
        print(PhotonNetwork.CurrentRoom.Name);
        PhotonNetwork.LoadLevel("Photon_Main_InGameScene");
    }
}
