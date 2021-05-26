using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleRevolution : MonoBehaviour
{
    Transform canvas;
    Rigidbody rigid;
    Vector3 dir;
    Vector2 xy;
    int x, y;
    float dist;
    bool accessCanvas;
    float currtime, checktime = 3;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        StartCoroutine(ResetGravity());
        GameObject canvasGo = GameObject.Find("Canvas");
        canvas = canvasGo.GetComponent<Transform>();
    }

    // Update is called once per frame
    private void Update()
    {
        //if (!accessCanvas)
        //    Revolution();
        //if (Input.GetMouseButtonDown(0))
        //{
        //  if (!accessCanvas)
        //     Move();
        //}
        FixedPuzzle();
        //print(xy);
    }

    IEnumerator ResetGravity()
    {
        yield return new WaitForSeconds(1);
        rigid.useGravity = false;
    }

   void Revolution()    // 공전 함수
    {
        dist = Vector3.Distance(transform.position, canvas.transform.position);
        transform.forward = canvas.transform.position - transform.position;

        if (dist <= 30)
        {
            dir = transform.forward + transform.right;
            rigid.AddForce(dir * 0.005f, ForceMode.Impulse);
        }
        else
        {
            dir = transform.position - canvas.transform.position;
            rigid.AddForce(-dir * 0.01f, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        xy = transform.position;
        x = (int)xy.x;
        y = (int)xy.y;
        print(x / 0.5);
        if (collision.gameObject.name == "Canvas")
            accessCanvas = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "Canvas")
            accessCanvas = false;
    }

    void Move()
    {
        rigid.AddForce(transform.forward * 0.3f, ForceMode.Impulse);
    }

    void FixedPuzzle()
    {
        if(xy.x >= x)
        {
            xy.x -= 0.01f;
            transform.position += new Vector3(-0.01f, 0);
        }
        if (xy.y >= y)
        {
            xy.y -= 0.01f;
            transform.position += new Vector3(0 , -0.01f);
        }
    }
}
