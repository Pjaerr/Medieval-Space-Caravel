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

    public void generateNewLevel()
   {
        generateBoundaries();
        generateWaterTiles();
        generateLandMass();
        setSpawnPoints();
   }

    void Start()
    {
        generateNewLevel();
    }   

    void setMinMax(float[] positions)
    {
        for (int i = 0; i < minMaxX.Length; i++)
        {
            minMaxX[i] = positions[i];
            minMaxY[i] = positions[i + 2];
        }
    }

    void generateBoundaries()
    {
        //Position and scale left boundary
        levelBoundaries[0].position = new Vector3(Random.Range(-10, 80), 0, 10);
        Vector2 leftBoundPos = levelBoundaries[0].position;
        levelBoundaries[0].localScale = new Vector3(1, Random.Range(90, 30), 0);
        Vector2 leftBoundScale = levelBoundaries[0].localScale;

        //Position and scale of top boundary.
        levelBoundaries[1].localScale = new Vector3(Random.Range(80, 200), 1, 0);
        Vector2 topBoundScale = levelBoundaries[1].localScale;
        levelBoundaries[1].position = new Vector3(leftBoundPos.x + (topBoundScale.x / 2), leftBoundScale.y / 2, 10);
        Vector2 topBoundPos = levelBoundaries[1].position;


        //Position and scale of right boundary
        levelBoundaries[2].position = new Vector3(topBoundScale.x + leftBoundPos.x, 0, 10);
        levelBoundaries[2].localScale = leftBoundScale;

        //Position and scale of bottom boundary
        levelBoundaries[3].position = new Vector3(topBoundPos.x, -(leftBoundScale.y / 2), 10);
        levelBoundaries[3].localScale = topBoundScale;

        setMinMax(new float[] {leftBoundPos.x, levelBoundaries[2].position.x, topBoundPos.y, levelBoundaries[3].position.y});
    }

     void generateWaterTiles()
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

    void generateLandMass()
    {

    }

    void setSpawnPoints()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            spawnPoints[i].position = new Vector3(Random.Range(minMaxX[0], minMaxX[1]),Random.Range(minMaxY[0], minMaxY[1]), 10);
        }
    }
}
