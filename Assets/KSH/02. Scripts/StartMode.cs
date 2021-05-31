using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMode : MonoBehaviour
{
    public void OnClickStart()
    {
        SceneManager.LoadScene(1);
    }   
}
