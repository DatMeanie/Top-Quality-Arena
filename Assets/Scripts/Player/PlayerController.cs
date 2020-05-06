using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    //playercontroller
    //able to walk, jump, crouch and change weapons

    //variables
    public int movementSpeed;
    public int jumpHeight;
    bool crouching = false;
    public bool equippedPrimary = true;
    public bool moving = false;

    //game objects
    public GameObject playerPerspectiveCamera;
    GameObject primaryWeapon;
    GameObject secondaryWeapon;

    //scripts, components
    EquippedWeapon eqwep;
    MapManager mapManager;
    Rigidbody rb;
    BoxCollider boxCol;

    private void Start()
    {
        //get components
        if (GameObject.Find("DataSaver"))
        {
            eqwep = GameObject.Find("EquippedWeapon").GetComponent<EquippedWeapon>();
            mapManager = GameObject.Find("MapManager").GetComponent<MapManager>();


            //get weapons
            primaryWeapon = GameObject.Find(eqwep.equippedPrimaryWeapon);
            secondaryWeapon = GameObject.Find(eqwep.equippedSecondaryWeapon);
        }
        else
        {
            primaryWeapon = GameObject.Find("Scar");
            secondaryWeapon = GameObject.Find("Glock18");
        }

        rb = GetComponent<Rigidbody>();
        boxCol = GetComponent<BoxCollider>();

        //change cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void FixedUpdate () {
        inputManager();
        //make player y rotation same as camera
        transform.rotation = Quaternion.Euler(0, playerPerspectiveCamera.transform.eulerAngles.y, 0);
	}

    void inputManager()
    {
        //basic movement
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward * movementSpeed * Time.deltaTime;
            moving = true;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.position -= transform.forward * movementSpeed * Time.deltaTime;
            moving = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += transform.right * movementSpeed * Time.deltaTime;
            moving = true;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.position -= transform.right * movementSpeed * Time.deltaTime;
            moving = true;
        }
        if(Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
        {
            StartCoroutine(notMoving());
        }
        //jump
        //must be near to ground
        if (Input.GetKeyDown(KeyCode.Space) && Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), 4.0f))
        {
            rb.velocity = new Vector3(0, jumpHeight, 0);
        }

        //change to primary weapon
        if (Input.GetKeyDown(KeyCode.Alpha1) && equippedPrimary == false)
        {
            //cant setactive(false) due to animations will glitch up
            //best to disable all components then putting weapon on location player will never see it
            if (secondaryWeapon.GetComponentInChildren<WeaponScript>().playingAnimation == false)
            {
                secondaryWeapon.GetComponentInChildren<WeaponScript>().enabled = false;
                secondaryWeapon.GetComponent<Animator>().enabled = false;
                primaryWeapon.GetComponentInChildren<WeaponScript>().enabled = true;
                primaryWeapon.GetComponent<Animator>().enabled = true;
                primaryWeapon.transform.localPosition = new Vector3(0, 0, 0);
                secondaryWeapon.transform.localPosition = new Vector3(0, 1000, 1000);
                equippedPrimary = true;

                if (GameObject.Find("DataSaver"))
                {
                    mapManager.ChangeWeapon(equippedPrimary);
                }
            }
        }
        //change to secondary weapon
        if (Input.GetKeyDown(KeyCode.Alpha2) && equippedPrimary == true)
        {
            //cant setactive(false) due to animations will glitch up
            //best to disable all components then putting weapon on location player will never see it
            if (primaryWeapon.GetComponentInChildren<WeaponScript>().playingAnimation == false)
            {
                primaryWeapon.GetComponentInChildren<WeaponScript>().enabled = false;
                primaryWeapon.GetComponent<Animator>().enabled = false;
                secondaryWeapon.GetComponentInChildren<WeaponScript>().enabled = true;
                secondaryWeapon.GetComponent<Animator>().enabled = true;
                secondaryWeapon.transform.localPosition = new Vector3(0, 0, 0);
                primaryWeapon.transform.localPosition = new Vector3(0, 1000, 1000);
                equippedPrimary = false;

                if (GameObject.Find("DataSaver"))
                {
                    mapManager.ChangeWeapon(equippedPrimary);
                }
            }
        }

        //change firemode
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (equippedPrimary)
            {
                primaryWeapon.GetComponentInChildren<WeaponScript>().ChangeMode();
            }
            else if (!equippedPrimary)
            {
                secondaryWeapon.GetComponentInChildren<WeaponScript>().ChangeMode();
            }
        }

        //crouch mode
        //will move slower
        //shorter boxcollider
        //player will have to move to make room for new collider
        if (Input.GetKeyDown(KeyCode.LeftControl) && crouching == false)
        {
            boxCol.size = new Vector3(2, 2, 2);
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.3f, transform.position.z);
            crouching = !crouching;
            movementSpeed -= 5;
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl) && crouching == true || Input.GetKeyDown(KeyCode.Space) && crouching == true)
        {
            boxCol.size = new Vector3(2, 3, 2);
            transform.position = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z);
            crouching = !crouching;
            movementSpeed += 5;
        }
    }

    IEnumerator notMoving()
    {
        yield return new WaitForSeconds(1f);
        moving = false;
    }
}
