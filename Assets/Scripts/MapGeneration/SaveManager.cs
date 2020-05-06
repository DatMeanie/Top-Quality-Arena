using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour {

    //Saving gameobject = saving all component values
    //unity cant save gameobjects, needs to be recreated instead

    List<Dictionary<int, List<float>>> objectList = new List<Dictionary<int, List<float>>>();

    //values to be saved
    List<List<string>> lists = new List<List<string>>();
    List<string> parentList = new List<string>();
    List<string> nameList = new List<string>();
    List<string> disabledList = new List<string>();
    List<string> setLists = new List<string>();
    //specific to chunks
    List<int> chunkSide = new List<int>();

    //IDs
    List<int> objectIDs = new List<int>();
    List<int> materialIDs = new List<int>();

    //All types of objects in the game
    List<GameObject> allObjects = new List<GameObject>();
    //All types of materials in the game
    List<GameObject> allMaterials = new List<GameObject>();

    //not saved
    string specialName;
    public GameObject go;
    public GameObject loadingScreen;
    List<GameObject> newObjects = new List<GameObject>();
    List<GameObject> chunks = new List<GameObject>();

    public void InitializeSave()
    {
        //find all types of objects and materials
        foreach (Transform trans in GameObject.Find("AllObjects").transform)
        {
            allObjects.Add(trans.gameObject);
        }
        foreach (Transform trans in GameObject.Find("AllMaterials").transform)
        {
            allMaterials.Add(trans.gameObject);
        }
        //create new name
        NewName();
        //save main object
        SaveObject(go);
        //save all children, children of children, etc
        SaveChildren(go.transform);
        //save every list
        SaveData();
    }
    public void InitializeLoad()
    {
        foreach(Transform trans in GameObject.Find("AllObjects").transform)
        {
            allObjects.Add(trans.gameObject);
        }
        foreach (Transform trans in GameObject.Find("AllMaterials").transform)
        {
            allMaterials.Add(trans.gameObject);
        }
        NewName();
        if (LoadData())
        {
            Debug.Log("Has data");
            DestroyAllObjects();
            Invoke("LoadObject", 1f);
            Invoke("ParentMaker", 2f);
            Invoke("SetChunkSides", 3f);
            Invoke("Disabler", 4f);
        }
        Invoke("LoadComplete", 5f);
    }
    public void SaveObject(GameObject go)
    {
        //set new special name
        //loading sets parents and for simplicity it is best if objects have unique name
        //mapsets and chunks are exception*
        if (go.GetComponent<MapSetSettings>() == false && go.GetComponent<AllSets>() == false)
        {
            NewName();
            go.name = specialName;
            nameList.Add(go.name);
        }
        else
        {
            nameList.Add(go.name);
        }
        //all components of object
        //unique IDs for components, see "Component List.txt"
        //component values are saved in list<float> format
        Dictionary<int, List<float>> components = new Dictionary<int, List<float>>();
        //transform is always added!
        List<float> trans = new List<float>();

        trans.Add(go.transform.position.x);
        trans.Add(go.transform.position.y);
        trans.Add(go.transform.position.z);
        trans.Add(go.transform.eulerAngles.x);
        trans.Add(go.transform.eulerAngles.y);
        trans.Add(go.transform.eulerAngles.z);
        trans.Add(go.transform.lossyScale.x);
        trans.Add(go.transform.lossyScale.y);
        trans.Add(go.transform.lossyScale.z);

        switch (go.tag)
        {
            case "Coin":
                trans.Add(0);
                break;
            case "Wall":
                trans.Add(1);
                break;
            case "Enemy":
                trans.Add(2);
                break;
            default:
                trans.Add(-1);
                break;
        }
        trans.Add(go.layer);
        //transform list includes if meshfilter and meshrenderer exist, ie not empty game object
        if(go.GetComponent<MeshRenderer>() == true) { trans.Add(1); } else { trans.Add(0); }
        if(go.GetComponent<MeshFilter>() == true) { trans.Add(1); } else { trans.Add(0); }
        components.Add(0, trans);

        //up to 4 box colliders
        int boxColliderCounter = 1;
        //goes through every component in object
        //unity default components or custom made scripts
        //save values of components to list
        //save valuelist to components list
        //cant use a switch (very sad!)
        foreach (Component comp in go.GetComponents<Component>())
        {
            if (comp is BoxCollider && boxColliderCounter < 5)
            {
                BoxCollider box = go.GetComponent<BoxCollider>();
                List<float> boxList = new List<float>();
                boxList.Add(box.center.x);
                boxList.Add(box.center.y);
                boxList.Add(box.center.z);
                boxList.Add(box.size.x);
                boxList.Add(box.size.y);
                boxList.Add(box.size.z);
                if(box.isTrigger == true) { boxList.Add(1); } else { boxList.Add(0); }
                components.Add(boxColliderCounter, boxList);
                boxColliderCounter++;
            }
            if (comp is SphereCollider)
            {
                SphereCollider sphere = go.GetComponent<SphereCollider>();
                List<float> sphereList = new List<float>();
                sphereList.Add(sphere.center.x);
                sphereList.Add(sphere.center.y);
                sphereList.Add(sphere.center.z);
                sphereList.Add(sphere.radius);
                components.Add(5, sphereList);
            }
            if (comp is MeshCollider)
            {
                MeshCollider mesh = go.GetComponent<MeshCollider>();
                List<float> meshList = new List<float>();
                float convex = 0;
                float isTrigger = 0;
                if(mesh.convex == true ) { convex = 1; } else { convex = 0; }
                meshList.Add(convex);
                if(mesh.isTrigger == true ) { isTrigger = 1; } else { isTrigger = 0; }
                meshList.Add(isTrigger);
                components.Add(10, meshList);
            }
            if (comp is Rigidbody)
            {
                Rigidbody rb = go.GetComponent<Rigidbody>();
                List<float> rbList = new List<float>();
                components.Add(11, rbList);
            }
            if (comp is MapSetSettings)
            {
                MapSetSettings set = go.GetComponent<MapSetSettings>();
                List<float> settingsList = new List<float>();
                settingsList.Add(set.gravity.x);
                settingsList.Add(set.gravity.y);
                settingsList.Add(set.gravity.z);
                components.Add(100, settingsList);
            }
            if (comp is ChunkHelper)
            {
                List<float> chunkHelperList = new List<float>();
                components.Add(101, chunkHelperList);
            }
            if (comp is SpecialChunk)
            {
                List<float> chunkList = new List<float>();
                SpecialChunk set = go.GetComponent<SpecialChunk>();
                if(set.startChunk == true) { chunkList.Add(1); } else { chunkList.Add(0); }
                if(set.lastTop == true) { chunkList.Add(1); } else { chunkList.Add(0); }
                if(set.lastRight == true) { chunkList.Add(1); } else { chunkList.Add(0); }
                chunkList.Add(set.positionX);
                chunkList.Add(set.positionY);

                chunkList.Add(set.northSideLength);
                if (set.northSideLoopOnMap == true) { chunkList.Add(1); } else { chunkList.Add(0); }
                chunkList.Add(set.eastSideLength);
                if (set.eastSideLoopOnMap == true) { chunkList.Add(1); } else { chunkList.Add(0); }

                if (set.randomPosition == true) { chunkList.Add(1); } else { chunkList.Add(0); }
                chunkList.Add(set.randomMinimumX);
                chunkList.Add(set.randomMaximumX);
                if (set.loopMaximumX == true) { chunkList.Add(1); } else { chunkList.Add(0); }
                chunkList.Add(set.randomMinimumY);
                chunkList.Add(set.randomMaximumY);
                if (set.loopMaximumY == true) { chunkList.Add(1); } else { chunkList.Add(0); }

                components.Add(102, chunkList);

                //add side chunks
                bool found = false;
                int northChildIndex = 0;
                if(set.northSide != null)
                {
                    foreach (Transform t in go.transform.parent)
                    {
                        if (t == set.northSide.transform)
                        {
                            found = true;
                            break;
                        }
                        northChildIndex++;
                    }
                }

                if (found)
                {
                    chunkSide.Add(northChildIndex);
                }
                else
                {
                    chunkSide.Add(-1);
                }

                found = false;
                int eastChildIndex = 0;
                if (set.eastSide != null)
                {
                    foreach (Transform t in go.transform.parent)
                    {
                        if (t == set.eastSide.transform)
                        {
                            found = true;
                            break;
                        }
                        eastChildIndex++;
                    }
                }
                if (found)
                {
                    chunkSide.Add(eastChildIndex);
                }
                else
                {
                    chunkSide.Add(-1);
                }

            }
            if (comp is AllSets)
            {
                List<float> allSetsList = new List<float>();
                components.Add(104, allSetsList);
            }
            if (comp is Jumppad)
            {
                List<float> jumpList = new List<float>();
                components.Add(200, jumpList);
            }
            if (comp is HealthKit)
            {
                List<float> healthList = new List<float>();
                components.Add(201, healthList);
            }
            if (comp is EnemyRagdoll)
            {
                EnemyRagdoll rag = go.GetComponent<EnemyRagdoll>();
                List<float> ragList = new List<float>();
                ragList.Add(rag.enemyHealth);
                components.Add(202, ragList);
            }
            if (comp is EnemyShooting)
            {
                EnemyShooting shoot = go.GetComponent<EnemyShooting>();
                List<float> shootList = new List<float>();
                shootList.Add(shoot.enemyTimeToShoot);
                shootList.Add(shoot.enemyRange);
                //bullets have ID, see "Bullets List.txt"
                shootList.Add(1);
                components.Add(203, shootList);
            }
            if (comp is EnemyAim)
            {
                List<float> aimList = new List<float>();
                components.Add(204, aimList);
            }
            if (comp is Coin)
            {
                List<float> coinList = new List<float>();
                components.Add(205, coinList);
            }
            
        }
        objectList.Add(components);
        //add what object's IDs corresponds with
        objectIDs.Add(MeshComparer(go));
        materialIDs.Add(MaterialComparer(go));

        //if object should be deactivated on game start
        //used to disable debug objects when playing
        if (go.activeSelf == false)
        {
            disabledList.Add(go.name);
        }
        //save parent or null
        if (go.transform.parent != null)
        {
            parentList.Add(go.transform.parent.name);
        }
        else
        {
            parentList.Add(null);
        }
    }

    public void LoadObject()
    {
        //for every object saved
        for (int i = 0; i < objectList.Count; i++)
        {
            //instantiate new object based on what objectID new object has, see "Object List.txt"
            GameObject newObject = new GameObject();
            if(objectIDs[i] >= 0)
            {
                //destroy empty game object
                Destroy(newObject);
                //create new object from template object
                newObject = Instantiate(GameObject.Find("AllObjects").transform.GetChild(objectIDs[i]).gameObject);
            }
            //if object has a material
            if(materialIDs[i] >= 0 && newObject.GetComponent<MeshRenderer>())
            {
                //get material from template material object
                newObject.GetComponent<Renderer>().material = GameObject.Find("AllMaterials").transform.GetChild(materialIDs[i]).gameObject.GetComponent<Renderer>().material;
            }
            //apply name
            newObject.name = nameList[i];
            //add MoveObject for use in mapeditor
            newObject.AddComponent<MoveObject>();
            //each pair is a component
            foreach (KeyValuePair<int, List<float>> comp in objectList[i])
                {
                //comp.key = component ID
                switch (comp.Key)
                {
                    //DEFAULT COMPONENTS
                    case 0:
                        List<float> gameTrans = comp.Value;
                        newObject.transform.position = new Vector3(gameTrans[0], gameTrans[1], gameTrans[2]);
                        newObject.transform.rotation = Quaternion.Euler(gameTrans[3], gameTrans[4], gameTrans[5]);
                        newObject.transform.localScale = new Vector3(gameTrans[6], gameTrans[7], gameTrans[8]);
                        switch ((int)gameTrans[9])
                        {
                            case 0:
                                newObject.tag = "Coin";
                                break;
                            case 1:
                                newObject.tag = "Wall";
                                break;
                            case 2:
                                newObject.tag = "Enemy";
                                break;
                            default:
                                break;
                        }
                        newObject.layer = (int)gameTrans[10];
                        //if meshfilter and renderer should NOT exist
                        if (gameTrans[11] == 0) { Destroy(newObject.GetComponent<MeshRenderer>()); }
                        if (gameTrans[12] == 0) { Destroy(newObject.GetComponent<MeshFilter>()); }
                        break;

                    case 5:
                        newObject.AddComponent<SphereCollider>();
                        List<float> placeHolder1 = comp.Value;
                        break;

                    case 10:
                        newObject.AddComponent<MeshCollider>();
                        List<float> placeHolder2 = comp.Value;
                        break;

                    case 11:
                        newObject.AddComponent<Rigidbody>();
                        //objects fly around at beginning, this is fix
                        //kinematic is turned off later
                        newObject.GetComponent<Rigidbody>().isKinematic = true;
                        break;

                    //MAP SCRIPTS
                    case 100:
                        List<float> setList = comp.Value;
                        newObject.AddComponent<MapSetSettings>();
                        newObject.GetComponent<MapSetSettings>().gravity = new Vector3(setList[0], setList[1], setList[2]);
                        break;

                    case 101:
                        newObject.AddComponent<ChunkHelper>();
                        break;

                    case 102:
                        List<float> chunkList = comp.Value;
                        newObject.AddComponent<SpecialChunk>();
                        SpecialChunk spc = newObject.GetComponent<SpecialChunk>();
                        if (chunkList[0] == 1) { spc.startChunk = true; }
                        if (chunkList[1] == 1) { spc.lastTop = true; }
                        if (chunkList[2] == 1) { spc.lastRight = true; }
                        spc.positionX = chunkList[3];
                        spc.positionY = chunkList[4];

                        spc.northSideLength = (int)chunkList[5];
                        if (chunkList[6] == 1) { spc.northSideLoopOnMap = true; }
                        spc.eastSideLength = (int)chunkList[7];
                        if (chunkList[8] == 1) { spc.eastSideLoopOnMap = true; }

                        if (chunkList[9] == 1) { spc.randomPosition = true; }
                        spc.randomMinimumX = (int)chunkList[10];
                        spc.randomMaximumX = (int)chunkList[11];
                        if (chunkList[12] == 1) { spc.loopMaximumX = true; }
                        spc.randomMinimumY = (int)chunkList[13];
                        spc.randomMaximumY = (int)chunkList[14];
                        if (chunkList[15] == 1) { spc.loopMaximumY = true; }

                        chunks.Add(newObject);
                        break;

                    case 104:
                        newObject.AddComponent<AllSets>();
                        break;

                    //GAMEPLAY SCRIPTS
                    case 200:
                        newObject.AddComponent<Jumppad>();
                        break;

                    case 201:
                        newObject.AddComponent<HealthKit>();
                        break;

                    case 202:
                        List<float> ragList = comp.Value;
                        newObject.AddComponent<EnemyRagdoll>();
                        newObject.GetComponent<EnemyRagdoll>().enemyHealth = 100;
                        break;

                    case 203:
                        List<float> shootList = comp.Value;
                        newObject.AddComponent<EnemyShooting>();
                        newObject.GetComponent<EnemyShooting>().enemyTimeToShoot = shootList[0];
                        newObject.GetComponent<EnemyShooting>().enemyRange = shootList[1];
                        //newObject.GetComponent<EnemyShooting>().templateBullet = GameObject.Find("EnemyBullet" + shootList[2].ToString());
                        break;

                    case 204:
                        newObject.AddComponent<EnemyAim>();
                        break;

                    case 205:
                        newObject.AddComponent<Coin>();
                        break;

                    default:
                        break;
                }

                //MULTIPLE COMPONENTS OF SAME TYPE
                if (comp.Key >= 1 && comp.Key < 5)
                {
                    List<float> boxCol = comp.Value;
                    BoxCollider newBox = newObject.AddComponent<BoxCollider>();
                    newBox.center = new Vector3(boxCol[0], boxCol[1], boxCol[2]);
                    newBox.size = new Vector3(boxCol[3], boxCol[4], boxCol[5]);
                    if (boxCol[6] == 1) { newBox.isTrigger = true; }
                }

            }
            //instantiated object has children it will not create them again
            foreach (Transform trans in newObject.transform)
            {
                i++;
            }
            //add newly created object
            newObjects.Add(newObject);
        }
    }

    void SaveData()
    {
        //binaryformatter to save lists and etc
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fileObjectList = File.Create(Application.persistentDataPath + "/MapData.dat");
        bf.Serialize(fileObjectList, objectList);
        fileObjectList.Close();
        FileStream fileObjectIDs = File.Create(Application.persistentDataPath + "/ObjectIDs.dat");
        bf.Serialize(fileObjectIDs, objectIDs);
        fileObjectIDs.Close();
        FileStream fileMaterialIDs = File.Create(Application.persistentDataPath + "/ObjectMaterialsIDs.dat");
        bf.Serialize(fileMaterialIDs, materialIDs);
        fileMaterialIDs.Close();
        FileStream fileChunkSides = File.Create(Application.persistentDataPath + "/ChunkSides.dat");
        bf.Serialize(fileChunkSides, chunkSide);
        fileChunkSides.Close();

        //add lists for more easier and more compact data saving
        lists.Add(parentList);
        lists.Add(nameList);
        lists.Add(disabledList);

        FileStream fileLists = File.Create(Application.persistentDataPath + "/Lists.dat");
        bf.Serialize(fileLists, lists);
        fileLists.Close();
    }
    bool LoadData()
    {
        //safety counter
        //might not have all lists
        int counter = 0;
        //binaryformatter to load all lists
        BinaryFormatter bf = new BinaryFormatter();
        if (File.Exists(Application.persistentDataPath + "/MapData.dat"))
        {
            FileStream file = File.Open(Application.persistentDataPath + "/MapData.dat", FileMode.Open);
            objectList = (List<Dictionary<int, List<float>>>)bf.Deserialize(file);
            file.Close();
            counter++;
        }
        if (File.Exists(Application.persistentDataPath + "/ObjectIDs.dat"))
        {
            FileStream file = File.Open(Application.persistentDataPath + "/ObjectIDs.dat", FileMode.Open);
            objectIDs = (List<int>)bf.Deserialize(file);
            file.Close();
            counter++;
        }
        if (File.Exists(Application.persistentDataPath + "/ObjectMaterialsIDs.dat"))
        {
            FileStream file = File.Open(Application.persistentDataPath + "/ObjectMaterialsIDs.dat", FileMode.Open);
            materialIDs = (List<int>)bf.Deserialize(file);
            file.Close();
            counter++;
        }
        if (File.Exists(Application.persistentDataPath + "/ChunkSides.dat"))
        {
            FileStream file = File.Open(Application.persistentDataPath + "/ChunkSides.dat", FileMode.Open);
            chunkSide = (List<int>)bf.Deserialize(file);
            file.Close();
            counter++;
        }
        if (File.Exists(Application.persistentDataPath + "/Lists.dat"))
        {
            FileStream file = File.Open(Application.persistentDataPath + "/Lists.dat", FileMode.Open);
            lists = (List<List<string>>)bf.Deserialize(file);
            parentList = lists[0];
            nameList = lists[1];
            disabledList = lists[2];
            file.Close();
            counter++;
        }
        //check if all lists exists
        if(counter == 5)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    //used to compare mesh with a template object
    //if it matches it sets object ID
    int MeshComparer(GameObject go)
    {
        int counter = 0;
        //-1 if null
        int position = -1;
        if (go.GetComponent<MeshFilter>() == true)
        {
            foreach (GameObject templateObject in allObjects)
            {
                if (go.GetComponent<MeshFilter>().sharedMesh == templateObject.GetComponent<MeshFilter>().sharedMesh)
                {
                    //ID for object
                    position = counter;
                    break;
                }
                counter++;
            }
        }
        return position;
    }
    //used to compare material with a template object
    //if it matches it sets material ID
    int MaterialComparer(GameObject go)
    {
        int counter = 0;
        //-1 if null
        int position = -1;
        if (go.GetComponent<Renderer>()== true)
        {
            foreach (GameObject templateObject in allMaterials)
            {
                if (go.GetComponent<Renderer>().sharedMaterial == templateObject.GetComponent<Renderer>().sharedMaterial)
                {
                    //ID for object
                    position = counter;
                }
                counter++;
            }
        }
        return position;
    }
    //sets parent of new objects
    void ParentMaker()
    {
        for (int i = 0; i < newObjects.Count; i++)
        {
            if(i == parentList.Count)
            {
                break;
            }
            if (parentList[i] != null)
            {
                if (GameObject.Find(parentList[i]))
                {  
                    //make child
                    newObjects[i].transform.parent = GameObject.Find(parentList[i]).transform; 
                }
            }
        }
    }
    //chunks can have connected sides
    //set sides with this function
    void SetChunkSides()
    {
        //for chunkSide list
        int counter = 0;

        foreach(GameObject chunk in chunks)
        {
            SpecialChunk script = chunk.GetComponent<SpecialChunk>();
            //chunks can have special chunk at either north or east side
            //chunkSide list has therefore double amount of values
            //counter goes up two for every chunk
            if(chunkSide[counter] != -1)
            {
                script.northSide = script.transform.parent.GetChild(chunkSide[counter]).gameObject;
            }
            counter++;
            if (chunkSide[counter] != -1)
            {
               script.eastSide = script.transform.parent.GetChild(chunkSide[counter]).gameObject;
            }
            counter++;
        }
    }
    //some objects are disabled from start
    void Disabler()
    {
        foreach(string str in disabledList)
        {
            if (GameObject.Find(str))
            {
                GameObject.Find(str).SetActive(false);
            }
        }
    }

    public void SaveChildren(Transform t)
    {
        foreach (Transform trans in t)
        {
            NewName();
            //saving might mess up if testing new features or there is bug
            try
            {
                SaveObject(trans.gameObject);
                SaveChildren(trans);
            }
            catch
            {
                Debug.Log("Error: ObjectSave, GameObject: " + trans.name);
            }
        }
    }
    //destroy all present objects
    public void DestroyAllObjects()
    {
        foreach(string str in nameList)
        {
            if (GameObject.Find(str))
            {
                Destroy(GameObject.Find(str));
            }
        }
    }
    //create new name for object
    public void NewName()
    {
        while (true)
        {
            string letters = "abcdefghijklmnopqrstuvwxyz0123456789";
            specialName = null;
            int lenght = 10;
            bool notExist = true;
            for (int i = 0; i < lenght; i++)
            {
                specialName += letters[Random.Range(0, letters.Length)];
            }
            //if it doesnt exist, break
            foreach(string str in nameList)
            {
                if(specialName == str)
                {
                    notExist = false;
                }
            }
            if( notExist) { break; }
        }
    }
    public void LoadComplete()
    {
        Debug.Log("Load Complete");
        loadingScreen.SetActive(false);
    }
    //reset maps
    public void DestroyData()
    {
        try
        {
            File.Delete(Application.persistentDataPath + "/MapData.dat");
            File.Delete(Application.persistentDataPath + "/ObjectIDs.dat");
            File.Delete(Application.persistentDataPath + "/ObjectMaterialsIDs.dat");
            File.Delete(Application.persistentDataPath + "/ChunkSides.dat");
            File.Delete(Application.persistentDataPath + "/Lists.dat");
        }
        catch
        {
            Debug.Log("Error: Could not delete map data");
        }

        //reload scene
        loadingScreen.SetActive(true);
        SceneManager.LoadScene(0);
    }
}