﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour 
{
	private Rigidbody2D rb;
	private Transform trans;
	private float lifeSpan = 3;
	private float force = 5;

	[HideInInspector] public int dmg;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		trans = GetComponent<Transform>();
	}

	void Update()
	{
		launchProjectile();
	}
	void launchProjectile()
	{
		if (gameObject.tag == "Enemy_Projectile")
		{
			trans.Translate(new Vector3(0, -force, 0) * Time.deltaTime);
		}
		else
		{
			trans.Translate(new Vector3(-force, 0, 0) * Time.deltaTime);
		}
		
	}
	void killProjectile()
	{
		Destroy(gameObject);
		//Reset stats  and move object out of level here when using object pooling.
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.tag == "Player_Projectile" || col.gameObject.tag == "Enemy_Projectile")
		{
			return;
		}

		if (col.gameObject.tag == "Player")
		{
			col.gameObject.GetComponent<PlayerController>().deductLives(dmg);
		}

		killProjectile();
	}
}
