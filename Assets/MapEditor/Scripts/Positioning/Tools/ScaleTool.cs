using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleTool : MonoBehaviour {
    //script to send information to xScale, yScale and zScale scripts
    //also make position almost the same as the selected object

    GameObject selectedGameObject;
    private void Start()
    {
        NullifyObject();
    }
    private void Update()
    {
        if (selectedGameObject != null)
        {
            //tool at selectobj pos
            gameObject.transform.position = new Vector3(selectedGameObject.transform.position.x, selectedGameObject.transform.position.y + selectedGameObject.transform.localScale.y / 2, selectedGameObject.transform.position.z);
        }
    }
    //player selected new object
    public void ChangeSelectedGameObject(GameObject newObject)
    {
        selectedGameObject = newObject;
    }
    //player has unselected
    public void NullifyObject()
    {
        selectedGameObject = null;
        gameObject.transform.position = new Vector3(2500, 2500, 2500);
    }
    public GameObject SendCurrentObject()
    {
        return selectedGameObject;
    }
}
