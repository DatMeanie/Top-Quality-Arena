using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyPaste : MonoBehaviour {

    //unfinished script
    //player will be able to copy paste objects
    //not used ingame currently

    GameObject selectedObject;
    bool isCreated = false;
	public void ChangeSelectedObject(GameObject newObject)
    {
        selectedObject = newObject;
    }
    public void MakeNewObject()
    {
        Instantiate(selectedObject, new Vector3(0, 0, 0), Quaternion.Euler(0,0,0));
    }
    public void ChangeState(bool newState)
    {
        isCreated = newState;
    }
    public bool ReturnState()
    {
        return isCreated;
    }
}
