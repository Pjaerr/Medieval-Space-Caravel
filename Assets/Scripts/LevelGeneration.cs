using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    public GameObject waterTile;

    [SerializeField] private Transform levelObjects;

    [HideInInspector] public float[] minMaxX = new float[2]; 
    [HideInInspector] public float[] minMaxY = new float[2]; 

    [Header("Left, Right, Up, Down Respectively")]
    [SerializeField] private Transform[] levelBoundaries = new Transform[4];


    void Start()
    {
        setMinMax();
        loadWaterTiles();
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
        for (int i = 0; i < minMaxX.Length; i++)
        {
            minMaxX[i] = levelBoundaries[i].position.x;
        }
        for (int i = 0; i < minMaxY.Length; i++)
        {
            minMaxY[i] = levelBoundaries[i + 2].position.y;
        }
    }

}
