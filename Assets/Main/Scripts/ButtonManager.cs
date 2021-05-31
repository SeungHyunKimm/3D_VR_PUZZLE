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
    public void OnClickExit()             //나가기
    {
        gameObject.SetActive(false);
    }

    public void OnClickRetry()            //재도전
    {

    }

    public void OnClickOther()            //다른 목록보기
    {

    }
}
