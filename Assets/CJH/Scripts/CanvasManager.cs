using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public static int height = 24;
    public static int width = 10;
    private static Transform[,] grid = new Transform[width, height];
    Rigidbody rigid;
    Material mat;
    int k;
    float albedo;

    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
        k = 1;
    }

    private void Update()
    {
        SetAlbedo();
    }

    void SetAlbedo()                                         //캔버스의 명암 조절
    {
         
        if (mat.color.a >= 1 || mat.color.a <= 0)
            k *= -1;

        albedo += 0.001f * k;

        mat.color = new Color(0.5f, 0.5f, 0.5f, albedo);         
    }

    private void OnCollisionEnter(Collision collision)        //퍼즐이 캔버스에 끼워졌을 때 물리 작용 제거
    {
        rigid = collision.transform.GetComponent<Rigidbody>();
        rigid.isKinematic = true;
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
