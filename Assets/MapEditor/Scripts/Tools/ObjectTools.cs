using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectTools : MonoBehaviour {

    bool isSomethingSelected = false;
    //currently selected object, from
    GameObject selectedObject;

    //Input from the inputfields on transform tools
    public InputField xPos;
    public InputField yPos;
    public InputField zPos;

    public InputField xRot;
    public InputField yRot;
    public InputField zRot;

    public InputField xScale;
    public InputField yScale;
    public InputField zScale;

    bool isInputFieldSelected = false;

    public bool IsSomethingSelected
    {
        get
        {
            return isSomethingSelected;
        }

        set
        {
            isSomethingSelected = value;
        }
    }

    private void Update()
    {
        CheckIfFocused();
        //if no inputfield is selected, will update transform on selected object
        if (IsSomethingSelected && isInputFieldSelected == false)
        {
            try
            {
                xPos.text = selectedObject.transform.position.x.ToString();
                yPos.text = selectedObject.transform.position.y.ToString();
                zPos.text = selectedObject.transform.position.z.ToString();

                xRot.text = selectedObject.transform.eulerAngles.x.ToString();
                yRot.text = selectedObject.transform.eulerAngles.y.ToString();
                zRot.text = selectedObject.transform.eulerAngles.z.ToString();

                xScale.text = selectedObject.transform.lossyScale.x.ToString();
                yScale.text = selectedObject.transform.lossyScale.y.ToString();
                zScale.text = selectedObject.transform.lossyScale.z.ToString();
            }
            catch
            {
                Debug.Log("Error: Can not update inputfields");
            }
        }
        //if inputfield is selected, able to change transform
        else if(IsSomethingSelected && isInputFieldSelected)
        {
            try
            {
                //limit for inputs, dont want game to crash
                if (float.Parse(xPos.text) <= 64 && float.Parse(xPos.text) >= -64 && float.Parse(yPos.text) <= 100 && float.Parse(yPos.text) >= -100 && float.Parse(zPos.text) <= 64 && float.Parse(zPos.text) >= -64)
                {
                    selectedObject.transform.position = new Vector3(float.Parse(xPos.text), float.Parse(yPos.text), float.Parse(zPos.text));
                }
                if (float.Parse(xRot.text) <= 360 && float.Parse(yRot.text) <= 360 && float.Parse(zRot.text) <= 360)
                {
                    selectedObject.transform.eulerAngles = new Vector3(float.Parse(xRot.text), float.Parse(yRot.text), float.Parse(zRot.text));
                }
                if (float.Parse(xScale.text) <= 64 && float.Parse(xScale.text) >= 1 && float.Parse(yScale.text) <= 100 && float.Parse(yScale.text) >= 1 && float.Parse(zScale.text) <= 64 && float.Parse(zScale.text) >= 1)
                {
                    selectedObject.transform.localScale = new Vector3(float.Parse(xScale.text), float.Parse(yScale.text), float.Parse(zScale.text));
                }
            }
            //if user inputs invalid number, example: 0, it cant be converted!
            catch
            {
                Debug.Log("Error: Can not convert");
            }
        }
    }

    public void CheckIfFocused()
    {
        //check every inputfield if they are in focus
        if (xPos.isFocused || yPos.isFocused || zPos.isFocused || xRot.isFocused || yRot.isFocused || zRot.isFocused || xScale.isFocused || yScale.isFocused || zScale.isFocused)
        {
            isInputFieldSelected = true;
        }
        else
        {
            isInputFieldSelected = false;
        }
    }
    public void MakeFalse()
    {
        IsSomethingSelected = false;
    }
    //used in MoveObject script
    public void ActiveObject(GameObject selectedObject)
    {
        this.selectedObject = selectedObject;
    }
}
