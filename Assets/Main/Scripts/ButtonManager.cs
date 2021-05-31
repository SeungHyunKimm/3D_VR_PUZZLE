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
    public void OnClickCancle()             //나가기
    {
        gameObject.SetActive(false);
    }

    public void OnClickRetry()            //재도전
    {
        SceneManager.LoadScene(2);
    }

    public void OnClickOther()            //다른 목록보기
    {
        SceneManager.LoadScene(1);
    }
}
