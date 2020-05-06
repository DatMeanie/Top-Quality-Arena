using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class PlayerHealth : MonoBehaviour {

    //player health data

    //variables, components, gameobjects
    public int health;
    PlayerHealth ph;
    public CameraController cameraController;
    public GameObject playingUI;
    public GameObject diedButton;

    private void Start()
    {
        ph = GetComponent<PlayerHealth>();
    }

    void Update () {
		if(health <= 0)
        {
            StartCoroutine(checkIfDead());
        }
	}
    //problems if not on timer
    IEnumerator checkIfDead()
    {
        yield return new WaitForSeconds(0.5f);
        //cursor becomes usable
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        //deactivate components
        ph.enabled = false;
        cameraController.enabled = false;
        playingUI.SetActive(false);
        //animation
        diedButton.transform.DOMove(playingUI.transform.position, 2.0f);
    }
    //player has been damaged or healed
    public void ChangeHealth(int damage)
    {
        if (health > 0)
        {
            try
            {
                GameObject.Find("HurtEffect").GetComponent<Animator>().Play("Hurt");
            }
            catch
            {
                Debug.Log("Error: Hurt Effect");
            }
        }
        health -= damage;
    }
}
