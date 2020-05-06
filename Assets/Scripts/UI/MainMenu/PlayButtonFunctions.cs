using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class PlayButtonFunctions : MonoBehaviour {

    //play config functions

    SettingsForMap settings;
    EquippedWeapon equippedWeapons;

    public GameObject buttonsParent;
    public GameObject playConfig;
    public Text setChooserText;
    Vector3 originalPosition;

    public Text chooseMapSizeText;
    int mapSize = 512;
    public Text chooseDifficultyText;
    float difficulty = 0.5f;

    public Text chooseGameMode;
    string gameMode;

    //all mapsets and their data
    public RawImage showCase;
    List<Texture> setImages = new List<Texture>();
    List<string> mapSets = new List<string>();
    string mapSet;
    int mapCounter = 0;

    public GameObject screenArea;
    public GameObject progressionTab;
    public GameObject changeLog;
    Vector3 originalPositionForProgressionTab;

    private void Start()
    {
        //get components
        settings = GameObject.Find("LevelConfig").GetComponent<SettingsForMap>();
        equippedWeapons = GameObject.Find("EquippedWeapon").GetComponent<EquippedWeapon>();

        //change cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        //get originalpos
        originalPosition = buttonsParent.transform.position;
        originalPositionForProgressionTab = progressionTab.transform.position;
    }

    public void TransitionToPlay()
    {
        //animations
        progressionTab.transform.DOMoveY(playConfig.transform.position.y, 1.0f);
        changeLog.transform.DOMoveX(-1200, 2.0f).SetEase(Ease.OutCirc);
        buttonsParent.transform.DOMoveX(-400, 2.0f).SetEase(Ease.OutCirc);
        playConfig.transform.DOMoveY(originalPosition.y, 1.0f).SetEase(Ease.InCirc);

        //get all maps
        foreach (Transform trans in GameObject.Find("All Sets").transform)
        {
            if (trans.GetComponent<MapSetSettings>())
            {
                mapSets.Add(trans.gameObject.name);
                setImages.Add(trans.GetComponent<MapSetSettings>().preview);
            }
        }

        //defaults
        chooseMapSizeText.text = "Medium";
        chooseDifficultyText.text = "Normal";
        gameMode = settings.GameMode;
        setChooserText.text = mapSets[0];
        mapSet = mapSets[0];
        showCase.texture = setImages[0];
    }
    //three sizes
    public void ChangeMapSize()
    {
        if(chooseMapSizeText.text == "Medium")
        {
            chooseMapSizeText.text = "Large";
            mapSize = 1024;
        }
        else if (chooseMapSizeText.text == "Large")
        {
            chooseMapSizeText.text = "Small";
            mapSize = 256;
        }
        else if(chooseMapSizeText.text == "Small")
        {
            chooseMapSizeText.text = "Medium";
            mapSize = 512;
        }
    }
    //three difficulties
    public void ChangeDifficulty()
    {
        if (chooseDifficultyText.text == "Normal")
        {
            chooseDifficultyText.text = "Hard";
            difficulty = 1f;
            
        }
        else if (chooseDifficultyText.text == "Hard")
        {
            chooseDifficultyText.text = "Easy";
            difficulty = 0.3f;
        }
        else if (chooseDifficultyText.text == "Easy")
        {
            chooseDifficultyText.text = "Normal";
            difficulty = 0.5f;
        }
    }
    //change set
    public void ChangeSet()
    {
        if (setChooserText.text == mapSets[mapCounter])
        {
            mapCounter++;
            if (mapCounter == mapSets.Count)
            {
                mapCounter = 0;
            }
            setChooserText.text = mapSets[mapCounter];
            showCase.texture = setImages[mapCounter];
            mapSet = mapSets[mapCounter];
        }
    }
    //two gamemodes
    public void ChangeGameMode()
    {
        if (gameMode == "Coin Hunt")
        {
            chooseGameMode.text = "Enemy Massacre";
            gameMode = "Enemy Massacre";
        }
        else if (gameMode == "Enemy Massacre")
        {
            chooseGameMode.text = "Coin Hunt";
            gameMode = "Coin Hunt";
        }
    }
    //export data
    public void LoadLevel()
    {
        settings.MapSet = mapSet;
        settings.MapSize = mapSize;
        settings.GameMode = gameMode;
        settings.Difficulty = difficulty;
        equippedWeapons.WeaponsChosen();
    }
}
