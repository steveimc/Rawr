using UnityEngine;
using System.Collections;

public class HeroStatus : MonoBehaviour 
{
	internal int m_iHeroId = 1;
	internal int m_iHeroHealth;
	internal int m_iHeroEnergy;
	internal string m_sPlayerName;

	const int MAX_HEALTH = 5;
	const int MAX_ENERGY = 100;

	GameObject m_goFrostNova;
	Renderer[] renderers;

	private bool m_bHasSword = false;

	private GameObject m_FrostNova;

	internal PlayerSyncInput syncInput;

	internal HeroBaseController m_Controller;

	private float m_EnergyTimer = 0;

	HeroStatus()
	{
		m_iHeroHealth = MAX_HEALTH;
		m_iHeroEnergy = MAX_ENERGY;
	}

	// Use this for initialization
	private void Start () 
	{
		m_goFrostNova = GetComponentInChildren<FrostNova>().gameObject;
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

		renderers = GetComponentsInChildren<Renderer>();
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
		HUDController.instance.UpdateHasSword(m_iHeroId, m_bHasSword);

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
		HUDController.instance.UpdateHeroHp(m_iHeroId, m_iHeroHealth);

		if(m_iHeroHealth > 0)
		{
			foreach(Renderer r in renderers)
			{
				if(r.material.shader.name == "Unlit/HeroShader")
					r.material.shader = Resources.Load("HeroShaderAnimated") as Shader;
			}

			Invoke("ReturnToNormalShader", 2.0f);
		}
		else if(m_iHeroHealth <= 0)
		{
			if(m_bHasSword)
			{
				DropSword();
				ChangeHeroController();
			}
			Die ();
		}
	}

	private void ReturnToNormalShader()
	{
		foreach(Renderer r in renderers)
		{
			if(r.material.shader.name == "Unlit/HeroShaderAnimated")
				r.material.shader = Resources.Load("HeroShader") as Shader;
		}
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

	public bool GetHasSword()
	{
		return m_bHasSword;
	}

	public void RestoreHeroHealth()
	{
		m_iHeroHealth = MAX_HEALTH;
		foreach(Renderer r in renderers)
		{
			r.enabled = true;
		}
	}

	private void Die()
	{
		GameManager.Instance.HeroDied();
		foreach(Renderer r in renderers)
		{
			r.enabled = false;
		}
	}

	private void Update()
	{
		m_EnergyTimer += Time.deltaTime;

		if(m_EnergyTimer > 1)
		{
			m_EnergyTimer = 0;

			if(m_bHasSword)
			{
				m_iHeroEnergy -= 10;
				if(m_iHeroEnergy < 0)
				{
					m_iHeroEnergy = 0;
					if(!m_Controller.m_isSpinning)
					{
						DropSword();
						ChangeHeroController();
					}
				}
				HUDController.instance.UpdateHeroEnergy(m_iHeroId, m_iHeroEnergy);
			}
			else
			{
				m_iHeroEnergy += 10;
				if(m_iHeroEnergy > MAX_ENERGY)
					m_iHeroEnergy = MAX_ENERGY;
				
				HUDController.instance.UpdateHeroEnergy(m_iHeroId, m_iHeroEnergy);
			}
		}
	}
}


