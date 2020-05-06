using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsForMap : MonoBehaviour
{
    //settings for map
    public float difficulty = 0.5f;
    public int mapSize = 512;
    public string mapSet = "Original";
    public string gameMode = "Coin Hunt";

    public int MapSize
    {
        get
        {
            return mapSize;
        }

        set
        {
            mapSize = value;
        }
    }

    public string MapSet
    {
        get
        {
            return mapSet;
        }

        set
        {
            mapSet = value;
        }
    }

    public string GameMode
    {
        get
        {
            return gameMode;
        }

        set
        {
            gameMode = value;
        }
    }

    public float Difficulty
    {
        get
        {
            return difficulty;
        }

        set
        {
            difficulty = value;
        }
    }

    //reset values to default
    public void ResetValues()
    {
        Difficulty = 0.5f;
        MapSize = 512;
        MapSet = "Original";
        GameMode = "Coin Hunt";
    }
}
