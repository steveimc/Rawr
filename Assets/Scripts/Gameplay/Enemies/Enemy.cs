using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour 
{
	[SerializeField] Essence essence;
	protected int numberOfEssences = 1;
	protected const float TIME_LIMIT = 2.0f;

	protected int m_iHealth;

	internal void SetSwordDamage(int iDamage)
	{
		m_iHealth += iDamage;

		if(m_iHealth <= 0)
		{
			DropEssence();
		}
	}

	internal void SetNoSwordDamage(int iDamage)
	{
		m_iHealth += iDamage;
	}

	internal void DropEssence()
	{
		for(int i = 0; i < numberOfEssences; i++)
		{
			Instantiate(essence, transform.position, Quaternion.identity);
		}
	}
}
