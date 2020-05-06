using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSave : MonoBehaviour {
    //load map data
	void Start () {
        SaveManager save = GameObject.Find("SaveManager").GetComponent<SaveManager>();
        save.InitializeLoad();
	}
}
