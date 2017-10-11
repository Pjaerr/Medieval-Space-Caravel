using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    public GameObject waterTile;

    [SerializeField] private Transform levelObjects;

    [HideInInspector] public float[] minMaxX = new float[2]; 
    [HideInInspector] public float[] minMaxY = new float[2]; 

    [Header("Left, Top, Right, Bottom Respectively")]
    [SerializeField] private Transform[] levelBoundaries = new Transform[4];

    [SerializeField] private Transform[] spawnPoints = new Transform[5];


    void Start()
    {
        randomlyGenerateBoundaries();
        setMinMax();
        loadWaterTiles();
        setSpawnPoints();
        
    }   

    void loadWaterTiles()
    {
        /*Instantiate water tile at minMaxX[0] and instantiate another one at minMaxX[0] + 1.6f iterativley
        and increase a counter each time by 1.6f, when said counter reaches to more than or equal to the value of
        minMaxX[1], stop instantiating. Do this for every y position + 1.6f until all y positions have been filled.*/

        //Go down the y axis.
        for (float i = minMaxY[0]; i > minMaxY[1]; i -= 1.6f)
        {
            for (float j = minMaxX[0]; j < minMaxX[1]; j += 1.6f)
            {
                Instantiate(waterTile, new Vector3(j, i, 10), Quaternion.identity, levelObjects);
            }
        }
    }


    void setMinMax()
    {
        minMaxX[0] = levelBoundaries[0].position.x;
        minMaxX[1] = levelBoundaries[2].position.x;
        minMaxY[0] = levelBoundaries[1].position.y;
        minMaxY[1] = levelBoundaries[3].position.y;
    }

    void randomlyGenerateBoundaries()
    {
        //Position and scale left boundary
        levelBoundaries[0].position = new Vector3(Random.Range(-10, 80), 0, 10);
        levelBoundaries[0].localScale = new Vector3(1, Random.Range(90, 30), 0);

        //Position and scale of top boundary.
        levelBoundaries[1].localScale = new Vector3(Random.Range(80, 200), 1, 0);
        levelBoundaries[1].position = new Vector3(levelBoundaries[0].position.x + (levelBoundaries[1].localScale.x / 2), levelBoundaries[0].localScale.y / 2, 10);

        //Position and scale of right boundary
        levelBoundaries[2].position = new Vector3(levelBoundaries[1].localScale.x + levelBoundaries[0].position.x, 0, 10);
        levelBoundaries[2].localScale = levelBoundaries[0].localScale;

        //Position and scale of bottom boundary
        levelBoundaries[3].position = new Vector3(levelBoundaries[1].position.x, -(levelBoundaries[0].localScale.y / 2), 10);
        levelBoundaries[3].localScale = levelBoundaries[1].localScale;
    }

    void setSpawnPoints()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            spawnPoints[i].position = new Vector3(Random.Range(levelBoundaries[0].position.x, levelBoundaries[2].position.x),Random.Range(levelBoundaries[1].position.y, levelBoundaries[3].position.y), 10);
        }
    }
}
