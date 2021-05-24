using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeZeroGravity : MonoBehaviour
{
    Rigidbody rigid;
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
            float dist = Vector3.Distance(transform.position, other.transform.position);
            transform.forward = other.transform.position - transform.position;
            if (dist <= 30)
            {
                rigid.AddForce(other.transform.forward * 0.01f, ForceMode.Impulse);
            }
            else
            {
                Vector3 dir = other.transform.position - transform.position;
                rigid.AddForce(dir * 0.01f, ForceMode.Impulse);
            }

        }
    }
}
