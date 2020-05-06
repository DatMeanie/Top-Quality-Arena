using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Won : MonoBehaviour {

    //components
    public PlayerHealth ph;
    public CameraController cameraController;
    public GameObject playingUI;
    public GameObject wonUI;
    public GameObject backToMenuButton;
    bool experienceControl = true;

    private void Start()
    {
        ph = GameObject.Find("Player").GetComponent<PlayerHealth>();
    }
    void Update () {
        StartCoroutine(checkIfWon());
    }
    //problems if not have timer
    IEnumerator checkIfWon()
    {
        yield return new WaitForSeconds(1.0f);
        //if all coins have been taken
        if (GameObject.Find("MapManager").GetComponent<MapManager>().gameMode == "Coin Hunt")
        {
            if (GameObject.FindGameObjectsWithTag("Coin").Length == 0)
            {
                //change cursor
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                //deactivate
                ph.enabled = false;
                cameraController.enabled = false;
                playingUI.SetActive(false);
                //animation
                backToMenuButton.transform.DOMove(playingUI.transform.position, 2.0f);
                if (experienceControl)
                {
                //save
                    if (GameObject.Find("Experience"))
                    {
                        GameObject.Find("Experience").GetComponent<ExperienceAndUnlocks>().SaveData();
                        GameObject.Find("LevelConfig").GetComponent<SettingsForMap>().ResetValues();
                    }
                    experienceControl = false;
                }
            }
        }
        //if all enemies have been killed
        else if (GameObject.Find("MapManager").GetComponent<MapManager>().gameMode == "Enemy Massacre")
        {
            if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
            {
                //change cursor
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                //deactivate
                ph.enabled = false;
                cameraController.enabled = false;
                playingUI.SetActive(false);
                //animation
                backToMenuButton.transform.DOMove(playingUI.transform.position, 2.0f);
                if (experienceControl)
                {
                //save
                    if (GameObject.Find("Experience"))
                    {
                        GameObject.Find("Experience").GetComponent<ExperienceAndUnlocks>().SaveData();
                        GameObject.Find("LevelConfig").GetComponent<SettingsForMap>().ResetValues();
                    }
                    experienceControl = false;
                }
            }
        }
    }
    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
