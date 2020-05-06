using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour {

    //allows player to move objects if in right scene
    //object must have collider

    //tools
    PositionTool posScript;
    ScaleTool scaleScript;
    ObjectTools transformTools;

    //variables
    //selected object
    static GameObject activeObject;
    //if active
    static bool state = false;
    string toolMode = "position";
    public bool scriptEnabled = true;

    private void Start()
    {
        //setcompiler exists only in mapeditor
        //if not in map editor, scriptenabled = false
        if(GameObject.Find("SetCompiler") == false)
        {
            gameObject.GetComponent<MoveObject>().enabled = false;
            scriptEnabled = false;
        }
        else
        {
            state = false;
        }
    }
    private void Update()
    {
        //if script not enabled dont run any code
        if (!scriptEnabled) return;
        //change to position tool
        if (Input.GetKey(KeyCode.Alpha1) && activeObject == gameObject)
        {
            toolMode = "position";
            if(state)
            {
                posScript.ChangeSelectedGameObject(gameObject);
                scaleScript.NullifyObject();
            }
        }
        //change to size tool
        if (Input.GetKey(KeyCode.Alpha2) && activeObject == gameObject)
        {
            toolMode = "size";
            if (state)
            {
                scaleScript.ChangeSelectedGameObject(gameObject);
                posScript.NullifyObject();
            }
        }
        //delete object
        //nullify other scripts
        if (Input.GetKey(KeyCode.Delete) && activeObject == gameObject)
        {
            if (state)
            {
                scaleScript.NullifyObject();
                posScript.NullifyObject();
                transformTools.IsSomethingSelected = false;
                activeObject = null;
                state = false;
                Destroy(gameObject);
            }
        }
    }
    //when player clicks on object collider
    void OnMouseDown()
    {
        if (!scriptEnabled) return;
        if (state == false)
        {
            activeObject = gameObject;
            if (toolMode == "position")
            {
                posScript.ChangeSelectedGameObject(gameObject);
            }
            else if(toolMode == "size")
            {
                scaleScript.ChangeSelectedGameObject(gameObject);
            }
            transformTools.ActiveObject(activeObject);
            transformTools.IsSomethingSelected = true;
            state = true;
        }
        //deselect, tools go away
        //works if clicked on other object
        else if(state)
        {
            state = false;
            posScript.NullifyObject();
            scaleScript.NullifyObject();
            transformTools.IsSomethingSelected = false;
            activeObject = null;
        }
    }
    public void InitializeScript()
    {
        //find tools
        posScript = GameObject.Find("PositionTool").GetComponent<PositionTool>();
        scaleScript = GameObject.Find("ScaleTool").GetComponent<ScaleTool>();
        transformTools = GameObject.Find("UIScripts").GetComponent<ObjectTools>();
        state = false;
        scriptEnabled = true;
    }
    //disable script
    public void Disable()
    {
        scriptEnabled = false;
    }
}
