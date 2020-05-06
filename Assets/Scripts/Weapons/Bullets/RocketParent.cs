using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketParent : MonoBehaviour
{
    Rocket rocket;
    Rigidbody rb;
    public int damage = 0;

    void Start()
    {
        rocket = transform.parent.gameObject.GetComponent<Rocket>();
        rb = GetComponent<Rigidbody>();
        rb.AddRelativeForce(0, 0, 70, ForceMode.Impulse);
        DeleteThisAfterSomeTime();
    }
    private void OnTriggerEnter(Collider other)

    {  
        foreach (Collider col in rocket.colliderList)
        {
            if (Physics.Raycast(transform.position, col.transform.position, 16, 10))
            {
                if (GameObject.Find("MainGun"))
                {
                    damage = GameObject.Find("MainGun").GetComponent<WeaponScript>().damage;
                }
                col.gameObject.GetComponent<EnemyRagdoll>().newHealth(damage);
                col.gameObject.GetComponent<Rigidbody>().AddRelativeForce(Random.Range(0.0f, 5.0f), 0, Random.Range(0.0f, 5.0f), ForceMode.Impulse);
            }
        }

        if (other.GetComponent<Rigidbody>() != null)
        {
            other.GetComponent<Rigidbody>().AddRelativeForce(Random.Range(0.0f, 5.0f), 0, Random.Range(0.0f, 5.0f), ForceMode.Impulse);
        }

        if (other.tag == "Enemy")
        {
            if (GameObject.Find("MainGun"))
            {
                damage = GameObject.Find("MainGun").GetComponent<WeaponScript>().damage;
            }
            other.GetComponent<EnemyRagdoll>().newHealth(damage);
            Destroy(transform.parent.gameObject);
        }
        else if (other.tag == "Wall")
        {
            Destroy(transform.parent.gameObject);
        }
        
    }
    IEnumerator DeleteThisAfterSomeTime()
    {
        yield return new WaitForSeconds(7.0f);
        Destroy(transform.parent.gameObject);
    }
}
