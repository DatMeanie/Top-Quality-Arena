using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour {

    //scene changes
    public void LaunchMap()
    {
        SceneManager.LoadScene(1);
    }
    public void LaunchMapEditor()
    {
        SceneManager.LoadScene(2);
    }
    public void ExitApplication()
    {
        Application.Quit();
    }
}
