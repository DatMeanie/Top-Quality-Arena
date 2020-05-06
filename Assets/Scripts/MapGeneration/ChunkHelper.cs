using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkHelper : MonoBehaviour {

    //return all children of object

    public List<Transform> ReturnChildren()
    {
        List<Transform> childrenList = new List<Transform>();
        foreach (Transform child in transform)
        {
            childrenList.Add(child);
        }
        return childrenList;
    }
}
