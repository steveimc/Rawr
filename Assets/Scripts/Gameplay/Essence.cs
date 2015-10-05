using UnityEngine;
using System.Collections;

public class Essence : MonoBehaviour 
{
	/*
	TODO: 
	Add Particle component
	 */
	private void OnTriggerEnter2D(Collider2D col2D)
	{
		if(col2D.GetComponent<HeroStatus>())
		{
			OnCapture();
			Destroy(this.gameObject);
		}
	}

	public void OnCapture()
	{
		GameManager.Instance.OnEssenceCaptured();
	}
}
