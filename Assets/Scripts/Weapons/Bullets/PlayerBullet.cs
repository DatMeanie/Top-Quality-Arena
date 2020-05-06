using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour {

    Rigidbody rb;
    public int damage = 0;
    public bool defaultBullet;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddRelativeForce(0, 0, 100, ForceMode.Impulse);
        DeleteThisAfterSomeTime();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (defaultBullet)
        {
            DefaultBullet(other);
        }

    }
    public void DefaultBullet(Collider col)
    {
        if (col.tag == "Wall")
        {
            if (col.GetComponent<Rigidbody>() != null)
            {
                col.GetComponent<Rigidbody>().AddRelativeForce(Random.Range(0.0f, 5.0f), 0, Random.Range(0.0f, 5.0f), ForceMode.Impulse);
            }
            Destroy(gameObject);
        }
        else if (col.tag == "Enemy")
        {
            if (GameObject.Find("Primary").transform.GetChild(0).GetChild(0).GetComponent<WeaponScript>().enabled == true)
            {
                damage = GameObject.Find("Primary").transform.GetChild(0).GetChild(0).GetComponent<WeaponScript>().damage;
            }
            else if (GameObject.Find("Secondary").transform.GetChild(0).GetChild(0).GetComponent<WeaponScript>().enabled == true)
            {
                damage = GameObject.Find("Secondary").transform.GetChild(0).GetChild(0).GetComponent<WeaponScript>().damage;
            }
            col.GetComponent<EnemyRagdoll>().newHealth(damage);
            Destroy(gameObject);
        }
    }
    IEnumerator DeleteThisAfterSomeTime()
    {
        yield return new WaitForSeconds(7.0f);
        Destroy(gameObject);
    }
}
