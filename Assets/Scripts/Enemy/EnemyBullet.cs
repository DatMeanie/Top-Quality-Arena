using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour {

    //script for enemyBullets

    Rigidbody rb;

	public void Initialize () {
        //launch bullet forward
        rb = GetComponent<Rigidbody>();
        rb.AddRelativeForce(0, 0, 100, ForceMode.Impulse);
        DeleteThisAfterSomeTime();
	}
    private void OnTriggerEnter(Collider other)
    {
        //if hit player
        if (other.name == "Player")
        {
            //damage player
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            playerHealth.ChangeHealth(10);
            //make player fly in random direction, very minor
            other.GetComponent<Rigidbody>().AddRelativeForce(Random.Range(0.0f, 5.0f), 0, Random.Range(0.0f, 5.0f), ForceMode.Impulse);
        }
        //if hit wall
        else if(other.tag == "Wall")
        {
            //if has rigidbody, fly in random direction
            if (other.GetComponent<Rigidbody>() != null)
            {
                other.GetComponent<Rigidbody>().AddRelativeForce(Random.Range(0.0f, 5.0f), 0, Random.Range(0.0f, 5.0f), ForceMode.Impulse);
            }
            Destroy(gameObject);
        }
        
    }
    //bullets take up resources
    //need to be deleted after used
    IEnumerator DeleteThisAfterSomeTime()
    {
        yield return new WaitForSeconds(7.0f);
        Destroy(gameObject);
    }
}
