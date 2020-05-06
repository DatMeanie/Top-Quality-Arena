using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ProgressionTab : MonoBehaviour {

    //UI
    public GameObject progressionTabParent;
    public GameObject buttonsParent;
    public GameObject progressionActivator;
    public GameObject changeLog;
    public GameObject areYouSureParent;

    //original pos
    Vector3 originalPositionForButtonParent;
    Vector3 originalPositionForExperienceTabParent;
    Vector3 originalPositionForProgressionTabActivator;

    //experience
    public Text experienceText;
    int experience;
    public ExperienceAndUnlocks experienceData;

    //contains all weapon buttons
    public GameObject unlocks;

    void Start () {
        //get original pos
        originalPositionForButtonParent = GameObject.Find("AllScreenArea").transform.position;
        originalPositionForExperienceTabParent = progressionTabParent.transform.position;
        originalPositionForProgressionTabActivator = progressionActivator.transform.position;
        //get exp
        experience = experienceData.experience;
	}
    public void OpenProgressionTab()
    {
        experience = experienceData.experience;
        experienceText.text = "Experience: " + experience.ToString();
        //animations
        buttonsParent.transform.DOMoveX(-400, 2.0f).SetEase(Ease.OutCirc);
        changeLog.transform.DOMoveX(-1200, 2.0f).SetEase(Ease.OutCirc);
        progressionActivator.transform.DOMoveY(originalPositionForButtonParent.y + originalPositionForExperienceTabParent.y, 2.0f).SetEase(Ease.OutCirc);
        progressionTabParent.transform.DOMoveY(originalPositionForButtonParent.y, 1.0f).SetEase(Ease.InCirc);

        //temporary method for unlocks
        //every weapon is 100 exp
        //disable weapon buttons if player does not have necessary exp
        int neededExp = 0;
        foreach(Transform trans in unlocks.transform)
        {
            if(experience >= neededExp)
            {
                trans.gameObject.SetActive(true);
            }
            else
            {
                trans.gameObject.SetActive(false);
            }
            neededExp += 100;
        }
    }
    public void CloseProgressionTab()
    {
        //go back to original pos
        buttonsParent.transform.DOMoveX(400, 2.0f).SetEase(Ease.OutCirc);
        progressionActivator.transform.DOMoveY(originalPositionForProgressionTabActivator.y, 2.0f).SetEase(Ease.OutCirc);
        progressionTabParent.transform.DOMoveY(originalPositionForExperienceTabParent.y, 1.0f).SetEase(Ease.OutCirc);
    }

    //reset exp
    public void AreYouSureCheck()
    {
        areYouSureParent.SetActive(true);
    }
    public void NoDontReset()
    {
        areYouSureParent.SetActive(false);
    }
    public void ResetProgression()
    {
        areYouSureParent.SetActive(false);
        GameObject.Find("Experience").GetComponent<ExperienceAndUnlocks>().ResetExperience();
        GameObject.Find("Experience").GetComponent<ExperienceAndUnlocks>().SaveData();
    }
}
