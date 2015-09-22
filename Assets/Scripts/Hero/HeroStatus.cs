using UnityEngine;
using System.Collections;

public class HeroStatus : MonoBehaviour 
{
	internal int m_iHeroId;
	internal int m_iHeroHealth;
	internal int m_iFatigue;

	internal bool m_bHasSword;

	public GameObject m_FrostNova;

	HeroStatus()
	{
		m_iFatigue = 1000;
		m_iHeroHealth = 1000;
	}
	// Use this for initialization
	private void Start () 
	{
		m_iHeroId = GameManager.Instance.AssignPlayerId(this.gameObject.name);

		if(m_iHeroId == (int)GameManager.HeroId.PLAYER1)
		{
			m_bHasSword = true;
			ToggleSword();
		}
		else
		{
			m_bHasSword = false;
			ToggleSword();
		}
	}

	internal void ToggleSword()
	{
		m_FrostNova.SetActive(m_bHasSword);
	}
}
