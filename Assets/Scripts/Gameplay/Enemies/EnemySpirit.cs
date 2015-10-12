using UnityEngine;
using System.Collections;

public class EnemySpirit : Enemy 
{
	GameObject[] linkedSpirits = new GameObject[2];
	HeroStatus[] heroes;
	Transform target;
	Vector3 direction;

	float rotZ;

	const float MIN_DISTANCE = 2.5f;
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
				GameManager.Instance.m_iEnemiesOnScreen -= numberOfEssences;
			}
		}
	}


	private void LookForTarget()
	{
		foreach(HeroStatus hero in heroes)
		{
			if(target == null && hero.GetHealth() > 0)
			{
				target = hero.transform;
			}

			if(target != null && Vector2.Distance(transform.position,hero.transform.position) < Vector2.Distance(transform.position,target.transform.position))
			{
				if(hero.GetHealth() >0)
					target = hero.transform;
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D col2D)
	{
		if(col2D.gameObject.GetComponent<HeroStatus>())
		{
			this.GetComponent<CircleCollider2D>().enabled = false;
			Invoke("EnableCollider", TIME_LIMIT);
			HeroStatus player = col2D.gameObject.GetComponent<HeroStatus>();
			player.TakeDamage(1);

			if(player.GetHealth() <= 0)
				target = null;
		}
		else if(col2D.gameObject.GetComponent<EnemySpirit>())
		{
			linkedSpirits[0] = col2D.gameObject;
			linkedSpirits[1] = this.gameObject;

			Vector3 scale;

			if(this.transform.localScale.x > linkedSpirits[0].transform.localScale.x)
				scale = this.transform.localScale;
			else if(this.transform.localScale.x < linkedSpirits[0].transform.localScale.x)
				scale = linkedSpirits[0].transform.localScale;
			else
				scale = this.transform.localScale;

			this.transform.localScale = new Vector3(scale.x + 0.5f,scale.y+ 0.5f,scale.z+ 0.5f);
			this.numberOfEssences++;

			EnemySpirit[] allSpirits = FindObjectsOfType<EnemySpirit>();

			for(int i = 0; i < allSpirits.Length; i++)
			{
				foreach(GameObject go in linkedSpirits)
				{
					if(go == allSpirits[i].gameObject)
					{
						Destroy (go .gameObject);
						return;
					}
				}
			}
		}

	}

	private void EnableCollider()
	{
		this.GetComponent<CircleCollider2D>().enabled = true;
	}

	internal void SetDamage(int iDamage)
	{
		m_iHealth += iDamage;
	}
}
