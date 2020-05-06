using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZScale : MonoBehaviour {

    //tool for scaling on Z axis

    Vector3 originalPos;
    Transform parentObject;
    private void OnMouseDown()
    {
        parentObject = transform.parent.GetComponent<ScaleTool>().SendCurrentObject().transform;
        originalPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
    }
    private void OnMouseDrag()
    {
        //cant be less than 0 and more than 100
        if (parentObject.localScale.z > 0 && parentObject.localScale.z < 101)
        {
            Vector3 objPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
            parentObject.localScale = new Vector3(parentObject.localScale.x, parentObject.localScale.y, parentObject.localScale.z - objPosition.x + originalPos.x);
        }
        //reset values
        if (parentObject.localScale.z <= 0)
        {
            parentObject.localScale = new Vector3(parentObject.localScale.x, parentObject.localScale.y, 1);
        }
        if (parentObject.localScale.z >= 100)
        {
            parentObject.localScale = new Vector3(parentObject.localScale.x, parentObject.localScale.y, 100);
        }
    }
}
