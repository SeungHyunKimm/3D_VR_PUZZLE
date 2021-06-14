using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetManager : MonoBehaviourPunCallbacks
{
    public string gameVerstion = "1";
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.GameVersion = gameVerstion;    //게임 버전 설정
        PhotonNetwork.AutomaticallySyncScene = true; //씬 동기화 여부 설정
        PhotonNetwork.ConnectUsingSettings();        //접속
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.NickName = "Player" + Random.Range(0, 4);
        PhotonNetwork.JoinLobby();                   //방 접속
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print("OnJoinedLobby");
        PhotonNetwork.JoinOrCreateRoom("CJH", new RoomOptions(), TypedLobby.Default); // 방 입장 및 생성
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print("OnCreatedRoom"); 
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("OnJoinedRoom");
        PhotonNetwork.Instantiate("Player", new Vector3(5, 5, 10) , new Quaternion(0, 180 , 0 , 1));
    }

}
