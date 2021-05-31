using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public static ButtonManager instance;
    public GameObject settingUI;
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

    private void Awake()
    {
        instance = this;
    }

    public void OnClickCancle()             //나가기
    {
        settingUI.SetActive(false);
    }

    public void OnClickRetry()            //재도전
    {
        SceneManager.LoadScene((int)state);
    }

    public void OnClickOther()            //다른 목록보기
    {
        SceneManager.LoadScene(1);
    }

    public void OnClickStart()      //클릭시 스타트
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
