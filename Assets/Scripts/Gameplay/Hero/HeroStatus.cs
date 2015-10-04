using UnityEngine;
using System.Collections;

public class HeroStatus : MonoBehaviour 
{
	internal int m_iHeroId = 1;
	internal int m_iHeroHealth;
	internal int m_iHeroEnergy;
	internal string m_sPlayerName;

	const int MAX_HEALTH = 10;
	const int MAX_ENERGY = 10;

	GameObject m_goFrostNova;

	//string playerName, int whichHero, int maxHealth, int maxFatigue

	internal bool m_bHasSword = true;

	private GameObject m_FrostNova;

	internal PlayerSyncInput syncInput;

	internal HeroBaseController m_Controller;

//	private GameObject[] playerObject = new GameObject[sizeof(HeroId)];

	HeroStatus()
	{
		m_iHeroHealth = MAX_HEALTH;
		m_iHeroEnergy = MAX_ENERGY;
	}

	// Use this for initialization
	private void Start () 
	{
		m_goFrostNova = GetComponentInChildren<FrostNova>().gameObject;
		// STEVE's previous code: m_iHeroId = GameManager.Instance.AssignPlayerId(this.gameObject.name);
		if(Game.Instance.IsLocalGame)
		{
			m_iHeroId = GameManager.Instance.PlayerJoined(this);
			GetComponent<PlayerSyncPosition>().enabled = false;
			GetComponent<PlayerSyncInput>().enabled = false;

				if (m_iHeroId ==1)
					FindObjectOfType<CameraController>().Init(this.transform);

			if(m_iHeroId == (int)HeroId.PLAYER1)
			{
				this.transform.GetChild(m_iHeroId).gameObject.SetActive(false);
				m_bHasSword = true;
				ToggleSword(); // m_bHasSword will be false and hide the sword	
			}
			else
			{
				this.transform.GetChild((int)HeroId.NONE).gameObject.SetActive(false);
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
				this.transform.GetChild((int)HeroId.NONE).gameObject.SetActive(false);
			}
		}

		this.gameObject.name = "Player" + m_iHeroId;

		m_sPlayerName = gameObject.name;
		HUDController.instance.InitUIPlayer(m_sPlayerName,m_iHeroId,MAX_HEALTH,MAX_ENERGY);
	}

	public void DropSword()
	{
		if(GameManager.Instance.m_FrostNovaCopy == null && m_bHasSword)
		{
			Quaternion rotation = Quaternion.Euler(320.0f,0,0);
			GameManager.Instance.m_FrostNovaCopy = Instantiate(m_goFrostNova, m_goFrostNova.transform.position, rotation) as GameObject;
			GameManager.Instance.m_FrostNovaCopy.transform.localScale = new Vector3(1.40f,1.40f,1.40f);
			GameManager.Instance.m_FrostNovaCopy.AddComponent<Rigidbody2D>();
			GameManager.Instance.m_FrostNovaCopy.AddComponent<EdgeCollider2D>();
			m_Controller.Flip(GameManager.Instance.m_FrostNovaCopy.transform);
			ToggleSword();
		}
		else if(GameManager.Instance.m_FrostNovaCopy != null)
		{
			if(GameManager.Instance.m_FrostNovaCopy != null)
			{
				Destroy(GameManager.Instance.m_FrostNovaCopy);
			}

			ToggleSword();
		}
	}

	public void ToggleSword()
	{
		if(GameManager.Instance.player[1] == null)
			SwitchSword();
		else
		{
			if(!GameManager.Instance.player[GetOtherPlayerId()].m_bHasSword)
				SwitchSword();
		}
	}

	private void SwitchSword()
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

	private int GetOtherPlayerId()
	{
		if(m_iHeroId == (int)HeroId.PLAYER1)
			return 1;
		else
			return 0;
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

	internal void ChangeHeroController()
	{
		if(m_bHasSword)
			m_Controller = GetComponent<HeroControllerSword>();
		else
			m_Controller = GetComponent<HeroControllerNoSword>();
	}
}


