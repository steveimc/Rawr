﻿using UnityEngine;
using System.Collections;

public class EnemySpirit : MonoBehaviour 
{
	HeroStatus[] heroes;
	Transform target;
	Vector3 direction;

	int m_iHealth;

	float rotZ;

	const float MIN_DISTANCE = 0.5f;
	const float MAX_SPEED = 2.0f;
	const float TIME_LIMIT = 1.5f;


	private void Start()
	{
		m_iHealth = 1;
		heroes = FindObjectsOfType<HeroStatus>();
	}

	// Update is called once per frame
	private void Update () 
	{
		if(heroes == null)
			return;

		LookForTarget();


		if(target)
		{
			direction = target.transform.position - transform.position;
			direction.Normalize();

			rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler(0f, 0f, rotZ - 90);

			if(Vector2.Distance(target.transform.position,this.transform.position) > MIN_DISTANCE)
			{
				transform.position += transform.up*MAX_SPEED*Time.deltaTime;
			}
		}

		if(m_iHealth <= 0)
			Destroy(this.gameObject);
	}

	private void LookForTarget()
	{
		float distance = 0xffffff;

		foreach(HeroStatus hero in heroes)
		{
			if(Vector2.Distance(hero.transform.position,this.transform.position) < distance)
				target = hero.transform;
		}
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.GetComponent<HeroStatus>())
		{
			this.GetComponent<Collider2D>().enabled = false;
			Invoke("EnableCollider", TIME_LIMIT);
			HeroStatus player = col.gameObject.GetComponent<HeroStatus>();
			HUDController.instance.UpdateHeroHp(player.m_iHeroId,player.m_iHeroHealth--);
		}
	}

	private void EnableCollider()
	{
		this.GetComponent<Collider2D>().enabled = true;
	}
}
