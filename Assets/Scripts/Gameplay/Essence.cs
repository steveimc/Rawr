using UnityEngine;
using System.Collections;

public class Essence : MonoBehaviour 
{
	private HeroStatus[] heroes;
	Transform target;

	Vector3 direction;
	
	float rotZ;
	
	const float MIN_DISTANCE = 5.0f;
	const float MAX_SPEED = 5.0f;

	/*
	TODO: 
	Add Particle component
	 */
	private void Start()
	{
		transform.position = new Vector3(Random.Range(transform.position.x,transform.position.x+3),Random.Range(transform.position.y,transform.position.y + 3),transform.position.z);
	}
	private void Update()
	{
		if(heroes == null)
			heroes = FindObjectsOfType<HeroStatus>();
		else
		{
			LookForTarget();
		
			if(target)
			{
				if(Vector2.Distance(target.transform.position,this.transform.position) < MIN_DISTANCE)
				{
					transform.position = Vector3.Lerp(this.transform.position, target.position, Time.deltaTime * MAX_SPEED);
				}
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
		if(col2D.GetComponent<HeroStatus>())
		{
			OnCapture();
			AudioManager.instance.PlayWorld(Audio.Bank.ESSENCE);
			Destroy(this.gameObject);
		}
	}

	public void OnCapture()
	{
		GameManager.Instance.OnEssenceCaptured();
	}
}
