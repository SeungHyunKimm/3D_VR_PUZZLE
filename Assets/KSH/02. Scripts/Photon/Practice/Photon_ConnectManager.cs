using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Photon_ConnectManager : MonoBehaviourPunCallbacks
{
    //���ӹ���
    public string gameVersion = "1";


    //int => key��, string => Value��
    //�� ��� ĳ��
    Dictionary<string, RoomInfo> roomCache = new Dictionary<string, RoomInfo>();


    //scrollview - content
    public Transform content;

    //RoomInfo ��ư ����
    public GameObject roomInfoFactory;

    private void Start()
    {
        


    }

    public void ModeD_Connection()
    {
        //1. Game Version�� �����Ѵ�.
        PhotonNetwork.GameVersion = gameVersion;
        //2. ���� ����ȭ�Ұ��� ����
        PhotonNetwork.AutomaticallySyncScene = true;
        //3. ����
        PhotonNetwork.ConnectUsingSettings();
    }

    //Name Server ���� ����

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
    //�� ���� ����
    public override void OnCreatedRoom()
    {
        print("�� ���� ����");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {

        base.OnCreateRoomFailed(returnCode, message);

    }
    //�� ���� ����
    public override void OnJoinedRoom()
    {
        print("�� ���� ����");
        print(PhotonNetwork.CurrentRoom.Name);
    }

    void CreateRoom()
    {
        //��ɼ�
        RoomOptions roomOption = new RoomOptions();
        //MaxPlayer 0���� ������ ���Ѵ� ���� ����
        roomOption.MaxPlayers = 1;
        //���� �����.
        PhotonNetwork.JoinOrCreateRoom("���̸�", roomOption, TypedLobby.Default);
        //PhotonNetwork.JoinRandomRoom();
        //PhotonNetwork.JoinRoom("���̸�");
    }

    public void JoinRoom()
    {
        print("�� ���� ����");
        print(PhotonNetwork.CurrentRoom.Name);
        //GameScene���� �̵�
        PhotonNetwork.LoadLevel("GameScene");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);

        Debug.LogWarning("�� ���� ����");
    }

    //���� �� ���� ����
    // ���ʿ��� ��ü �� ����Ʈ�� �ش�.
    // �� ������ �߰� ������ �� ������ ���´�.

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        for(int i = 0; i < roomList.Count; i++)
        {
            print(roomList[i].Name);
            
        }

        // ���� ������� UI�� ����
        DeleteRoomList();
        // roomCache ���� ����
        UpdateRoomCache(roomList);
        // UI ���Ӱ� �����.

        CreateRoomList();
    }

    // RoomCache����
    void UpdateRoomCache(List<RoomInfo> roomList)
    {
        for(int i = 0; i < roomList.Count; i++)
        {
            //���࿡ ���� �Ǵ� �߰��� ���� RoomCache�� ������
            if (roomCache.ContainsKey(roomList[i].Name))
            {
                //���࿡ �� ���� ������ �Ѵٸ�
                if (roomList[i].RemovedFromList)
                {
                    roomCache.Remove(roomList[i].Name);
                    continue;
                }

            }
            //���� roomCache�� ���� �Ǵ� �߰�
            roomCache[roomList[i].Name] = roomList[i];

        }


    }

    //�� ���� ����
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
            //1. RoomInfo ��ư ���忡�� roomInfo ��ư ����
            GameObject room = Instantiate(roomInfoFactory);
            //2. ������� roomInfo��ư�� content�� �ڽ����� �ִ´�.
            room.transform.parent = content;
            //3. ������� roomInfo��ư���� RoomInfoBtn ������Ʈ �����´�.
            //RoomInfoBtn btn = room.GetComponent<RoomInfoBtn>();
            //4. ������ ������Ʈ�� SetInformation �Լ� ȣ��
            //btn.SetInformation(info.Name, info.PlayerCount, info.MaxPlayers);
        }

    }



}
