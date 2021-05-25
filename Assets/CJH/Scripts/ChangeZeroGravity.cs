using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeZeroGravity : MonoBehaviour
{
    Rigidbody rigid;
    Vector3 dir;
    float dist;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Sphere")
        {
            dist = Vector3.Distance(transform.position, other.transform.position);
            transform.forward = other.transform.position - transform.position;
            
            if (dist <= 30)
            {
                dir = transform.forward + transform.right;
                rigid.AddForce(dir * 0.01f, ForceMode.Impulse);
            }
            else
            {
                dir = transform.position - other.transform.position;
                rigid.AddForce(-dir * 0.01f, ForceMode.Impulse);
            }
        }
    }
}
