using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour {

    //makes enemy shoot at player after some time

    //player object
    GameObject player;

    //variables
    public float enemyTimeToShoot;
    public float enemyRange;
    float timer;
    public bool scriptEnabled = false;
    public AudioSource audio;

    //aim is a child and always looks at player
    //bullets shoot from it
    Transform aim;

    //bullet
    public GameObject templateBullet;

    //get values
    public void Initialize () {
        aim = transform.GetChild(0);
        timer = enemyTimeToShoot;
        templateBullet = GameObject.Find("epicNewEnemyBullet");
        player = GameObject.Find("Player");
        scriptEnabled = true;
    }
    
	void Update () {
        if(scriptEnabled)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        //if player is in range
        if (Vector3.Distance(transform.position, player.transform.position) < enemyRange)
        {
            //makes enemy look at player
            transform.LookAt(player.transform);
            //only y rot is changed, looks nicer
            transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
            if (timer <= 0)
            {
                try
                {
                    //launch bullet
                    GameObject newBullet = Instantiate(templateBullet, aim.transform.position, aim.transform.rotation);
                    try
                    {
                        newBullet.GetComponent<EnemyBullet>().Initialize();
                        if (audio != null)
                        {
                            audio.Play();
                        }
                    }
                    catch
                    {
                        Debug.Log("Error: Can not launch bullet. Gameobject : " + gameObject.name);
                    }
                }
                catch
                {
                    //get values and try again later
                    Initialize();
                    Debug.Log("Error: Enemybullet not found. Gameobject : " + gameObject.name);
                }
            }
            //reset timer
            if (timer <= 0)
            {
                timer = enemyTimeToShoot;
            }

            timer -= Time.deltaTime;
        }
    }
}
