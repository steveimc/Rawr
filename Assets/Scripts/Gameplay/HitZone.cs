using UnityEngine;
using System.Collections;

public class HitZone : MonoBehaviour 
{

	private void OnTriggerEnter2D(Collider2D col2D)
	{
		if(col2D.GetComponent<Enemy>())
		{
			col2D.GetComponent<Enemy>().SetSwordDamage(-1);
		}
	}
}
