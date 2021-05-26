using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public static int height = 24;
    public static int width = 10;
    private static Transform[,] grid = new Transform[width, height];
    float currtime, checktime = 3;
    int k = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    

    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.name == "Puzzle")
        //{
        //    Vector2 xy = collision.gameObject.transform.position;
        //    int x = (int)xy.x;
        //    int y = (int)xy.y;

        //    collision.gameObject.transform.position = new Vector2(x, y);
        //}
    }

    void AddtoGrid()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            int positionX = Mathf.RoundToInt(transform.GetChild(i).position.x);
            int positionY = Mathf.RoundToInt(transform.GetChild(i).position.y);
            grid[positionX, positionY] = transform.GetChild(i);
        }
    }
}
