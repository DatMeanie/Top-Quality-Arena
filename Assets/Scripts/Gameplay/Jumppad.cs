using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumppad : MonoBehaviour {

    //player enters triggerzone and flies into air
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            other.GetComponent<Rigidbody>().velocity = new Vector3(other.GetComponent<Rigidbody>().velocity.x, 0, other.GetComponent<Rigidbody>().velocity.z);
            other.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, 300, 0), ForceMode.Impulse);
        }
    }
}
