using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XScale : MonoBehaviour {

    //tool for scaling on X axis

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
        if (parentObject.localScale.x > 0 && parentObject.localScale.x < 101)
        {
            Vector3 objPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
            parentObject.localScale = new Vector3(parentObject.localScale.x - objPosition.x + originalPos.x, parentObject.localScale.y, parentObject.localScale.z);
        }
        //reset values
        if (parentObject.localScale.x <= 0)
        {
            parentObject.localScale = new Vector3(1, parentObject.localScale.y, parentObject.localScale.z);
        }
        if (parentObject.localScale.x >= 100)
        {
            parentObject.localScale = new Vector3(100, parentObject.localScale.y, parentObject.localScale.z);
        }
    }
}
