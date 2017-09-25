using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour 
{

	[Header("Min and Max Position for both X and Y respectively on which to spawn enemies at random.")]
	[SerializeField] private float[] minMaxX = new float[] {-20, 15};
	[SerializeField] private float[] minMaxY = new float[] {15, 9};

	[SerializeField] private int numberOfEnemies = 5;
	[SerializeField] private GameObject enemyPrefab;
	[HideInInspector] public int wave = 1;
	public static GameManager singleton = null;

	void Awake()
	{
		if (singleton == null)	//Check if an instance of GameManager already exists.
		{
			singleton = this; 	//If not, make this that instance.
		}
		else if (singleton != this)	//If an instance already exists and it isn't this.
		{
			Destroy(gameObject);	//Destroy this.
		}

		DontDestroyOnLoad(gameObject);
	}

	void Start()
	{
		spawnEnemies(numberOfEnemies);
	}

	void spawnEnemies(int numberOfEnemies)
	{
		for (int i = 0; i < numberOfEnemies; i++)
		{
			Instantiate(enemyPrefab, new Vector2(Random.Range(minMaxX[0], minMaxX[1]), Random.Range(minMaxY[0], minMaxY[1])), Quaternion.identity);
		}
	}
	
}
