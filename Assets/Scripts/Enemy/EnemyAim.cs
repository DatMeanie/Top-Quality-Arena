using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAim : MonoBehaviour {

    //always looks at player script
    public new bool enabled = true;
	void Update () {
        if (enabled)
        {
            transform.LookAt(Camera.main.transform);
        }
    }
}
