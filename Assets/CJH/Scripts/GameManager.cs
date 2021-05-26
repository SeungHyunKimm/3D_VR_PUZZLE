using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Rigidbody rigid;
    Vector3 hitposition;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
        if (Input.GetMouseButtonDown(0))
        {
            if(Physics.Raycast(ray , out hit))
            {
                rigid = hit.transform.GetComponent<Rigidbody>();
                Vector3 dir = Camera.main.transform.position - hit.transform.position;
                rigid.AddForce(dir * 0.2f, ForceMode.Impulse);
                
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if(Physics.Raycast(ray , out hit))
            {
                rigid = hit.transform.GetComponent<Rigidbody>();
                hit.transform.position = transform.position;
                rigid.isKinematic = true;
            }
        }
    }
}
