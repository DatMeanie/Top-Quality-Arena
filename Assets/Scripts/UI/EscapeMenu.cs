using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeMenu : MonoBehaviour {

    //escape menu
    public GameObject escapeMenu;
    public CameraController cameraController;
    private void Start()
    {
        //pause set to false
        escapeMenu.SetActive(false);
    }
    void Update()
    {
        BringUp();
    }
    public void Resume()
    {
        //back to default state
        escapeMenu.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        cameraController.ChangeState(true);
    }
    public void BringUp()
    {
        //if press escape: cursor free and visible, time set to 0 and no camera movement
        if (Input.GetKey(KeyCode.Escape))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
            escapeMenu.SetActive(true);
            cameraController.ChangeState(false);
        }
    }
    public void BackToMenu()
    {
        //time needs to be set to 1 again or else it will be stuck at timescale 0
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
