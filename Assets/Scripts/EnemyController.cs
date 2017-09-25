using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour 
{
	enum Type {Melee, Ranged};

	[SerializeField] private Type enemyType = Type.Melee;

	private Transform trans;

	//Player References
	private GameObject playerObject;
	private PlayerController playerController;
	private Transform playerTransform;

	[SerializeField] private float movementSpeed = 0.1f;
	[SerializeField] private float rotationSpeed = 1;
	[SerializeField] private int damage = 1;
	[SerializeField] private int lives = 5;

	void Start()
	{
		//Assign the damage and lives of this enemy to the current wave number to provide level of abritry difficulty.
		damage = GameManager.singleton.wave;
		lives = GameManager.singleton.wave;

		trans = GetComponent<Transform>();

		//Load the player references once when the object starts.
		playerObject = GameObject.FindWithTag("Player");
		playerController = playerObject.GetComponent<PlayerController>();
		playerTransform = playerObject.GetComponent<Transform>();
	}

	void Update()
	{
		if (enemyType == Type.Melee)
		{
			followPlayer();
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
		lives -= playerController.damage;
	}


	void followPlayer()
	{
		//Rotate to look at the player.
		float angle = Mathf.Atan2(playerTransform.position.y - trans.position.y, 
		playerTransform.position.x - trans.position.x) * Mathf.Rad2Deg;
		trans.rotation = Quaternion.AngleAxis(angle + 90, new Vector3(0, 0, rotationSpeed));

		//Move towards the player at the given movementSpeed.
		float step = movementSpeed * Time.deltaTime;
		trans.position = Vector3.MoveTowards(trans.position, playerTransform.position, step);
	}
}
