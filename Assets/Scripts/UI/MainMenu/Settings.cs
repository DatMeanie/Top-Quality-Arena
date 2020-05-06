using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Rendering.PostProcessing;

public class Settings : MonoBehaviour {

    //objects to move during transition
    public GameObject buttonsParent;
    public GameObject settingsParent;
    public GameObject changeLog;
    public GameObject progressionTab;
    //original pos
    Vector3 originalPositionForButtonParent;
    Vector3 originalPositionForSettingsParent;
    Vector3 originalPositionForProgressionTab;

    //options that can be changed
    public Slider mouseSlider;
    public Text sensText;
    public Toggle btnmouseX;
    public Toggle btnmouseY;
    public Slider drawDistanceSlider;
    public Text drawDistanceText;
    public Toggle ambToggle;
    public Toggle blurToggle;

    //post processing
    AmbientOcclusion amb = null;
    MotionBlur blur = null;
    PostProcessVolume volume;

    void Start () {

        volume = Camera.main.GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings(out amb);
        volume.profile.TryGetSettings(out blur);
        //first time running the game
        if (PlayerPrefs.GetInt("FirstRun") != 10)
        {
            PlayerPrefs.SetInt("FirstRun", 10);
            PlayerPrefs.SetFloat("MouseSens", 2);
            PlayerPrefs.SetInt("DrawDistance", 300);
            PlayerPrefs.SetInt("AmbientOcclusion", 1);
            PlayerPrefs.SetInt("MotionBlur", 1);
        }
        //get original pos
        originalPositionForButtonParent = buttonsParent.transform.position;
        originalPositionForSettingsParent = settingsParent.transform.position;
        originalPositionForProgressionTab = progressionTab.transform.position;
    }
    public void EnableSettings()
    {
        //transition to settings menu
        changeLog.transform.DOMoveX(-1200, 2.0f).SetEase(Ease.OutCirc);
        buttonsParent.transform.DOMoveX(-400, 2.0f).SetEase(Ease.OutCirc);
        progressionTab.transform.DOMoveY(originalPositionForSettingsParent.y, 1.0f);
        settingsParent.transform.DOMoveY(originalPositionForButtonParent.y, 2.0f).SetEase(Ease.InCirc);
        //make options same as playerprefs
        btnmouseX.isOn = true ? PlayerPrefs.GetInt("InvertedX") == 1 : false;
        btnmouseY.isOn = true ? PlayerPrefs.GetInt("InvertedY") == 1 : false;
        ambToggle.isOn = true ? PlayerPrefs.GetInt("AmbientOcclusion") == 1 : false;
        blurToggle.isOn = true ? PlayerPrefs.GetInt("MotionBlur") == 1 : false;
        mouseSlider.value = PlayerPrefs.GetFloat("MouseSens");
        drawDistanceSlider.value = PlayerPrefs.GetInt("DrawDistance");
        sensText.text = mouseSlider.value.ToString();
    }
    public void DisableSetting()
    {
        //transition to main menu
        progressionTab.transform.DOMoveY(originalPositionForProgressionTab.y, 1.0f);
        settingsParent.transform.DOMoveY(originalPositionForSettingsParent.y, 2.0f).SetEase(Ease.OutCirc);
        buttonsParent.transform.DOMoveX(originalPositionForButtonParent.x, 2.0f).SetEase(Ease.InCirc);
    }

    //changes to options

    public void ChangeSettingMouseX(bool isXInverted)
    {
        if (isXInverted)
        {
            PlayerPrefs.SetInt("InvertedX", 1);
        }
        else
        {
            PlayerPrefs.SetInt("InvertedX", 0);
        }
    }
    public void ChangeSettingMouseY(bool isYInverted)
    {
        if (isYInverted)
        {
            PlayerPrefs.SetInt("InvertedY", 1);
        }
        else
        {
            PlayerPrefs.SetInt("InvertedY", 0);
        }
    }
    public void ChangeSensitivity()
    {
        PlayerPrefs.SetFloat("MouseSens", mouseSlider.value);
        sensText.text = mouseSlider.value.ToString();
    }
    public void ChangeDrawDistance()
    {
        PlayerPrefs.SetInt("DrawDistance", (int)drawDistanceSlider.value);
        drawDistanceText.text = drawDistanceSlider.value.ToString();
    }
    public void ChangeAmb(bool newState)
    {
        if (ambToggle.isOn)
        {
            PlayerPrefs.SetInt("AmbientOcclusion", 1);
            amb.enabled.value = true;
        }
        else
        {
            PlayerPrefs.SetInt("AmbientOcclusion", 0);
            amb.enabled.value = false;
        }
    }
    public void ChangeBlur(bool newState)
    {
        if (blurToggle.isOn)
        {
            PlayerPrefs.SetInt("MotionBlur", 1);
            blur.enabled.value = true;
        }
        else
        {
            PlayerPrefs.SetInt("MotionBlur", 0);
            blur.enabled.value = false;
        }
    }
}
