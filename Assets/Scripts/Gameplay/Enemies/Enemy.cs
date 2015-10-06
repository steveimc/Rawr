using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour 
{
	[SerializeField] Essence essence;

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
		Instantiate(essence, transform.position, Quaternion.identity);
	}
}
