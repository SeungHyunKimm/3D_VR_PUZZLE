using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class CamerMove : MonoBehaviour , IPunObservable
{
    float rotX, rotY;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = new Vector3(Input.GetAxis("Horizontal"), 0 , Input.GetAxis("Vertical"));
        dir.Normalize();

        transform.position += dir * 5 * Time.deltaTime;

        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");

        rotX += x;
        rotY += y;
        
        transform.eulerAngles = new Vector3(-rotY, rotX, 0);
    }
}
