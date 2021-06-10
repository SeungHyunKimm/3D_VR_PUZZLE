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
        //���� nickname�� ���̰� 0�̸�
        //if (nickname.text.Length == 0)
        //{
        //    //������ �����Ѵ�.
        //    Debug.LogWarning("���̵� �Է��ϼ���.");
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
        print("�� ���� ����");
    }

    //�� ���� ����
    public override void OnJoinedRoom()
    {
        print("�� ���� ����");
        print(PhotonNetwork.CurrentRoom.Name);
    }

    public void CreateRoom()
    {
        RoomOptions roomOption = new RoomOptions();
        roomOption.MaxPlayers = 2;
        PhotonNetwork.JoinOrCreateRoom("�����", roomOption, TypedLobby.Default);
    }

    public void JoinRoom()
    {
        print("JoinRoom is completed");
        print(PhotonNetwork.CurrentRoom.Name);
        PhotonNetwork.LoadLevel("KSH_PuzzleMode_D_Photon");
    }





}
