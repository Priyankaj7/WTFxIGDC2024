using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]string[] gameScenes;

    public void OnPlayButtonClick(){
        SceneManager.LoadScene(gameScenes[0]);
    }

    public void OnQuit(){
        Application.Quit();
    }

}
