using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;
using UnityEngine;


public class BoardManager : MonoBehaviour {

    public class Count
    {
        public int minimum;
        public int maximum;

        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }

    }

    public int columns  = 8;
    public int rows     = 8;
    public Count wallCount = new Count (5, 9);
    public Count foodCount = new Count(1, 5);
    public GameObject exit;
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject[] enemyTiles;
    public GameObject[] outerWallTiles;

    public Transform boardHolder;
    private List<Vector3> gridPositions = new List<Vector3>();

    public void InitialiseList()
    {
        gridPositions.Clear();
        for(int x = 1; x < columns - 1; ++x)
        {
            for(int y = 1; y < rows - 1; ++y)
            {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    void boardSetUp()
    {
        boardHolder = new GameObject("Board").transform;
        
        for(int x = -1; x < columns + 1; ++x)
        {   
            for(int y = -1; y < rows + 1; ++y)
            {
                GameObject toInstatiate = floorTiles[Random.Range(0, floorTiles.Length)];
                if(x == -1 || x == columns || y == -1 || y == rows)
                {
                     toInstatiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                }

                GameObject instance = Instantiate(toInstatiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
                instance.transform.SetParent(boardHolder);
            }
        }
    }

    Vector3 RandomPosition ()
    {
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);
        return randomPosition;
    }

    void LayOutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        int objectCount = Random.Range(minimum, maximum + 1);
        GameObject instance;

        for(int i = 0; i < objectCount; ++i)
        {
            Vector3 randomPosition = RandomPosition();
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            instance = Instantiate(tileChoice, randomPosition, Quaternion.identity);
            instance.transform.SetParent(boardHolder);
        }
    } 
    
    public void SetUpScene(int level)
    {
        GameObject instance;
        boardSetUp();
        InitialiseList();
        LayOutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
        LayOutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);
        int enemyCount = (int)Mathf.Log(level, 2f);
        LayOutObjectAtRandom(enemyTiles, enemyCount, enemyCount);
        instance = Instantiate(exit, new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity);

        instance.transform.SetParent(boardHolder);  
    }
}
