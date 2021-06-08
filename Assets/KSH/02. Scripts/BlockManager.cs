using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class BlockManager : MonoBehaviour
{

    public Slider totalAmount_Slider;
    public int totalBlockNumber = 0;

    private void Start()
    {

    }
    private void Update()
    {
        //여기서 총 갯수의 현황을 슬라이더에 적용시키고 싶다.
        totalAmount_Slider.value = totalBlockNumber * 0.01f;
    }
}