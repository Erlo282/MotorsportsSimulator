using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Selection : MonoBehaviour
{
    public void SceneLoader(string scenename)
    {
        SceneManager.LoadScene(scenename);
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
    
}
