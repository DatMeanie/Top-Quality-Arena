using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEditorCameraMovement : MonoBehaviour {
    
    //Script is combination of CameraController.cs and PlayerController.cs
    //allows object to move around freely 

    bool invertedY = false;
    bool invertedX = false;
    //transform for gameobject
    Transform t;
    private void Start()
    {
        //inverted
        if (GameObject.Find("controlSettings"))
        {
            invertedX = true ? PlayerPrefs.GetInt("InvertedX") == 1 : false;
            invertedY = true ? PlayerPrefs.GetInt("InvertedY") == 1 : false;
        }
        t = transform;
    }
    private void Update()
    {
        //mouse controls
        float v = 1.0f * Input.GetAxis("Mouse X") * PlayerPrefs.GetFloat("MouseSens");
        float h = 1.0f * Input.GetAxis("Mouse Y") * -1 * PlayerPrefs.GetFloat("MouseSens");
        if (invertedX)
        {
            v *= -1;
        }
        if (invertedY)
        {
            h *= -1;
        }

        //movement
        //hold in leftalt to go faster
        if (Input.GetKey(KeyCode.W))
        {
            if (Input.GetKey(KeyCode.LeftAlt))
            {
                transform.position += transform.forward * Time.deltaTime * 20;
            }
            else
            {
                transform.position += transform.forward * Time.deltaTime * 12;
            }
        }
        if (Input.GetKey(KeyCode.A))
        {
            if (Input.GetKey(KeyCode.LeftAlt))
            {
                transform.position -= transform.right * Time.deltaTime * 20;
            }
            else
            {
                transform.position -= transform.right * Time.deltaTime * 12;
            }
        }
        if (Input.GetKey(KeyCode.S))
        {
            if (Input.GetKey(KeyCode.LeftAlt))
            {
                transform.position -= transform.forward * Time.deltaTime * 20;
            }
            else
            {
                transform.position -= transform.forward * Time.deltaTime * 12;
            }
        }
        if (Input.GetKey(KeyCode.D))
        {
            if (Input.GetKey(KeyCode.LeftAlt))
            {
                transform.position += transform.right * Time.deltaTime * 20;
            }
            else
            {
                transform.position += transform.right * Time.deltaTime * 12;
            }
        }

        //more aroundd more freely when usind leftshift
        if (Input.GetKey(KeyCode.LeftShift))
        {
            t.Rotate(h, v, 0);
            t.eulerAngles = new Vector3(t.eulerAngles.x, t.eulerAngles.y, 0);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
