using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour {

    //start menu for mapeditor

    //components, gameobjects
    public GameObject welcomer;
    public GameObject createSet;
    public InputField nameInput;
    public GameObject tools;
    GameObject usedSet;
    public Dropdown setDropDown;
    public GameObject changeExistingSet;

    List<string> setNameList = new List<string>();

    void Start()
    {
        //find sets and add them to dropdown
        if (GameObject.Find("All Sets"))
        {
            List<Transform> setList = new List<Transform>();
            foreach (Transform trans in GameObject.Find("All Sets").transform)
            {
                setList.Add(trans);
                setNameList.Add(trans.name);
            }
            setDropDown.ClearOptions();
            setDropDown.AddOptions(setNameList);
        }
        else
        {
            List<Transform> setList = new List<Transform>();
            foreach (Transform trans in GameObject.Find("BackUpSets").transform)
            {
                setList.Add(trans);
                setNameList.Add(trans.name);
            }
            setDropDown.ClearOptions();
            setDropDown.AddOptions(setNameList);
        }
    }

    public void CreateSet()
    {
        //change name in setcompiler to new map set name
        GameObject.Find("SetCompiler").GetComponent<SetCompiler>().ChangeOriginalName(nameInput.text);
        usedSet = GameObject.Find("UsedSet");

        //create new chunk to make setcompiler work
        GameObject newChunk = new GameObject("NewChunk");
        newChunk.transform.parent = usedSet.transform;
        newChunk.transform.position = new Vector3(0, 0, 0);
        newChunk.AddComponent<SpecialChunk>();
        GameObject.Find("SetCompiler").GetComponent<SetCompiler>().AddChunk(newChunk.transform);

        createSet.SetActive(false);
        tools.SetActive(true);
        Camera.main.GetComponent<MapEditorCameraMovement>().enabled = true;

    }
    public void ShowCreateSet()
    {   //show tools
        welcomer.SetActive(false);
        createSet.SetActive(true);
    }
    public void ShowChangeExistingSet()
    {
        welcomer.SetActive(false);
        changeExistingSet.SetActive(true);
    }
    public void ChangeSet()
    {
        tools.SetActive(true);
        changeExistingSet.SetActive(false);
        Camera.main.GetComponent<MapEditorCameraMovement>().enabled = true;
    }
    public void MenuButton()
    {
        SceneManager.LoadScene(0);
    }
}
