using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraController : MonoBehaviour {

    //variables
    float horizontalSpeed = 2.0f;
    bool invertedY = false;
    float verticalSpeed = 2.0f;
    bool invertedX = false;
    bool scriptEnabled = true;
    //components
    Transform t;
    AmbientOcclusion amb = null;
    MotionBlur blur = null;
    PostProcessVolume volume;

    GameObject playerObject;
    Vector3 offset;

    private void Start()
    {
        //post processing settings
        volume = Camera.main.GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings(out amb);
        volume.profile.TryGetSettings(out blur);
        //turned off or on
        amb.enabled.value = true ? PlayerPrefs.GetInt("AmbientOcclusion") == 1 : false;
        Debug.Log("Ambient Occlusion: " + PlayerPrefs.GetInt("AmbientOcclusion"));
        blur.enabled.value = true ? PlayerPrefs.GetInt("MotionBlur") == 1 : false;
        Debug.Log("Motion Blur: " + PlayerPrefs.GetInt("MotionBlur"));

        invertedX = true ? PlayerPrefs.GetInt("InvertedX") == 1 : false;
        invertedY = true ? PlayerPrefs.GetInt("InvertedY") == 1 : false;

        GetComponent<Camera>().farClipPlane = PlayerPrefs.GetInt("DrawDistance");
        t = transform;
        playerObject = GameObject.Find("Player");
        offset = transform.position - playerObject.transform.position;
    }
    void Update () {
        //mouse controls
        if (scriptEnabled == false) return;
        float v = verticalSpeed * Input.GetAxis("Mouse X") * PlayerPrefs.GetFloat("MouseSens");
        float h = horizontalSpeed * Input.GetAxis("Mouse Y") * -1 * PlayerPrefs.GetFloat("MouseSens");
        //inverted mouse
        if (invertedX)
        {
            v *= -1;
        }
        if (invertedY)
        {
            h *= -1;
        }
        t.Rotate(h, v, 0);
        //z angle not change
        t.eulerAngles = new Vector3(t.eulerAngles.x, t.eulerAngles.y, 0);
        //player should not look be able to look too far up or down
        if (t.eulerAngles.x > 40 && t.eulerAngles.x < 60)
        {
            t.eulerAngles = new Vector3(40, t.eulerAngles.y, 0);
        }
        if (t.eulerAngles.x < 300 && t.eulerAngles.x > 80)
        {
            t.eulerAngles = new Vector3(300, t.eulerAngles.y, 0);
        }
        t.position = new Vector3(playerObject.transform.position.x, playerObject.transform.position.y + offset.y, playerObject.transform.position.z);
	}
    public void ChangeState(bool newState)
    {
        scriptEnabled = newState;
    }
}
