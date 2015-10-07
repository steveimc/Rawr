using UnityEngine;
using System.Collections;

public class EnemyDragon : Enemy 
{
	private float m_fShootingTimer;

	[SerializeField] Rigidbody2D m_FireBall;

	const int MAX_FORCE = 15;
	const int MIN_FORCE = 5;


	private void Start()
	{
		m_iHealth = 1;
	}

	// Update is called once per frame
	private void Update () 
	{
		if(m_iHealth <= 0)
		{
			Destroy(this.gameObject);
			GameManager.Instance.m_iEnemiesOnScreen--;
		}

		m_fShootingTimer += Time.deltaTime;

		if(m_fShootingTimer > Random.Range(1,5))
		{
			ShootFireBall();
		}
	}

	private void ShootFireBall()
	{
		m_fShootingTimer = 0;

		Rigidbody2D fireBall;
		fireBall = Instantiate(m_FireBall, transform.position, transform.rotation) as Rigidbody2D;

		fireBall.velocity = new Vector2(transform.right.x * Random.Range(MIN_FORCE,MAX_FORCE),Random.Range(MIN_FORCE,MAX_FORCE));

	}

	private void OnTriggerEnter2D(Collider2D col2D)
	{
		if(col2D.gameObject.GetComponent<HeroStatus>())
		{
			this.GetComponent<Collider2D>().enabled = false;
			Invoke("EnableCollider", TIME_LIMIT);
			HeroStatus player = col2D.gameObject.GetComponent<HeroStatus>();
			player.TakeDamage(1);
		}
	}

	private void EnableCollider()
	{
		this.GetComponent<Collider2D>().enabled = true;
	}
}
