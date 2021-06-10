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
        PhotonNetwork.JoinOrCreateRoom("辫铰泅", roomOption, TypedLobby.Default);
        print("规 积己 己傍!");
        //PhotonNetwork.JoinRandomRoom();
    }
    public override void OnCreatedRoom()
    {

        print("规 积己 己傍");

    }

    public void JoinRoom()
    {

        print("JoinRoom is completed");
        print(PhotonNetwork.CurrentRoom.Name);
        PhotonNetwork.JoinRoom("辫铰泅");
        
    }
    public override void OnJoinedRoom()
    {
        print("规 立加 己傍");
        print(PhotonNetwork.CurrentRoom.Name);
        PhotonNetwork.LoadLevel("03. KSH_PuzzleMode_D_Photon");
    }

}
