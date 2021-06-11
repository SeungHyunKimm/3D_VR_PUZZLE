using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonNet_Manager : MonoBehaviourPunCallbacks
{
    void Start()
    {

        PhotonNetwork.Instantiate("VRPlayer_KSH", new Vector3(5, 5, -5), Quaternion.identity);

    }

    void Update()
    {
        
    }
}
