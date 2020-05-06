using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddMoveObject : MonoBehaviour {

    //all objects need a moveobject script to be moved in mapeditor
    //meant to save time when making maps in unity editor
    private void Start()
    {
        AddMoveObjectScript(gameObject.transform);
    }
    public void AddMoveObjectScript(Transform t)
    {
        foreach (Transform trans in t)
        {
            try
            {
                if(trans.gameObject.GetComponent<MoveObject>() == false && trans.gameObject.GetComponent<Collider>() == true)
                {
                    trans.gameObject.AddComponent<MoveObject>();
                }
                AddMoveObjectScript(trans);
            }
            catch
            {
                Debug.Log("Error: MoveObjectScrip Not Added, GameObject: " + trans.name);
            }
        }
    }
}
