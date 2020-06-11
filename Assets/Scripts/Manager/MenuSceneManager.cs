using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSceneManager : MonoBehaviour
{
    public void StartGame(){
        SceneManager.LoadScene("SampleScene");
    }
    public void ExitGame(){
        Debug.Log("Exit Game");
        Application.Quit();
    }
}
