using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedPuzzle : MonoBehaviour
{
    float currtime, checktime = 3;
    int k = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
            //currtime += Time.deltaTime;
            //if(currtime > checktime)
            //{
            //    print(k);
            //    currtime = 0;
            //    k++;
            //}
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "puzzle")
        {
            Vector2 xy = collision.gameObject.transform.position;
            int x = (int)xy.x;
            int y = (int)xy.y;

            //currtime += Time.deltaTime;
            //if (currtime > checktime)
            //{
            //    xy.x -= 0.01f;
            //    currtime = 0;

            //    print(xy.x);
            //}

            collision.gameObject.transform.position = new Vector2(x, y);
        }
    }
}
