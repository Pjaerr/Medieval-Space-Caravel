using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour 
{
	enum Type {Melee, Ranged};

	[SerializeField] private Type enemyType = Type.Melee;

	private Transform trans;

	private SpriteRenderer spriteRenderer;

	//Player References
	private GameObject playerObject;
	private PlayerController playerController;
	private Transform playerTransform;

	[SerializeField] private GameObject projectile;
	[SerializeField] private Transform projectileLaunchPoint;


	[SerializeField] private float movementSpeed = 0.1f;
	[SerializeField] private float rotationSpeed = 1;

	[Range(0.0f, 100.0f)]
	[SerializeField] private float range = 20.0f;
	[SerializeField] private int damage = 1;
	[SerializeField] private int lives = 5;

	private bool cooldownHasEnded = true;
	[SerializeField] private float cooldownTime = 2f;

	void Start()
	{
		//Assign the damage and lives of this enemy to the current wave number to provide level of abritry difficulty.
		damage = GameManager.singleton.wave;
		lives = GameManager.singleton.wave;

		trans = GetComponent<Transform>();
		spriteRenderer = GetComponent<SpriteRenderer>();

		//Load the player references once when the object starts.
		playerObject = GameObject.FindWithTag("Player");
		playerController = playerObject.GetComponent<PlayerController>();
		playerTransform = playerObject.GetComponent<Transform>();


		//Randomly assign values.
		enemyType = (Type)Random.Range(0, 2);
		movementSpeed = Random.Range(0.5f, 3.0f);
		cooldownTime = Random.Range(0.5f, 4.0f);
		range = Random.Range(5.0f, 60.0f);
	}

	void Update()
	{
		if (enemyType == Type.Melee)
		{
			followPlayer();
		}
		else if (enemyType == Type.Ranged)
		{
			randomlyPatrol();
		}
	}

	//Call this function when a collision happens betwen this object and the player, or this object's projectile and the player.
	void dealDamage()
	{
		playerController.deductLives(damage);
	}

	//Call this function when a collision happens between this object and a player projectile.
	void takeDamage()
	{
		StartCoroutine(flashSprite());
		lives -= playerController.damage;
		isDead();
	}
	IEnumerator flashSprite()
    {
        spriteRenderer.enabled = false;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.enabled = true;
    }

	void isDead()
	{
		if (lives <= 0)
		{
			GameManager.singleton.numberOfEnemies--;
			Destroy(gameObject);
		}
	}

	void followPlayer()
	{
		//Rotate to look at the player.
		GameManager.singleton.LookAtPosition(trans, playerTransform.position, rotationSpeed);

		//Move towards the player at the given movementSpeed.
		float step = movementSpeed * Time.deltaTime;
		trans.position = Vector3.MoveTowards(trans.position, playerTransform.position, step);
	}

	private bool currentlyPatrolling = false;
	private Vector3 patrolPoint = new Vector3(0, 0, 10);

	void randomlyPatrol()
	{
		float step = movementSpeed * Time.deltaTime;

		if (!currentlyPatrolling)
		{
			currentlyPatrolling = true;
			patrolPoint = new Vector3(Random.Range(-21, 13), Random.Range(13, -13), 10);
		}

		if (canSeePlayer())
		{
			GameManager.singleton.LookAtPosition(trans, playerTransform.position, rotationSpeed);
			if (cooldownHasEnded)
			{
				StartCoroutine(shootAtPlayer());
			}
		}
	

		if (trans.position != patrolPoint)
		{
			GameManager.singleton.LookAtPosition(trans, patrolPoint, rotationSpeed);
			trans.position = Vector3.MoveTowards(trans.position, patrolPoint, step);
		}
		else
		{
			currentlyPatrolling = false;
		}
		

	}

	bool canSeePlayer()
	{
		Vector3 offset = playerTransform.position - trans.position;
		float distance = offset.sqrMagnitude * 0.1f; 

		if (distance < range)
		{
			Debug.DrawLine(trans.position, playerTransform.position, Color.red);
			return true;
		}
		
		return false;
	}

	/*Gets called via a Coroutine, will fire a projectile and then wait the cooldowntime in seconds before setting
	cooldownHasEnded to true. This is checked for in randomlyPatrol() before allowing another call of this via a coroutine.*/
	IEnumerator shootAtPlayer()
	{
		cooldownHasEnded = false;
		GameObject go = (GameObject)Instantiate(projectile, projectileLaunchPoint.position, trans.rotation);
		go.GetComponent<Projectile>().dmg = damage;
		yield return new WaitForSeconds(cooldownTime);
		cooldownHasEnded = true;
	}

	void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            playerController.deductLives(damage);
        }
		
		if (col.gameObject.tag == "Player_Projectile")
		{
			takeDamage();
		}
    }

}


