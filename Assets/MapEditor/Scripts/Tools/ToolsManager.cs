using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolsManager : MonoBehaviour {

    //tools for mapeditor

    //variables, gameobjects etc
    public GameObject gridLines;
    public Dropdown creatableObjectsDropDown;
    bool gridlinesEnabled = true;

    private void Start()
    {
        List<string> creatableobjects = new List<string>();
        foreach(Transform trans in GameObject.Find("CreatableObjects").transform)
        {
            creatableobjects.Add(trans.gameObject.name);
        }
        creatableObjectsDropDown.ClearOptions();
        creatableObjectsDropDown.AddOptions(creatableobjects);
        creatableObjectsDropDown.value = 0;
    }

    public void CreateNewObject()
    {
        //if currently editchunk exists
        if (GameObject.Find("SetCompiler").GetComponent<SetCompiler>().ReturnActiveChunk())
        {
            //instantiate from what is selected in dropdown
            GameObject newObject = Instantiate(GameObject.Find("CreatableObjects").transform.GetChild(creatableObjectsDropDown.value).gameObject);
            newObject.transform.position = new Vector3(0, 0, 0);
            newObject.transform.parent = GameObject.Find("SetCompiler").GetComponent<SetCompiler>().ReturnActiveChunk();
            if (newObject.GetComponent<Rigidbody>())
            {
                newObject.GetComponent<Rigidbody>().isKinematic = true;
            }
            newObject.AddComponent<MoveObject>();
            newObject.GetComponent<MoveObject>().InitializeScript();
        }
    }
    //change gridlines
    public void ChangeGridlineState()
    {
        gridlinesEnabled = !gridlinesEnabled;
        gridLines.SetActive(gridlinesEnabled);
    }
}
