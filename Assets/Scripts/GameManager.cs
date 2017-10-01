using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour 
{
	private UI gUI;

	[Header("Min and Max Position for both X and Y respectively on which to spawn enemies at random.")]
	[SerializeField] private float[] minMaxX = new float[] {-20, 15};
	[SerializeField] private float[] minMaxY = new float[] {15, 9};

	[HideInInspector] public int numberOfEnemies = 2;
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
		gUI = GetComponent<UI>();
		spawnEnemies(numberOfEnemies);
	}

	void Update()
	{
		if (Time.frameCount % 50 == 0) //Every 50th frame.
		{
			if (numberOfEnemies <= 0)
			{
				StartCoroutine(newWave());
			}
		}
	}

	IEnumerator newWave()
	{
		wave++;
		numberOfEnemies = wave * 2;
		gUI.updateNumberOfWaves(wave);
		yield return new WaitForSeconds(3);
		spawnEnemies(numberOfEnemies);
	}

	void spawnEnemies(int numberOfEnemies)
	{
		for (int i = 0; i < numberOfEnemies; i++)
		{
			Instantiate(enemyPrefab, new Vector2(Random.Range(minMaxX[0], minMaxX[1]), Random.Range(minMaxY[0], minMaxY[1])), Quaternion.identity);
		}
	}

	public void LookAtPosition(Transform thisTransform, Vector3 targetPos, float rotationSpeed)
	{
		float angle = Mathf.Atan2(targetPos.y - thisTransform.position.y, 
		targetPos.x - thisTransform.position.x) * Mathf.Rad2Deg;
		thisTransform.rotation = Quaternion.AngleAxis(angle + 90, new Vector3(0, 0, rotationSpeed));
	}
	
}
