using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllSets : MonoBehaviour {

    //singleton
    //used on object that has all sets as children

    public static AllSets control;

    private void Awake()
    {
        //singleton
        if (control == null)
        {
            control = this;
        }
        else if (control != this)
        {
            Destroy(gameObject);
        }
        //sets need to be carried through scenes
        DontDestroyOnLoad(gameObject);
    }
}
