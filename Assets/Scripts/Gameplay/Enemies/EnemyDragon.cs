using UnityEngine;
using System.Collections;

public class EnemyDragon : MonoBehaviour 
{

	public enum Direction
	{
		LEFT,
		RIGHT
	}

	internal Direction m_Facing;
	private int m_iHealth;
	private float m_fShootingTimer;

	[SerializeField] Rigidbody2D m_FireBall;

	const float TIME_LIMIT = 1.5f;
	const int MAX_FORCE = 15;
	const int MIN_FORCE = 5;


	private void Start()
	{
		m_iHealth = 1;
		m_Facing = Direction.RIGHT;
	}

	// Update is called once per frame
	private void Update () 
	{
		if(m_iHealth <= 0)
			Destroy(this.gameObject);

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

		if(m_Facing == Direction.RIGHT)
		{
			fireBall.velocity = new Vector2(Random.Range(MIN_FORCE,MAX_FORCE),Random.Range(MIN_FORCE,MAX_FORCE));
		}
		else if(m_Facing == Direction.LEFT)
		{
			fireBall.velocity = new Vector2(-Random.Range(MIN_FORCE,MAX_FORCE),Random.Range(MIN_FORCE,MAX_FORCE));;
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

	internal virtual void Flip(Transform objectToFlip)
	{
		float fScale = objectToFlip.localScale.y;

		if(m_Facing == Direction.RIGHT)
			objectToFlip.localScale = new Vector3(fScale,fScale,fScale);
		else if(m_Facing == Direction.LEFT)
			objectToFlip.localScale = new Vector3(-fScale,fScale,fScale);

	}

	internal void SetDamage(int iDamage)
	{
		m_iHealth += iDamage;
	}
}
