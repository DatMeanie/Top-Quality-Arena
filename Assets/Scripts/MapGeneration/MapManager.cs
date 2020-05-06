using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {

    //Mapmanager
    //create map
    //holds a lot of data
    //every map related function is here

    //scripts with necessary data
    SettingsForMap settings;
    ChunkHelper chunksHelperScript;
    EquippedWeapon equippedWeaponsScript;
    GameObject player;
    GameObject mapSet;

    //default variables
    float difficulty = 0.5f;
    public string gameMode = "Coin Hunt";
    public int mapSize = 512;
    public GameObject backupSet;
    void Awake()
    {
        if (GameObject.Find("DataSaver"))
        {
            settings = GameObject.Find("LevelConfig").GetComponent<SettingsForMap>();
            mapSize = settings.MapSize;
        }
        player = GameObject.Find("Player");
        DisableAllEnemyScripts();
    }
    void Start () {

        chunksHelperScript = backupSet.GetComponent<ChunkHelper>();
        mapSet = backupSet;

        //settings found, started from main menu
        if (GameObject.Find("DataSaver"))
        {
            //load in values, scripts, objects needed

            //destroy backupsets
            Destroy(backupSet);
            Destroy(GameObject.Find("BackUpPrimary"));
            Destroy(GameObject.Find("BackUpSecondary"));

            equippedWeaponsScript = GameObject.Find("EquippedWeapon").GetComponent<EquippedWeapon>();
            mapSet = GameObject.Find(settings.MapSet);
            Debug.Log(mapSet);
            chunksHelperScript = mapSet.GetComponent<ChunkHelper>();
            //make values same as settings for map
            difficulty = settings.Difficulty;
            gameMode = settings.GameMode;
            mapSize = settings.MapSize;

            equippedWeaponsScript.SpawnWeapons();
            //secondary weapon should not be active at start
            GameObject secWeapon = GameObject.Find(equippedWeaponsScript.equippedSecondaryWeapon);
            secWeapon.transform.localPosition = new Vector3(0, 1000, 1000);
            secWeapon.GetComponentInChildren<WeaponScript>().enabled = false;
            secWeapon.GetComponentInChildren<Animator>().enabled = false;

            Debug.Log("Found settings.");
        }
        else
        {
            GameObject.Find("Glock18").GetComponentInChildren<WeaponScript>().enabled = false;
            GameObject.Find("Glock18").GetComponent<Animator>().enabled = false;
            GameObject.Find("Glock18").transform.localPosition = new Vector3(0, 1000, 1000);
            Debug.Log("Error: SettingsForMap Not Found");
        }

        // map builds
        PlaceChunks(chunksHelperScript);
        Debug.Log("Placed chunks");
        FixAmountOfEnemiesFunction(difficulty);
        Debug.Log("Fixed enemies");
        FixAmountOfCoinsFunction();
        Debug.Log("Fixed coins");
        Invoke("StartAllScripts", 1f);
        Debug.Log("Enabled enemies");
        Invoke("RigidBodyActivator", 1f);
        if (GameObject.Find("LevelConfig"))
        {
            settings.ResetValues();
        }
        player.GetComponent<Rigidbody>().useGravity = true;
        // map has been built //

        //moveobject script is on every object
        //still run update even if disabled
        //bad performance
        RemoveMoveObjectScripts(mapSet);
    }

    void Update()
    {
        //disable enemy scripts when they are far away
        //DisableInactiveEnemies();
    }

    //place chunks in correct pos
    public void PlaceChunks(ChunkHelper chunks)
    {
        //positions on map
        List<Vector3> vectorList = new List<Vector3>();
        //completely random chunks
        List<Transform> randomChunksList = new List<Transform>();
        //chunks with specific info
        List<Transform> specialChunks = new List<Transform>();

        //get possible positions
        for (int i = 0; i < mapSize; i += 64)
        {
            for (int j = 0; j < mapSize; j += 64)
            {
                vectorList.Add(new Vector3(i, 0, j));
            }
        }
        //get chunks
        randomChunksList = chunks.ReturnChildren();

        //check for specialchunks and adds them to specialchunks list
        foreach (Transform chunk in randomChunksList)
        {
            //get component
            SpecialChunk specialChunkScript = chunk.GetComponent<SpecialChunk>();
            //export mapsize to chunk
            specialChunkScript.ChangeMapSize(mapSize);
            //get x and y pos
            specialChunkScript.GetPositionX();
            specialChunkScript.GetPositionY();

            //if x or y is specified, continue
            //if startchunk, continue
            //if specified random pos, continue
            if (specialChunkScript.GetPositionX() != 0 || specialChunkScript.GetPositionY() != 0 || specialChunkScript.startChunk == true || specialChunkScript.randomPosition)
            {
                specialChunks.Add(chunk);
                //if chunk has specified northSide chunk
                if (specialChunkScript.northSide != null)
                {
                    //loop for amount of specified northSide chunks to the north
                    for (int i = 0; i < specialChunkScript.ReturnNorthSideLenght() + 1; i++)
                    {
                        Transform northSideChunk = specialChunkScript.northSide.transform;
                        //if northSide is the same as specialchunk
                        if (specialChunkScript.northSide.name == specialChunkScript.gameObject.name)
                        {
                            northSideChunk = Instantiate(northSideChunk);
                        }
                        //if first northSide chunk
                        if (i == 0)
                        {
                            northSideChunk.GetComponent<SpecialChunk>().positionX = specialChunkScript.GetPositionX();
                            //north is chunk.y + 1
                            northSideChunk.GetComponent<SpecialChunk>().positionY = specialChunkScript.GetPositionY() + 1;
                            specialChunks.Add(northSideChunk);
                        }
                        //more than one northSide chunk
                        else
                        {
                            GameObject extension = Instantiate(northSideChunk.gameObject);
                            SpecialChunk extensionChunkScript = extension.GetComponent<SpecialChunk>();
                            extensionChunkScript.positionX = northSideChunk.GetComponent<SpecialChunk>().positionX;
                            //north is chunk.y + 1
                            extensionChunkScript.positionY = northSideChunk.GetComponent<SpecialChunk>().positionY + i;
                            extension.transform.position = new Vector3(extensionChunkScript.positionX, 0, extensionChunkScript.positionY) * 64;
                            specialChunks.Add(extension.transform);
                        }
                        Destroy(northSideChunk.gameObject);
                    }
                }
                //if chunk has specified northSide chunk
                if (specialChunkScript.eastSide != null)
                {
                    //loop for amount of specified eastSide chunks to the north
                    for (int i = 0; i < specialChunkScript.ReturnEastSideLenght() + 1; i++)
                    {
                        Transform eastSideChunk = specialChunkScript.eastSide.transform;
                        //if eastSide is the same as specialchunk
                        if (specialChunkScript.eastSide.name == specialChunkScript.gameObject.name)
                        {
                            eastSideChunk = Instantiate(eastSideChunk);
                        }
                        if (i == 0)
                        {
                            //east is chunk.x + 1
                            eastSideChunk.GetComponent<SpecialChunk>().positionX = specialChunkScript.GetPositionX() + i;
                            eastSideChunk.GetComponent<SpecialChunk>().positionY = specialChunkScript.GetPositionY();
                            specialChunks.Add(eastSideChunk);
                        }
                        //more than one eastSide chunk
                        else
                        {
                            GameObject extension = Instantiate(eastSideChunk.gameObject);
                            SpecialChunk extensionChunkScript = extension.GetComponent<SpecialChunk>();
                            //east is chunk.x + 1
                            extensionChunkScript.positionX = eastSideChunk.GetComponent<SpecialChunk>().positionX + i;
                            extensionChunkScript.positionY = eastSideChunk.GetComponent<SpecialChunk>().positionY;
                            extension.transform.position = new Vector3(extensionChunkScript.positionX, 0, extensionChunkScript.positionY) * 64;
                            specialChunks.Add(extension.transform);
                        }
                        Destroy(eastSideChunk.gameObject);
                    }
                }
            }
        }

        //remove already existing chunks and special chunks from randomChunksList
        foreach (Transform trans in specialChunks)
        {
            randomChunksList.Remove(trans);
        }
        //destroy chunks
        //avoid duplicates
        foreach (Transform trans in specialChunks)
        {
            Destroy(trans.gameObject);
        }
        foreach (Transform trans in randomChunksList)
        {
            Destroy(trans.gameObject);
        }
        Destroy(chunks.transform.parent.gameObject);

        //place chunks with positions established in vectorList
        foreach (Vector3 vec in vectorList)
        {
            bool newCheck = true;
            //go through specialchunks to see if one has same pos
            foreach (Transform chunk in specialChunks)
            {
                SpecialChunk specialChunkScript = chunk.GetComponent<SpecialChunk>();
                if (new Vector3(specialChunkScript.positionX, 0.0f, specialChunkScript.positionY) == vec / 64 && newCheck == true)
                {
                    //Debug.Log("placed " + chunk.name.ToString() + " from specialChunks at " + vec.ToString());
                    chunk.transform.parent = null;
                    chunk.transform.position = vec;
                    Instantiate(chunk);
                    newCheck = false;
                }
            }
            //no special chunk at pos, pick random
            if (newCheck == true)
            {
                Transform newChunk = randomChunksList[Random.Range(0, randomChunksList.Count)];
                //Debug.Log("placed random chunk at " + vec.ToString());
                newChunk.transform.parent = null;
                newChunk.transform.position = vec;
                newChunk.transform.rotation = Quaternion.Euler(newChunk.transform.eulerAngles.x, Random.Range(0, 5) * 90, newChunk.transform.eulerAngles.z);
                Instantiate(newChunk);
            }
        }
    }

    public void FixAmountOfCoinsFunction()
    {
        GameObject[] allCoinsArray = GameObject.FindGameObjectsWithTag("Coin");
        int safetyCounter = allCoinsArray.Length;
        if (gameMode == "Coin Hunt")
        {
            foreach (GameObject go in allCoinsArray)
            {
                if (Random.Range(0.0f, 1.0f) > difficulty && safetyCounter > 2)
                {
                    Destroy(go);
                    safetyCounter--;
                }
            }
        }
        else if (gameMode != "Coin Hunt")
        {
            foreach (GameObject go in allCoinsArray)
            {
                Destroy(go);
            }
        }

    }
    public void FixAmountOfEnemiesFunction(float difficulty)
    {
        GameObject[] allEnemiesArray = GameObject.FindGameObjectsWithTag("Enemy");
        int safetyCounter = allEnemiesArray.Length;
        foreach (GameObject go in allEnemiesArray)
        {
            if (Random.Range(0.0f, 1.0f) > difficulty && safetyCounter > 2)
            {
                Destroy(go);
                safetyCounter--;
            }
        }
    }
    public void DisableAllEnemyScripts()
    {
        GameObject[] listOfAllEnemies;
        listOfAllEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject go in listOfAllEnemies)
        {
            go.GetComponent<EnemyShooting>().enabled = false;
            if (go.GetComponent<EnemyRagdoll>())
            {
                go.GetComponent<EnemyRagdoll>().enabled = false;
            }
            else
            {
                go.GetComponentInChildren<EnemyRagdoll>().enabled = false;
            }
        }
        Debug.Log("Enemies Disabled");
    }
    public void StartAllScripts()
    {
        GameObject[] listOfAllEnemies;
        listOfAllEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject go in listOfAllEnemies)
        {
            go.GetComponent<EnemyShooting>().enabled = true;
            go.GetComponent<EnemyShooting>().Initialize();
            if (go.GetComponent<EnemyRagdoll>())
            {
                go.GetComponent<EnemyRagdoll>().enabled = true;
                go.GetComponent<EnemyRagdoll>().Initialize();
            }
            else
            {
                go.GetComponentInChildren<EnemyRagdoll>().enabled = true;
                go.GetComponentInChildren<EnemyRagdoll>().Initialize();
            }
            
        }
    }
    void RemoveMoveObjectScripts(GameObject go)
    {
        if (go.GetComponent<MoveObject>())
        {
            Destroy(go.GetComponent<MoveObject>());
        }
        foreach (Transform trans in go.transform)
        {
            if (trans.gameObject.GetComponent<MoveObject>())
            {
                Destroy(trans.gameObject.GetComponent<MoveObject>());
            }
            RemoveMoveObjectScripts(trans.gameObject);
        }
    }
    public void DisableInactiveEnemies()
    {
        //this function does impact performance quite a bit
        //enemies used to deactivate themselves in update function, but this has less impact on performance
        //will rework in future
        GameObject[] listOfAllEnemies;
        listOfAllEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject go in listOfAllEnemies)
        {
            EnemyShooting script = go.GetComponent<EnemyShooting>();
            if(Vector3.Distance(go.transform.position, player.transform.position) < script.enemyRange)
            {
                script.scriptEnabled = true;
            }
            else
            {
                script.scriptEnabled = false;
            }
        }
    }
    //rb bodies load in kinematic, otherwise they might bug out
    //this turns off kinematic
    void RigidBodyActivator()
    {
        Rigidbody[] rbObjects =  FindObjectsOfType<Rigidbody>();
        foreach (Rigidbody rb in rbObjects)
        {
            rb.isKinematic = false;
        }
    }
    //weapon has changed
    public void ChangeWeapon(bool primaryEquipped)
    {
        GameObject[] listOfAllEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject go in listOfAllEnemies)
        {
            if (go.GetComponent<EnemyRagdoll>())
            {
                go.GetComponent<EnemyRagdoll>().WeaponChanged(primaryEquipped);
            }
            else
            {
                go.GetComponentInChildren<EnemyRagdoll>().WeaponChanged(primaryEquipped);
            }
        }
    }
}
