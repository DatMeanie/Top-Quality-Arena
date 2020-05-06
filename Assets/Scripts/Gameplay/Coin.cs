using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

    //player go into coin
    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Player")
        {
            //if coin is active add experience and play coin taken
            if (gameObject.GetComponent<MeshRenderer>().enabled == true)
            {
                if (GameObject.Find("Experience"))
                {
                    GameObject.Find("Experience").GetComponent<ExperienceAndUnlocks>().changeExperience(5);
                }
                if (GameObject.Find("CoinTakenText"))
                {
                    GameObject.Find("CoinTakenText").GetComponent<Animator>().Play("CoinTaken");
                }
            }
            //disable coin
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.tag = "Untagged";
            //playerUI is on the first child of player object
            //check function in playerUI script
            other.gameObject.transform.GetChild(0).GetComponent<PlayerUI>().RemoveCoin();
        }
    }
}
