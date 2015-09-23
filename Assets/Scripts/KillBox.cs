using UnityEngine;
using System.Collections;

public class KillBox : MonoBehaviour 
{
	public Transform spawnPoint;


	private void OnTriggerEnter2D(Collider2D col2D)
	{
		if(col2D.gameObject.GetComponent<HeroStatus>() != null)
		{
			col2D.transform.position = spawnPoint.position;
		}
	}
}
