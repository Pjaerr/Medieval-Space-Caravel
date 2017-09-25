using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour 
{
	private Transform trans;
	private GameObject playerObject;
	private PlayerController playerController;

	[SerializeField] private int damage = 1;
	[SerializeField] private int lives = 5;

	void Start()
	{
		trans = GetComponent<Transform>();

		//Load the player references once when the object starts.
		playerObject = GameObject.FindWithTag("Player");
		playerController = playerObject.GetComponent<PlayerController>();
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
	
}
