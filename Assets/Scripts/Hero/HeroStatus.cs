using UnityEngine;
using System.Collections;

public class HeroStatus : MonoBehaviour 
{
	internal int m_iHeroId = 1;
	internal int m_iHeroHealth;
	internal int m_iFatigue;

	internal bool m_bHasSword = true;

	private GameObject m_FrostNova;

	internal PlayerSyncInput syncInput;

	private GameObject[] playerObject = new GameObject[sizeof(GameManager.HeroId)];

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
			GetComponent<PlayerSyncPosition>().enabled = false;
			GetComponent<PlayerSyncInput>().enabled = false;

				if (m_iHeroId ==1)
					FindObjectOfType<CameraController>().Init(this.transform);

			if(m_iHeroId == (int)GameManager.HeroId.PLAYER1)
			{
				this.transform.GetChild(m_iHeroId).gameObject.SetActive(false);
				m_bHasSword = true;
				ToggleSword(); // m_bHasSword will be false and hide the sword	
			}
			else
			{
				this.transform.GetChild((int)GameManager.HeroId.NONE).gameObject.SetActive(false);
				m_bHasSword = true;
				ToggleSword();
			}
		}
		else
		{
			syncInput =  this.gameObject.GetComponent<PlayerSyncInput>();
			if(syncInput.isLocalPlayer)
			{
				m_iHeroId = 1;
				this.transform.GetChild(m_iHeroId).gameObject.SetActive(false);
				ToggleSword();
			}
			else
			{
				m_iHeroId = 2;
				this.transform.GetChild((int)GameManager.HeroId.NONE).gameObject.SetActive(false);
			}
		}
		this.gameObject.name = "Player" + m_iHeroId;
	}

	internal void ToggleSword()
	{
		m_bHasSword = !m_bHasSword;

		if(m_FrostNova == null)
		{
			m_FrostNova = GetComponentInChildren<FrostNova>().gameObject;
		}
		m_FrostNova.SetActive(m_bHasSword);

		// This shouldnt happen in remote cuz it checks isLocalplayer before sending
		if(syncInput != null)
		{
			if(m_bHasSword)
				syncInput.SendInput(UserInput.ToggleSwordON);
			else if(!m_bHasSword)
				syncInput.SendInput(UserInput.ToggleSwordOFF);
		}
	}

	internal void ToggleSwordFromNetwork(bool state)
	{
		m_bHasSword = state;
		if(m_FrostNova == null)
		{
			m_FrostNova = GetComponentInChildren<FrostNova>().gameObject;
		}
		m_FrostNova.SetActive(state);
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

	public void SendInput(UserInput animatorState)
	{
		if(syncInput != null)
			syncInput.SendInput(animatorState);
	}
}
