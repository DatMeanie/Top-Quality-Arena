using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveObjectsThatEnter : MonoBehaviour {

    //remove all objects that enter trigger zone

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Player")
        {
            other.gameObject.GetComponent<PlayerHealth>().ChangeHealth(10000);
        }
        else
        {
            Destroy(other.gameObject);
        }
    }
}
