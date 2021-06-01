using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    GameObject star;
    Vector3[] dir;
    float speed;
    public GameObject center;
    // Start is called before the first frame update
    private void Start()
    {
        speed = 25;
        star = GameObject.Find("Star");
        dir = new Vector3[star.transform.childCount];
    }

    private void Update()
    {
        center.transform.localScale += new Vector3(0.001f, 0.001f, 0.001f);
        for (int i = 0; i < star.transform.childCount; i++)
        {
            print(Vector3.Distance(transform.position, star.transform.GetChild(i).position));
            if (Vector3.Distance(transform.position, star.transform.GetChild(i).position) <= 1)
            {
                star.transform.GetChild(i).localScale -= new Vector3(0.15f, 0.15f, 0.15f);
                center.transform.localScale -= new Vector3(0.05f, 0.05f, 0.05f);
            }
            else
            {
                dir[i] = transform.position - star.transform.GetChild(i).position;
                dir[i].Normalize();
                star.transform.GetChild(i).position += dir[i] * speed * Time.deltaTime;
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        Destroy(other.gameObject);
    }

}
