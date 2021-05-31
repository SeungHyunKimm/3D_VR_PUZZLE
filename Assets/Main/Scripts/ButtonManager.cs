using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public enum ButtonState
    {
        Start,
        Select,
        Mode_A,
        Mode_B,
        Mode_C,
        Mode_D
    }

    public ButtonState state;

    void Start()
    {

    }

    private void Update()
    {
    }

    public void OnClickCancel()             //������
    {
        gameObject.SetActive(false);
    }

    public void OnClickRetry()            //�絵��
    {

    }

    public void OnClickOther()            //�ٸ� ��Ϻ���
    {

    }

    public void OnClickStart()      //Ŭ���� ��ŸƮ
    {
        SceneManager.LoadScene(1);
        ;
    }
    public void OnClickMode_A()     // Mode_A Scene Load
    {
        SceneManager.LoadScene(2);


    }
    public void OnClickMode_B()     // Mode_B Scene Load
    {
        SceneManager.LoadScene(3);


    }
    public void OnClickMode_C()     // Mode_C Scene Load
    {
        SceneManager.LoadScene(4);


    }
    public void OnClickMode_D()     // Mode_D Scene Load
    {
        SceneManager.LoadScene(5);

    }
}
