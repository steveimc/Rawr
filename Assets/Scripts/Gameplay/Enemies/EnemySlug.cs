using UnityEngine;
using System.Collections;

public class EnemySlug : Enemy 
{
	const float MAX_SPEED = 3.0f;
	// Update is called once per frame

	void Start()
	{
		m_iHealth = 1;
	}

	void Update () 
	{
		transform.position += transform.right*MAX_SPEED*Time.deltaTime;

		if(m_iHealth <= 0)
		{
			Destroy(this.gameObject);
			GameManager.Instance.m_iEnemiesOnScreen--;
		}
	}

	private void OnTriggerEnter2D(Collider2D col2D)
	{
		if(col2D.gameObject.GetComponent<HeroStatus>())
		{
			HeroStatus player = col2D.gameObject.GetComponent<HeroStatus>();
			player.m_iHeroHealth--;
			HUDController.instance.UpdateHeroHp(player.m_iHeroId,player.m_iHeroHealth);
			GameManager.Instance.m_iEnemiesOnScreen--;
			Destroy(this.gameObject);
		}
	}
}
