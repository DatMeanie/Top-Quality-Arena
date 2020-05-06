using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YPosition : MonoBehaviour {

    //tool for positioning on Y axis

    float distance;
    Transform parentObject;
    Vector3 offset;
    void OnMouseDown()
    {
        //get parent object
        parentObject = transform.parent.GetComponent<PositionTool>().SendCurrentObject().transform;
        //distance from camera
        distance = Camera.main.WorldToScreenPoint(parentObject.transform.position).z;
        Vector3 posMove = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance));
        offset = parentObject.transform.position - posMove;
    }
    void OnMouseDrag()
    {
        Vector3 posMove = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance));
        parentObject.position = new Vector3(parentObject.position.x, posMove.y + offset.y, parentObject.position.z);
    }
}
