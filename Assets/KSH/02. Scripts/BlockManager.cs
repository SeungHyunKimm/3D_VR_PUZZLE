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
        //���⼭ �� ������ ��Ȳ�� �����̴��� �����Ű�� �ʹ�.
        totalAmount_Slider.value = totalBlockNumber * 0.01f;
    }
}