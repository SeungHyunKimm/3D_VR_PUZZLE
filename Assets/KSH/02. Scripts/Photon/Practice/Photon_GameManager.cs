using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Photon_GameManager : MonoBehaviour
{
    void Start()
    {

        //�� Player ����
        PhotonNetwork.Instantiate("VRPlayer", new Vector3(0,0,0), Quaternion.identity);
        
    }

    void Update()
    {
        
    }


}
