using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speedpad : MonoBehaviour {

    //player enters triggerzone and speed changes
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            StartCoroutine(ChangeSpeed(other.gameObject.GetComponent<PlayerController>()));
        }
    }
    IEnumerator ChangeSpeed(PlayerController player)
    {
        player.movementSpeed += 10;
        yield return new WaitForSeconds(2);
        player.movementSpeed -= 10;
    }
}
