using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    public List<Collider> colliderList = new List<Collider>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            colliderList.Add(other);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            colliderList.Remove(other);
        }
    }
}
