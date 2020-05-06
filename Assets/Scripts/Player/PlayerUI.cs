using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

    //script for all gameplay UI

    //coinnearby
	public GameObject nearbyText;
    //coins in range
    List<bool> coins = new List<bool>();

    //playerhealth
    public Text healthText;
    PlayerHealth playerHealth;
    
    //magazine UI
    public Text magazine;
    WeaponScript weaponScript;
    EquippedWeapon eqwep;

    //coin or enemy counter
    public Text counterText;
    SettingsForMap settings;
    string gameMode = "Coin Hunt";

    void Start()
    {
        playerHealth = GameObject.Find("Player").GetComponent<PlayerHealth>();
        //get gamemode
        if (GameObject.Find("DataSaver"))
        {
            eqwep = GameObject.Find("EquippedWeapon").GetComponent<EquippedWeapon>();
            settings = GameObject.Find("LevelConfig").GetComponent<SettingsForMap>();
            gameMode = settings.GameMode;
        }
    }

    private void Update()
    {
        //update text
        UpdateNearbyText();
        UpdateMagazineText();
        UpdateCounterText();
        healthText.text = playerHealth.health.ToString();
    }

    //coin entered trigger
    private void OnTriggerEnter(Collider other)
    {
        //coin needs to be not collected!!
        if(other.gameObject.tag == "Coin" && other.gameObject.GetComponent<MeshRenderer>().enabled == true)
        {
            coins.Add(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
                                                //coin needs to be not collected!!
        if (other.gameObject.tag == "Coin" && other.gameObject.GetComponent<MeshRenderer>().enabled == true)
        {
            try
            {
                coins.RemoveAt(coins.Count - 1);
            }
            catch
            {
                Debug.Log("Error: Coin list empty. Type 0");
            }
        }
    }

    //used in coin script
    public void RemoveCoin()
    {
        //remove coin from list and fix text
        try
        {
            coins.RemoveAt(coins.Count - 1);
        }
        catch
        {
            Debug.Log("Error: Coin list empty. Type 1");
        }
        nearbyText.SetActive(false);
    }

    void UpdateNearbyText()
    {
        //if there is a coin within range
        if (coins.Count > 0)
        {
            nearbyText.SetActive(true);
        }
        else
        {
            nearbyText.SetActive(false);
        }
    }
    void UpdateMagazineText()
    {
        //get current equipped weapon data
        if (GameObject.Find("DataSaver"))
        {
            if (GameObject.Find(eqwep.equippedPrimaryWeapon).GetComponentInChildren<WeaponScript>().enabled == true)
            {
                weaponScript = GameObject.Find(eqwep.equippedPrimaryWeapon).GetComponentInChildren<WeaponScript>();
            }
            else if (GameObject.Find(eqwep.equippedSecondaryWeapon).GetComponentInChildren<WeaponScript>().enabled == true)
            {
                weaponScript = GameObject.Find(eqwep.equippedSecondaryWeapon).GetComponentInChildren<WeaponScript>();
            }
        }
        else
        {
            if (GameObject.Find("Scar").GetComponentInChildren<WeaponScript>().enabled == true)
            {
                weaponScript = GameObject.Find("Scar").GetComponentInChildren<WeaponScript>();
            }
            else if (GameObject.Find("Glock18").GetComponentInChildren<WeaponScript>().enabled == true)
            {
                weaponScript = GameObject.Find("Glock18").GetComponentInChildren<WeaponScript>();
            }
        }
        
        //if there is no weaponscript
        if (weaponScript == null)
        {
            magazine.text = " ";
        }
        //if weapon is in use
        else
        {
            magazine.text = weaponScript.bulletsInMagazine.ToString() + " / " + weaponScript.magazineSize.ToString();
        }
    }
    void UpdateCounterText()
    {
        //update counter every frame based on gamemode
        if (gameMode == "Coin Hunt")
        {
            counterText.text = GameObject.FindGameObjectsWithTag("Coin").Length.ToString() + " coins left";
        }
        if (gameMode == "Enemy Massacre")
        {
            counterText.text = GameObject.FindGameObjectsWithTag("Enemy").Length.ToString() + " enemies left";
        }
    }
}
