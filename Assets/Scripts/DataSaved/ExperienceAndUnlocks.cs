using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class ExperienceAndUnlocks : MonoBehaviour {

    //stores experience data

    //start with 200 experience
    //every weapon is 100 exp
    //player needs at least 2 weapons
    public int experience = 200;
	void Awake () {
        if (File.Exists(Application.persistentDataPath + "/playerProgression.dat"))
        {
            LoadData();
        }
        else
        {
            SaveData();
        }
    }
	
    //save experience
    public void SaveData()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerProgression.dat");
        ExperienceData experienceData = new ExperienceData();
        experienceData.experience = experience;

        bf.Serialize(file, experienceData);
        file.Close();
    }
    //load experience
    public void LoadData()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/playerProgression.dat", FileMode.Open);
        ExperienceData experienceData = (ExperienceData)bf.Deserialize(file);
        experience = experienceData.experience;
        file.Close();
    }


    public void changeExperience(int newExperience)
    {
        experience += newExperience;
    }
    public void ResetExperience()
    {
        experience = 200;
    }
}

//experience data
//will contain unlocks in future
[Serializable]
class ExperienceData {
    public int experience;
}

