using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockStateWords : MonoBehaviour
{
    Text tx;
    public float[] rgb;
    int i;
    public float j;
    void Start()
    {
        tx = GetComponent<Text>();
    }

    void Update()
    {
       if (rgb[i] <= 0 || rgb[i] >= 1)
        {
            j *= -1;
            i++;
        }
        i %= rgb.Length;
        rgb[i] += j;
        tx.color = new Color(rgb[0], rgb[1], rgb[2], 0.8f);

        //나이트클럽 색상
        //tx.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1);
        
    }
}
