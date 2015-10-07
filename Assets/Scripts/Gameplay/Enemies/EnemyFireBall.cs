using UnityEngine;
using System.Collections;

public class EnemyFireBall : MonoBehaviour 
{

	private void OnTriggerEnter2D(Collider2D col2D)
	{
		if(col2D.gameObject.GetComponent<HeroStatus>())
		{
			this.GetComponent<Collider2D>().enabled = false;
			HeroStatus player = col2D.gameObject.GetComponent<HeroStatus>();
			player.TakeDamage(1);
			Destroy(this.gameObject);
		}
	}
}
