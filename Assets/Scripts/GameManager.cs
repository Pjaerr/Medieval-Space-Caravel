using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour 
{
	private UI gUI;

	public AudioSource fireSound;

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
				StartCoroutine(newWave()); //Starts a new wave.
			}
		}
	}

	//Ends the game, showing relevant game screen whether the player has won or not.
	public void endGame(bool playerHasWon)
	{
		gUI.generalUI.SetActive(false);

		if (playerHasWon)
		{
			gUI.youWinScreen.SetActive(true);
		}
		else
		{
			gUI.youLoseScreen.SetActive(true);
		}

		Time.timeScale = 0.3f;
	}

	/*Starts a new wave, meant to be called via a coroutine. Will increase the wave count, and make the next set of enemies
	equal to the wave number multiplied by 2. Will wait 3 seconds and then spawn said enemies.*/
	IEnumerator newWave()
	{
		if (wave >= 15)
		{
			endGame(true);
		}

		wave++;
		numberOfEnemies = wave * 2;
		gUI.updateNumberOfWaves(wave);
		yield return new WaitForSeconds(3);
		spawnEnemies(numberOfEnemies);
	}

	/*Spawns enemies randomly between minmaxX and Y positions respectivley.*/
	void spawnEnemies(int numberOfEnemies)
	{
		for (int i = 0; i < numberOfEnemies; i++)
		{
			Instantiate(enemyPrefab, new Vector2(Random.Range(minMaxX[0], minMaxX[1]), Random.Range(minMaxY[0], minMaxY[1])), Quaternion.identity);
		}
	}

	/*Will rotate object 'thisTransform' by degrees pointing in the direction of object 'targetPos' from position of 'thisTransform'
	utilises Mathf.Atan2.*/
	public void LookAtPosition(Transform thisTransform, Vector3 targetPos, float rotationSpeed)
	{
		float angle = Mathf.Atan2(targetPos.y - thisTransform.position.y, 
		targetPos.x - thisTransform.position.x) * Mathf.Rad2Deg;
		thisTransform.rotation = Quaternion.AngleAxis(angle + 90, new Vector3(0, 0, rotationSpeed));
	}
	
}
