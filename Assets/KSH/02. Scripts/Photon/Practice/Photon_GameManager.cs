using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Photon_GameManager : MonoBehaviour
{
    void Start()
    {

        //³» Player »ý¼º
        PhotonNetwork.Instantiate("Player", new Vector3(0,0,0), Quaternion.identity);
        
    }

    void Update()
    {
        
    }
}
