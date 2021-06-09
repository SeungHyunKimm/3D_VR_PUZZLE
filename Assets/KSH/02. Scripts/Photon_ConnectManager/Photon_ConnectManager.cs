using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Photon_ConnectManager : MonoBehaviourPunCallbacks
{
    //���ӹ���
    public string gameVersion = "1";
    public void ModeD_Connection()
    {
        //1. Game Version�� �����Ѵ�.
        PhotonNetwork.GameVersion = gameVersion;
        //2. ���� ����ȭ�Ұ��� ����
        PhotonNetwork.AutomaticallySyncScene = true;
        //3. ����
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnected()
    {
        base.OnConnected();
    }
}
