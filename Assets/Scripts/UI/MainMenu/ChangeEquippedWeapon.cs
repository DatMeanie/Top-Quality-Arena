using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeEquippedWeapon : MonoBehaviour {

    //change weapon
    //buttons have same name as weapon

    //variables, components, gameobjects
    //name of currently used button
    string currentName;
    public GameObject changeStatusNotif;
    EquippedWeapon equippedWeaponScript;
    private void Start()
    {
        equippedWeaponScript = GameObject.Find("EquippedWeapon").GetComponent<EquippedWeapon>();
    }
    public void AskToChangeStatus()
    {
        //get current weaponbutton and ask what it should do
        currentName = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.parent.name;
        changeStatusNotif.SetActive(true);
    }

    //change weapon
    public void ChangeStatusToPrimary()
    {
        equippedWeaponScript.ChangeEquippedPrimaryWeapon(currentName);
        changeStatusNotif.SetActive(false);
    }
    public void ChangeStatusToSecondary()
    {
        equippedWeaponScript.ChangeEquippedSecondaryWeapon(currentName);
        changeStatusNotif.SetActive(false);
    }

    //cancel
    public void GoBack()
    {
        changeStatusNotif.SetActive(false);
    }
}
