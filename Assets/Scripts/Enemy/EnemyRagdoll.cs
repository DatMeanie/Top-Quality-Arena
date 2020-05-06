using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyRagdoll : MonoBehaviour {

    //script for enemy health and death

    public int enemyHealth;
    WeaponScript weaponScript;
    Rigidbody rb;
    string gameMode = "Coin Hunt";
    bool dead = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Initialize()
    {
        gameMode = GameObject.Find("MapManager").GetComponent<MapManager>().gameMode;
        weaponScript = GameObject.Find("Primary").GetComponentInChildren<WeaponScript>();
    }
    //weapon has changed
    //will load new data
    public void WeaponChanged(bool primary)
    {
        if (primary)
        {
            weaponScript = GameObject.Find("Primary").GetComponentInChildren<WeaponScript>();
        }
        else if (!primary)
        {
            weaponScript = GameObject.Find("Secondary").GetComponentInChildren<WeaponScript>();
        }
    }

    public void newHealth(int damage)
    {
        enemyHealth -= damage;
        //enemy dies
        if (enemyHealth <= 0 && dead == false)
        {
            Die();
        }
    }

    void Die()
    {
        //deactivate object
        Destroy(GetComponent<EnemyShooting>());
        gameObject.layer = 0;
        gameObject.tag = "Untagged";
        foreach (EnemyAim aim in transform.GetComponentsInChildren<EnemyAim>())
        {
            aim.enabled = false;
        }
        //fly back
        rb.AddRelativeForce(Vector3.back * weaponScript.impact, ForceMode.Impulse);
        dead = true;
        if (gameMode == "Enemy Massacre")
        {
            //one less enemy to kill
            if (GameObject.Find("CoinTakenText"))
            {
                GameObject.Find("CoinTakenText").GetComponent<Animator>().Play("CoinTaken");
            }
        }
        //gain exp
        if (GameObject.Find("Experience"))
        {
            GameObject.Find("Experience").GetComponent<ExperienceAndUnlocks>().changeExperience(10);
        }
        //make children fly away too
        Explode(transform);
    }
    void Explode(Transform trans)
    {
        foreach (Transform t in transform)
        {
            //not to waste resources on invisible objects
            t.SetParent(null);
            t.gameObject.AddComponent<Rigidbody>().AddRelativeForce(Vector3.back * weaponScript.impact, ForceMode.Impulse);
            Explode(t);
        }
    }
}