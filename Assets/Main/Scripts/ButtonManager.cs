using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public void OnClickCancle()             //������
    {
        gameObject.SetActive(false);
    }

    public void OnClickRetry()            //�絵��
    {
        SceneManager.LoadScene(2);
    }

    public void OnClickOther()            //�ٸ� ��Ϻ���
    {
        SceneManager.LoadScene(1);
    }
}
