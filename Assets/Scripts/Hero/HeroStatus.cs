using UnityEngine;
using System.Collections;

public class HeroStatus : MonoBehaviour 
{
	internal int m_iHeroId = 1;
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
		// STEVE's previous code: m_iHeroId = GameManager.Instance.AssignPlayerId(this.gameObject.name);
		if(Game.Instance.IsLocalGame)
		{
			m_iHeroId = GameManager.Instance.PlayerJoined();
			if(Game.Instance.IsLocalGame)
			{
				GetComponent<PlayerSyncPosition>().enabled = false;
			}
		}

		if(!Game.Instance.IsLocalGame)
		{
			if(this.GetComponent<PlayerSyncPosition>().isLocalPlayer)
			{
				m_iHeroId = 1;
			}
			else
			{
				m_iHeroId = 2;
			}
		}

		this.gameObject.name = "Player" + m_iHeroId;

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

	public void TakeDamage(int dmg)
	{
		m_iHeroHealth -= dmg;
	}
	
	public int GetHealth()
	{
		return m_iHeroHealth;
	}
	
	public int GetFatigue()
	{
		return m_iHeroHealth;
	}
}
