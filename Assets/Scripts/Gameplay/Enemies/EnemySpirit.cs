using UnityEngine;
using System.Collections;

public class EnemySpirit : Enemy 
{
	HeroStatus[] heroes;
	Transform target;
	Vector3 direction;

	float rotZ;

	const float MIN_DISTANCE = 0.5f;
	const float MAX_SPEED = 2.0f;

	private void Start()
	{
		m_iHealth = 1;
	}

	// Update is called once per frame
	private void Update () 
	{
		if(heroes == null)
		{
			heroes = FindObjectsOfType<HeroStatus>();
		}

		if(heroes != null)
		{
			LookForTarget();


			if(target)
			{
				direction = target.transform.position - transform.position;
				direction.Normalize();

				rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
				Quaternion rotation = Quaternion.Euler(0f, 0f, rotZ - 90);

				transform.rotation = Quaternion.Slerp(this.transform.rotation,rotation,0.1f);

				if(Vector2.Distance(target.transform.position,this.transform.position) > MIN_DISTANCE)
				{
					transform.position += transform.up*MAX_SPEED*Time.deltaTime;
				}
			}

			if(m_iHealth <= 0)
			{
				Destroy(this.gameObject);
				GameManager.Instance.m_iEnemiesOnScreen--;
			}
		}
	}


	private void LookForTarget()
	{
		foreach(HeroStatus hero in heroes)
		{
			if(target == null)
			{
				target = hero.transform;
			}

			if(Vector2.Distance(transform.position,hero.transform.position) < Vector2.Distance(transform.position,target.transform.position))
			{
				target = hero.transform;
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D col2D)
	{
		if(col2D.gameObject.GetComponent<HeroStatus>())
		{
			this.GetComponent<Collider2D>().enabled = false;
			Invoke("EnableCollider", TIME_LIMIT);
			HeroStatus player = col2D.gameObject.GetComponent<HeroStatus>();
			player.m_iHeroHealth--;
			HUDController.instance.UpdateHeroHp(player.m_iHeroId,player.m_iHeroHealth);
		}
	}

	private void EnableCollider()
	{
		this.GetComponent<Collider2D>().enabled = true;
	}

	internal void SetDamage(int iDamage)
	{
		m_iHealth += iDamage;
	}
}
