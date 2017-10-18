using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    //Objects to be instantiated.
    public GameObject waterTile;
    public GameObject[] landMass; //Array containing references to prefabs of rocks, islands etc.

    //Hierarchy stuff.
    [SerializeField] private Transform levelObjects;

    //Reference Values
    private int numOfLandMass; //The number of land mass objects to spawn, changed depending upon the size of the level.
    [HideInInspector] public float[] minMaxX = new float[2]; //As far left and as far right as possible.
    [HideInInspector] public float[] minMaxY = new float[2]; //As far up and as far down as possible.
    [Header("Left, Top, Right, Bottom Respectively")]
    [SerializeField] private Transform[] levelBoundaries = new Transform[4];


    //Sets the min max to that of the level boundaries positions.
    void setMinMax(float[] positions)
    {
        for (int i = 0; i < minMaxX.Length; i++)
        {
            minMaxX[i] = positions[i];
            minMaxY[i] = positions[i + 2];
        }
    }

    /*Works out the area of the level, and then finds the nearest integer when that area is halved
    and divided by 1000 (to get a number <= 2 digits); */
    int getNumberOfLandmass()
    {
        int numToReturn = 1; //Default to 1 if area is not high enough. (< 2000);
        float height = minMaxY[0] - minMaxY[1];
        float width = minMaxX[1] - minMaxX[0];
        float areaOfLevel = height * width;

        if (areaOfLevel > 2000)
        {
            float num = (areaOfLevel / 2000);

            /*+ 0.5 or -0.5 depending on sign of number, will get the number closest to its nearest integer
            which casting as an int will then round up/down automatically*/
            numToReturn = (int)(num < 0 ? (num - 0.5) : (num + 0.5));
        }

        return numToReturn;
    }

    void generateBoundaries()
    {
        //Position and scale left boundary
        levelBoundaries[0].position = new Vector3(Random.Range(0, 80), 0, 10);
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
            //Go along the x axis.
            for (float j = minMaxX[0]; j < minMaxX[1]; j += 1.6f)
            {
                Instantiate(waterTile, new Vector3(j, i, 10), Quaternion.identity, levelObjects);
            }
        }
    }


    private List<Transform> spawnedObjects = new List<Transform>();

    void generateLandMass()
    {

        bool isRetrying = false;
        int arraySize = landMass.Length;
        numOfLandMass = getNumberOfLandmass();
        GameObject obj = landMass[Random.Range(0, arraySize)];
        Vector3 pos;

        for (int i = 0; i < numOfLandMass; i++)
        {
            int count = 0;

            if (!isRetrying) //If the last land mass was successfully placed, choose another at random.
            {
                obj = landMass[Random.Range(0, arraySize)];
            }
            
            pos = new Vector3(Random.Range(minMaxX[0], minMaxX[1]), Random.Range(minMaxY[0], minMaxY[1]), 10);
            AABB currentLandMass = new AABB(pos, obj.transform.localScale);

            for (int j = 0; j < spawnedObjects.Count; j++)
            {
                if (AABBHit(currentLandMass, new AABB(spawnedObjects[j].localPosition, spawnedObjects[j].localScale)) == false)
                {
                    count++;
                }
            }
           
            if (count == spawnedObjects.Count) //If all tests have been passed.
            {
                GameObject go = (GameObject)Instantiate(obj, pos, Quaternion.identity, levelObjects);
                isRetrying = false;
                spawnedObjects.Add(go.transform);
            }
            else
            {
                isRetrying = true;
                i--;
            }
        }
    }

    bool AABBHit(AABB obj1, AABB obj2)
    {

        if (Mathf.Max(obj1.x, obj1.x + obj1.width) >= Mathf.Min(obj2.x, obj2.x + obj2.width) &&
            Mathf.Min(obj1.x, obj1.x + obj1.width) <= Mathf.Max(obj2.x, obj2.x + obj2.width) &&
            Mathf.Max(obj1.y, obj1.y + obj1.height) >= Mathf.Min(obj2.y, obj2.y + obj2.height) &&
            Mathf.Min(obj1.y, obj1.y + obj1.height) <= Mathf.Max(obj2.y, obj2.y + obj2.height))
            {
                return true;
            }

        return false;
    }

    public void generateNewLevel()
    {
        generateBoundaries();
        generateLandMass();
        generateWaterTiles();
        
    }

    void Start()
    {
        generateNewLevel();
    }   
}

public struct AABB
{
    public float x, y;
    public float width, height;

    public AABB(Vector2 position, Vector2 localScale)
    {
        width = localScale.x;
        height = localScale.y;
        x = position.x - (width / 2);
        y = position.y + (height / 2);
    }

    public AABB(float x, float y, float width, float height)
    {
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;
    }
}
