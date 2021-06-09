using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Photon_ConnectManager : MonoBehaviourPunCallbacks
{
    //게임버전
    public string gameVersion = "1";


    //int => key값, string => Value값
    //방 목록 캐시
    Dictionary<string, RoomInfo> roomCache = new Dictionary<string, RoomInfo>();


    //scrollview - content
    public Transform content;

    //RoomInfo 버튼 공장
    public GameObject roomInfoFactory;

    private void Start()
    {
        


    }

    public void ModeD_Connection()
    {
        //1. Game Version을 설정한다.
        PhotonNetwork.GameVersion = gameVersion;
        //2. 씬을 동기화할건지 여부
        PhotonNetwork.AutomaticallySyncScene = true;
        //3. 접속
        PhotonNetwork.ConnectUsingSettings();
    }

    //Name Server 접속 성공

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
        PhotonNetwork.LoadLevel("LobbyScene");
        CreateRoom();
    }
    //방 생성 성공
    public override void OnCreatedRoom()
    {
        print("방 생성 성공");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {

        base.OnCreateRoomFailed(returnCode, message);

    }
    //방 접속 성공
    public override void OnJoinedRoom()
    {
        print("방 접속 성공");
        print(PhotonNetwork.CurrentRoom.Name);
    }

    void CreateRoom()
    {
        //방옵션
        RoomOptions roomOption = new RoomOptions();
        //MaxPlayer 0으로 설정시 무한대 접속 가능
        roomOption.MaxPlayers = 1;
        //방을 만든다.
        PhotonNetwork.JoinOrCreateRoom("방이름", roomOption, TypedLobby.Default);
        //PhotonNetwork.JoinRandomRoom();
        //PhotonNetwork.JoinRoom("방이름");
    }

    public void JoinRoom()
    {
        print("방 접속 성공");
        print(PhotonNetwork.CurrentRoom.Name);
        //GameScene으로 이동
        PhotonNetwork.LoadLevel("GameScene");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);

        Debug.LogWarning("방 접속 실패");
    }

    //현재 방 정보 갱신
    // 최초에는 전체 방 리스트를 준다.
    // 그 다음은 추가 삭제된 방 정보만 들어온다.

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        for(int i = 0; i < roomList.Count; i++)
        {
            print(roomList[i].Name);
            
        }

        // 현재 만들어진 UI를 삭제
        DeleteRoomList();
        // roomCache 정보 갱신
        UpdateRoomCache(roomList);
        // UI 새롭게 만든다.

        CreateRoomList();
    }

    // RoomCache갱신
    void UpdateRoomCache(List<RoomInfo> roomList)
    {
        for(int i = 0; i < roomList.Count; i++)
        {
            //만약에 변경 또는 추가된 방이 RoomCache에 있으면
            if (roomCache.ContainsKey(roomList[i].Name))
            {
                //만약에 그 방을 지워야 한다면
                if (roomList[i].RemovedFromList)
                {
                    roomCache.Remove(roomList[i].Name);
                    continue;
                }

            }
            //방을 roomCache에 변경 또는 추가
            roomCache[roomList[i].Name] = roomList[i];

        }


    }

    //방 정보 삭제
    void DeleteRoomList()
    {
        foreach(Transform tr in content)
        {
            Destroy(tr.gameObject);
        }
    }

    void CreateRoomList()
    {

        foreach(RoomInfo info in roomCache.Values)
        {
            //1. RoomInfo 버튼 공장에서 roomInfo 버튼 생성
            GameObject room = Instantiate(roomInfoFactory);
            //2. 만들어진 roomInfo버튼을 content의 자식으로 넣는다.
            room.transform.parent = content;
            //3. 만들어진 roomInfo버튼에서 RoomInfoBtn 컴포넌트 가져온다.
            //RoomInfoBtn btn = room.GetComponent<RoomInfoBtn>();
            //4. 가져온 컴포넌트의 SetInformation 함수 호출
            //btn.SetInformation(info.Name, info.PlayerCount, info.MaxPlayers);
        }

    }



}
