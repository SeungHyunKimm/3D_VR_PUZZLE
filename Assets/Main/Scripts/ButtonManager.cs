using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
    }

    // Update is called once per frame
    public void OnClickExit()             //������
    {
        gameObject.SetActive(false);
    }

    public void OnClickRetry()            //�絵��
    {

    }

    public void OnClickOther()            //�ٸ� ��Ϻ���
    {

    }
}
