using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SetCompiler : MonoBehaviour {

    //load sets and save sets

    //components and gameobjects
    GameObject finishedSet;
    GameObject newSet;
    PositionTool posScript;
    ScaleTool scaleScript;
    ObjectTools tools;
    SaveManager save;
    public Text setSelector;

    //variables
    string originalName;
    //chunks in the edited set
    List<Transform> editChunksList = new List<Transform>();
    //what chunk in editChunkList should be visible?
    int visibleChunk = 0;
    //backupset: debugging feature
    bool backupSetUsed = true;
    void Awake() {
        //puts all chunks into UsedSet
        newSet = new GameObject("UsedSet");
        newSet.transform.position = new Vector3(0, 0, 0);

        if (GameObject.Find("All Sets"))
        {
            Destroy(GameObject.Find("BackUpSets"));
            backupSetUsed = false;
        }
    }
    private void Start()
    {
        //initialize scripts for objects
        MoveObject[] moveableObjects = FindObjectsOfType<MoveObject>();
        save = GetComponent<SaveManager>();
        Debug.Log("Moveable Objects: " + moveableObjects.Length);
        foreach (MoveObject move in moveableObjects)
        {
            move.enabled = true;
            move.InitializeScript();
        }
        //get objects
        posScript = GameObject.Find("PositionTool").GetComponent<PositionTool>();
        scaleScript = GameObject.Find("ScaleTool").GetComponent<ScaleTool>();
        tools = GameObject.Find("UIScripts").GetComponent<ObjectTools>();
    }
    public void TransferSet()
    {
        //transfer chunks in chosen set to edit
        originalName = setSelector.text;
        Debug.Log("Loading " + setSelector.text);
        GameObject selectedSet = GameObject.Find(setSelector.text);
        foreach (Transform trans in selectedSet.transform)
        {
            editChunksList.Add(trans);
            trans.transform.position = new Vector3(0, 0, 0);
        }
        //above loop doesnt work properly if trans is changed, therefore its done this way instead
        foreach (Transform trans in editChunksList)
        {
            trans.transform.parent = newSet.transform;
        }
        for (int i = 1; i < editChunksList.Count; i++)
        {
            editChunksList[i].gameObject.SetActive(false);
        }
        Destroy(selectedSet);
    }
    public void ChangeSelectedChunk()
    {
        //disable current chunk
        editChunksList[visibleChunk].gameObject.SetActive(false);
        //goes up one chunk, but if at end of list it restarts
        if (visibleChunk == editChunksList.Count - 1)
        {
            visibleChunk = 0;
            editChunksList[visibleChunk].gameObject.SetActive(true);
        }
        else
        {
            visibleChunk++;
            editChunksList[visibleChunk].gameObject.SetActive(true);
        }
    }
    //quit editor and go to main menu
    public void SaveAndExit()
    {
        finishedSet = new GameObject();
        finishedSet.name = originalName;
        if (backupSetUsed)
        {
            finishedSet.transform.parent = GameObject.Find("BackUpSets").transform;
            foreach(Transform trans in editChunksList)
            {
                trans.transform.parent = finishedSet.transform;
            }
        }
        else
        {
            finishedSet.transform.parent = GameObject.Find("All Sets").transform;
            //arbitrary number for location
            int number = 1000;
            foreach (Transform trans in editChunksList)
            {
                //change chunks to be children of finished set
                trans.transform.parent = finishedSet.transform;
                trans.gameObject.SetActive(true);
                trans.position = new Vector3(number, 0, 0);
                number += 64;
            }
            //save all sets to be used in other scenes
            DontDestroyOnLoad(GameObject.Find("All Sets"));
        }
        finishedSet.AddComponent<ChunkHelper>();
        finishedSet.AddComponent<MapSetSettings>();
        finishedSet.GetComponent<MapSetSettings>().gravity = new Vector3(0, 9.81f, 0);

        //return everything to default settings
        posScript.NullifyObject();
        scaleScript.NullifyObject();
        tools.MakeFalse();

        MoveObject[] moveableObjects = FindObjectsOfType<MoveObject>();
        foreach (MoveObject move in moveableObjects)
        {
            move.enabled = false;
            move.Disable();
        }
        foreach (Transform trans in editChunksList)
        {
            trans.gameObject.SetActive(true);
        }

        //only one set can be save atm
        //will rework
        save.go = finishedSet;
        
        save.InitializeSave();
        //give time to save map
        Invoke("LoadMainMenu", 1f);
    }
    public void CreateNewChunk()
    {
        GameObject newChunk = new GameObject("NewChunk");
        newChunk.transform.parent = newSet.transform;
        newChunk.transform.position = new Vector3(0, 0, 0);
        newChunk.AddComponent<SpecialChunk>();

        //make new chunk visible
        editChunksList[visibleChunk].gameObject.SetActive(false);
        editChunksList.Add(newChunk.transform);
        visibleChunk = editChunksList.Count - 1;
        editChunksList[visibleChunk].gameObject.SetActive(true);
    }
    //used for newSet. need at least 1 chunk for everything to work
    public void AddChunk(Transform newChunk)
    {
        editChunksList.Add(newChunk);
    }
    public void ChangeOriginalName(string newName)
    {
        originalName = newName;
    }
    //returns active chunk
    public Transform ReturnActiveChunk()
    {
        return editChunksList[visibleChunk];
    }
    void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}