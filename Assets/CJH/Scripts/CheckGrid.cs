using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGrid : MonoBehaviour
{
    public static int height = 24;
    public static int width = 10;
    private static Transform[,] grid = new Transform[width, height];
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

    bool MovingRange()    //이동 가능 범위
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            int positionX = Mathf.RoundToInt(transform.GetChild(i).position.x);
            int positionY = Mathf.RoundToInt(transform.GetChild(i).position.y);
            if (positionX < 0 || positionX >= width || positionY < 0 || positionY >= height)
            {
                return false;
            }
            if (grid[positionX, positionY])
                return false;
        }
        return true;
    }
}
