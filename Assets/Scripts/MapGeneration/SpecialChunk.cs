using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpecialChunk : MonoBehaviour {

    //data for specific chunks
    //even chunks with no info has this script

    //variables, sidechunks
    [Header("Position and Connected Chunks")]
    public float positionX;
    public float positionY;
    public int northSideLength;
    public bool northSideLoopOnMap;
    public GameObject northSide;
    public int eastSideLength;
    public bool eastSideLoopOnMap;
    public GameObject eastSide;
    public bool startChunk;

    //position variables
    [Header("Randomized Position")]
    public bool randomPosition;
    public int randomMinimumX;
    public int randomMaximumX;
    public bool loopMaximumX;
    bool randomizedPosXComplete = false;
    public int randomMinimumY;
    public int randomMaximumY;
    public bool loopMaximumY;
    bool randomizedPosYComplete = false;
    public bool lastRight;
    public bool lastTop;

    //default size
    int mapSize = 512 / 64;

    public float GetPositionX()
    {
        if (randomPosition == true && randomizedPosXComplete == false)
        {
            //loop maximumX on mapSize
            if (loopMaximumX)
            {
                positionX = Random.Range(randomMinimumX, mapSize);
            }
            else
            {
                positionX = Random.Range(randomMinimumX, randomMaximumX);
            }

            randomizedPosXComplete = true;
        }
        else if(lastRight)
        {
            positionX = mapSize - 1;
        }
        
        return positionX;
    }
    public float GetPositionY()
    {
        if (randomPosition == true && randomizedPosYComplete == false)
        {
            //loop maximumY on mapSize
            if (loopMaximumY)
            {
                positionY = Random.Range(randomMinimumY, mapSize);
            }
            else
            {
                positionY = Random.Range(randomMinimumY, randomMaximumY);
            }
            randomizedPosYComplete = true;
        }
        else if (lastTop)
        {
            positionY = mapSize - 1;
        }
        return positionY;
    }
    public int ReturnNorthSideLenght()
    {
        //loop northSide on map
        if (northSideLoopOnMap)
        {
            int newLength = Mathf.RoundToInt(GetPositionY());
            northSideLength = mapSize - newLength - 1;
            return northSideLength;
        }
        else
        {
            return northSideLength;
        }
    }
    public int ReturnEastSideLenght()
    {
        //loop eastSide on map
        if (eastSideLoopOnMap)
        {
            int newLength = Mathf.RoundToInt(GetPositionX());
            eastSideLength = mapSize - newLength - 1;
            return eastSideLength;
        }
        else
        {
            return eastSideLength;
        }
    }
    public void ChangeMapSize(int mapSize)
    {
        this.mapSize = mapSize / 64;
    }
}