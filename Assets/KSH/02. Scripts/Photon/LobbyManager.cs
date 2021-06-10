using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{

    void Start()
    {

        
        CreateRoom();



    }

    void Update()
    {


        
    }
    public void CreateRoom()
    {

        RoomOptions roomOption = new RoomOptions();
        roomOption.MaxPlayers = 2;
        PhotonNetwork.JoinOrCreateRoom("�����", roomOption, TypedLobby.Default);
        print("�� ���� ����!");
        //PhotonNetwork.JoinRandomRoom();
    }
    public override void OnCreatedRoom()
    {

        print("�� ���� ����");

    }

    public void JoinRoom()
    {

        print("JoinRoom is completed");
        print(PhotonNetwork.CurrentRoom.Name);
        PhotonNetwork.JoinRoom("�����");
        
    }
    public override void OnJoinedRoom()
    {
        print("�� ���� ����");
        print(PhotonNetwork.CurrentRoom.Name);
        PhotonNetwork.LoadLevel("03. KSH_PuzzleMode_D_Photon");
    }

}
