using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour 
{
	private Portal[] m_Portals;
	private bool m_bTeleport = true;
	internal bool  m_HeroCanTeleport = false;

	private Portal m_linkedPortal;

	const int LIFETIME = 10;

	// Use this for initialization
	void Awake() 
	{
		m_Portals = FindObjectsOfType<Portal>();

		foreach(Portal portal in m_Portals)
		{
			portal.gameObject.GetComponent<Renderer>().enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void OnTriggerEnter2D(Collider2D col2D)
	{
		if(!m_bTeleport)
			return;
		
		if(col2D.gameObject.GetComponent<HeroStatus>() && m_HeroCanTeleport)
		{
			m_linkedPortal.SetTeleport(false);
			col2D.transform.position = m_linkedPortal.transform.position;
		}
		else if(col2D.gameObject.GetComponent<EnemySlug>())
		{
			int iRandom = SelectRandom();

			if(m_linkedPortal)
			{
				m_linkedPortal.GetComponent<Renderer>().enabled = false;
				m_linkedPortal.m_HeroCanTeleport = false;
			}
			m_Portals[iRandom].GetComponent<Renderer>().enabled = true;
			this.GetComponent<Renderer>().enabled = true;

			m_HeroCanTeleport = true;
			m_Portals[iRandom].m_HeroCanTeleport = true;

			m_linkedPortal = m_Portals[iRandom];
			m_Portals[iRandom].m_linkedPortal = this;

			m_linkedPortal.SetTeleport(false);

			Invoke("DisablePortals",LIFETIME);
			col2D.transform.position = m_linkedPortal.transform.position;
			col2D.transform.rotation = m_linkedPortal.transform.rotation;
		}
	}

	private void DisablePortals()
	{
		m_linkedPortal.GetComponent<Renderer>().enabled = false;
		m_linkedPortal.m_HeroCanTeleport = false;

		m_linkedPortal.m_linkedPortal.m_HeroCanTeleport = false;
		m_linkedPortal.m_linkedPortal.GetComponent<Renderer>().enabled = false;
	}

	private void OnTriggerExit2D(Collider2D col2D)
	{
		if(col2D.gameObject.GetComponent<HeroStatus>())
		{
			SetTeleport(true);
		}
		else if(col2D.gameObject.GetComponent<EnemySlug>())
		{
			SetTeleport(true);
		}
	}

	private int SelectRandom()
	{
		int iRandom = 0;

		for(int i = 0; i < m_Portals.Length; i++)
		{
			iRandom = Random.Range(0,m_Portals.Length);

			while(m_Portals[iRandom] == this || m_Portals[iRandom].m_HeroCanTeleport)
			{
				iRandom = Random.Range(0,m_Portals.Length);
			}
		}

		return iRandom;
	}

	public void SetTeleport(bool bStatus)
	{
		m_bTeleport = bStatus;
	}
}
