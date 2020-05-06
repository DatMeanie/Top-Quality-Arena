using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthKit : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        //if player enters collider
        if(other.gameObject.name == "Player")
        {
            //change health to full 100
            other.gameObject.GetComponent<PlayerHealth>().health = 100;
            //destroy healthkit
            Destroy(gameObject);
        }
    }
}
