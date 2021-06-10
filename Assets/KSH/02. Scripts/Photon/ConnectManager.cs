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



    public void Connect_ModeD()
    {
        //만약 nickname의 길이가 0이면
        //if (nickname.text.Length == 0)
        //{
        //    //접속을 차단한다.
        //    Debug.LogWarning("아이디를 입력하세요.");
        //}

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
        //CreateRoom();
    }
    public override void OnCreatedRoom()
    {
        print("방 생성 성공");
    }

    //방 접속 성공
    public override void OnJoinedRoom()
    {
        print("방 접속 성공");
        print(PhotonNetwork.CurrentRoom.Name);
    }

    public void CreateRoom()
    {
        RoomOptions roomOption = new RoomOptions();
        roomOption.MaxPlayers = 2;
        PhotonNetwork.JoinOrCreateRoom("김승현", roomOption, TypedLobby.Default);
    }

    public void JoinRoom()
    {
        print("JoinRoom is completed");
        print(PhotonNetwork.CurrentRoom.Name);
        PhotonNetwork.LoadLevel("KSH_PuzzleMode_D_Photon");
    }





}
