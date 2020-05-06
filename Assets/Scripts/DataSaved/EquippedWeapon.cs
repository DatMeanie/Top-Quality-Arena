using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippedWeapon : MonoBehaviour {

    //stores equipment data

    //equipped weapons
    public string equippedPrimaryWeapon;
    public string equippedSecondaryWeapon;
    //if player has changed default weapons
    bool changedPrim = false;
    bool changedSec = false;
    //equipped weapon objects
    GameObject primWeapon;
    GameObject secWeapon;

    public void WeaponsChosen()
    {
        //change to default loadout if nothing chosen
        if(changedPrim == false)
        {
            equippedPrimaryWeapon = "Scar";
        }
        if (changedSec == false)
        {
            equippedSecondaryWeapon = "Glock18";
        }
        DontDestroyOnLoad(GameObject.Find("Weapons"));
    }
    public void SpawnWeapons()
    {
        //Find objects to use
        primWeapon = GameObject.Find(equippedPrimaryWeapon);
        secWeapon = GameObject.Find(equippedSecondaryWeapon);

        //transfer weapons to primary and secondary positions
        //make weapons active
        primWeapon.transform.parent = GameObject.Find("Primary").transform;
        primWeapon.GetComponentInChildren<WeaponScript>().enabled = true;
        primWeapon.GetComponentInChildren<WeaponScript>().Initialize();
        secWeapon.transform.parent = GameObject.Find("Secondary").transform;
        secWeapon.GetComponentInChildren<WeaponScript>().enabled = true;
        secWeapon.GetComponentInChildren<WeaponScript>().Initialize();
        //destroy other weapons
        Destroy(GameObject.Find("Weapons"));
    }

    //change weapons
    public void ChangeEquippedPrimaryWeapon(string newWeapon)
    {
        equippedPrimaryWeapon = newWeapon;
        changedPrim = true;
    }
    public void ChangeEquippedSecondaryWeapon(string newWeapon)
    {
        equippedSecondaryWeapon = newWeapon;
        changedSec = true;
    }
}
